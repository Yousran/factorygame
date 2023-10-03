using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System;

public class MeshGen
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    public GameObject ChunkObject;
    MeshFilter meshFilter;
    MeshCollider MeshCol;
    MeshRenderer meshRend;
    public static int ChunkSizeX = IslandGen.MapSizeX / 10;
    public static int ChunkSizeY = IslandGen.MapSizeY;
    public static int ChunkSizeZ = IslandGen.MapSizeZ / 10;
    Vector3Int ChunkPosition;

    float[,,] MapData = IslandGen.DataMap;

    void ClearMeshData()
    {
        vertices.Clear();
        triangles.Clear();
    }

    void BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        MeshCol.sharedMesh = mesh;

    }

    public MeshGen(Vector3Int _Position)
    {
        ChunkObject = new GameObject();
        ChunkObject.name = string.Format("Chunk {0}, {1}", _Position.x, _Position.z);
        ChunkPosition = _Position;
        ChunkObject.transform.position = ChunkPosition;
        meshFilter = ChunkObject.AddComponent<MeshFilter>();
        MeshCol = ChunkObject.AddComponent<MeshCollider>();
        meshRend = ChunkObject.AddComponent<MeshRenderer>();
        meshRend.material = Resources.Load<Material>("Materials/TerrainMaterial");
        ChunkObject.transform.tag = "Terrain";
        ChunkObject.layer = 6;

        ClearMeshData();
        BuatMeshData(_Position);
        BuildMesh();

        ChunkObject.GetComponent<MeshRenderer>().enabled = false;
    }

    int GetCubeConfig(float[] cube)
    {
        int ConfigIndex = 0;
        for (int i = 0; i < 8; i++)
        {
            if (cube[i] > 0)
            {
                ConfigIndex |= 1 << i;
            }
        }
        return ConfigIndex;
    }

    public void PlaceTerrain(Vector3 Pos)
    {
        Vector3Int vector3Int = new Vector3Int(Mathf.CeilToInt(Pos.x), Mathf.CeilToInt(Pos.y), Mathf.CeilToInt(Pos.z));
        MapData[vector3Int.x, vector3Int.y, vector3Int.z] = 0f;
        ClearMeshData();
        vector3Int = ChunkPosition;
        BuatMeshData(vector3Int);
        BuildMesh();
    }
    public void RemoveTerrain(Vector3 Pos)
    {
        Vector3Int vector3Int = new Vector3Int(Mathf.CeilToInt(Pos.x), Mathf.CeilToInt(Pos.y), Mathf.CeilToInt(Pos.z));
        MapData[vector3Int.x, vector3Int.y, vector3Int.z] = 1;
        MapData[vector3Int.x + 1, vector3Int.y + 1, vector3Int.z + 1] = 1;
        MapData[vector3Int.x - 1, vector3Int.y - 1, vector3Int.z - 1] = 1;
        ClearMeshData();
        vector3Int = ChunkPosition;
        BuatMeshData(vector3Int);
        BuildMesh();
    }


    void MarchCube(Vector3 Position, float[] Cube)
    {
        int ConfigIndex = GetCubeConfig(Cube);
        //Debug.Log(ConfigIndex);
        if (ConfigIndex == 0 || ConfigIndex == 255)
        {
            return;
        }
        int EdgeIndex = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int Indice = IslandGen.TriangleTable[ConfigIndex, EdgeIndex];
                if (Indice == -1)
                {
                    return;
                }
                Vector3 vert1 = Position + IslandGen.EdgeTable[Indice, 0];
                Vector3 vert2 = Position + IslandGen.EdgeTable[Indice, 1];

                Vector3 finalvert = (vert1 + vert2) / 2f;

                vertices.Add(finalvert);
                triangles.Add(vertices.Count - 1);
                EdgeIndex++;
            }
        }
    }

    void BuatMeshData(Vector3 Position)
    {
        for (int x = 0; x < ChunkSizeX; x++)
        {
            for (int z = 0; z < ChunkSizeZ; z++)
            {
                for (int y = 0; y < ChunkSizeY; y++)
                {
                    float[] cube = new float[8];
                    for (int i = 0; i < 8; i++)
                    {
                        Vector3Int corner = new Vector3Int(x, y, z) + IslandGen.CornerTable[i];
                        cube[i] = MapData[corner.x + (int)Position.x, corner.y, corner.z + (int)Position.z];
                    }
                    MarchCube(new Vector3(x, y, z), cube);
                }
            }
        }

    }

}
