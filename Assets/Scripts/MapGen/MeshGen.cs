using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    MeshFilter meshFilter;
    MeshCollider MeshCol;

    float[,,] MapData;

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
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCol = GetComponent<MeshCollider>();
        MapData = IslandGen.IslandData();
        transform.tag = "Terrain";
        ClearMeshData();
        BuatMeshData();
        BuildMesh();
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
        Vector3Int vector3Int = new Vector3Int(Mathf.CeilToInt(Pos.x),Mathf.CeilToInt(Pos.y),Mathf.CeilToInt(Pos.z));
        IslandGen.DataMap[vector3Int.x, vector3Int.y, vector3Int.z] = 0f;
        ClearMeshData();
        BuatMeshData();
        BuildMesh();
    }
    public void RemoveTerrain(Vector3 Pos)
    {
        Vector3Int vector3Int = new Vector3Int(Mathf.CeilToInt(Pos.x), Mathf.CeilToInt(Pos.y), Mathf.CeilToInt(Pos.z));
        MapData[vector3Int.x, vector3Int.y, vector3Int.z] = 1;
        MapData[vector3Int.x+1, vector3Int.y+1, vector3Int.z+1] = 1;
        MapData[vector3Int.x-1, vector3Int.y-1, vector3Int.z - 1] = 1;
        ClearMeshData();
        BuatMeshData();
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

    void BuatMeshData()
    {
     //Debug.Log(islandGen.MapSizeX);
            for (int x = 0; x < IslandGen.MapSizeX; x++)
            {
                for (int z = 0; z < IslandGen.MapSizeZ; z++)
                {
                    for (int y = 0; y < IslandGen.MapSizeY; y++)
                    {
                        float[] cube = new float[8];
                        for (global::System.Int32 i = 0; i < 8; i++)
                        {
                            Vector3Int corner = new Vector3Int(x, y, z) + IslandGen.CornerTable[i];
                            cube[i] = MapData[corner.x, corner.y, corner.z];
                        }
                        MarchCube(new Vector3(x, y, z), cube);
                    }
                }
            }
        
    }
}
