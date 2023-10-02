using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;

public class MeshGen : IDisposable
{
    NativeList<Vector3> vertices;
    NativeList<int> triangles;
    public GameObject ChunkObject;
    MeshFilter meshFilter;
    MeshCollider MeshCol;
    MeshRenderer meshRend;
    public static int ChunkSizeX = IslandGen.MapSizeX / 10;
    public static int ChunkSizeY = IslandGen.MapSizeY;
    public static int ChunkSizeZ = IslandGen.MapSizeZ / 10;
    public static int[][] TriangleTable;
    public static int[][] EdgeTable;
    Vector3Int ChunkPosition;

    float[,,] MapData = IslandGen.IslandData();

    void ClearMeshData()
    {
        vertices.Clear();
        triangles.Clear();
    }

    void BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices.ToArray(Allocator.Persistent).ToArray();
        mesh.triangles = triangles.ToArray(Allocator.Persistent).ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        MeshCol.sharedMesh = mesh;

    }

    // Start is called before the first frame update
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

        vertices = new NativeList<Vector3>(Allocator.Persistent);
        triangles = new NativeList<int>(Allocator.Persistent);
        TriangleTable = new int[256][];
        for (int i = 0; i < 256; i++)
        {
            TriangleTable[i] = new int[16];
            for (int j = 0; j < 16; j++)
            {
                // Populate TriangleTable with your values from the original multi-dimensional array
                TriangleTable[i][j] = IslandGen.TriangleTable[i, j];/* your values here */;
            }
        }
        BuatMeshData(_Position);
        BuildMesh();

        ChunkObject.GetComponent<MeshRenderer>().enabled = false;
    }

    static int GetCubeConfig(float[] cube)
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
        MapData[vector3Int.x, vector3Int.y, vector3Int.z] = 0f;
        ClearMeshData();
        vector3Int = ChunkPosition;
        BuatMeshData(vector3Int);
        BuildMesh();
    }
    public void RemoveTerrain(Vector3 Pos)
    {
        Vector3Int vector3Int = new Vector3Int(Mathf.CeilToInt(Pos.x), Mathf.CeilToInt(Pos.y), Mathf.CeilToInt(Pos.z));
        Debug.Log(vector3Int.ToString());
        MapData[vector3Int.x, vector3Int.y, vector3Int.z] = 1;
        MapData[vector3Int.x+1, vector3Int.y+1, vector3Int.z+1] = 1;
        MapData[vector3Int.x-1, vector3Int.y-1, vector3Int.z - 1] = 1;
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
                triangles.Add(vertices.Length - 1);
                EdgeIndex++;
            }
        }
    }

    void BuatMeshData(Vector3 Position)
    {
        NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(Allocator.Temp);
        NativeList<MeshGenJob> jobs = new NativeList<MeshGenJob>(Allocator.Temp);

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
                    float3 cubeFloat3 = new float3(cube[0], cube[1], cube[2]); // Mengonversi ke float3
                    var job = new MeshGenJob
                    {
                        Position = new float3(x, y, z),
                        Cube = cubeFloat3, // Menggunakan cubeFloat3 yang sudah dikonversi
                        Vertices = vertices,
                        Triangles = triangles
                    };

                    jobs.Add(job);
                }
            }
        }

        // Memulai pekerjaan dalam multithreading
        for (int i = 0; i < jobs.Length; i++)
        {
            jobHandles.Add(jobs[i].Schedule());
        }

        // Menunggu hingga semua pekerjaan selesai
        JobHandle.CompleteAll(jobHandles);

        // Dispose jobHandles dan jobs
        jobHandles.Dispose();
        jobs.Dispose();
    }
    [BurstCompile]
    private struct MeshGenJob : IJob
    {
        public float3 Position;
        public float3 Cube; // Menggunakan float3 untuk Cube

        [WriteOnly]
        public NativeList<Vector3> Vertices;
        [WriteOnly]
        public NativeList<int> Triangles;

        public void Execute()
        {
            float[] cubeArray = new float[] { Cube.x, Cube.y, Cube.z };
            int ConfigIndex = GetCubeConfig(cubeArray);

            if (ConfigIndex == 0 || ConfigIndex == 255)
            {
                return;
            }

            int EdgeIndex = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int Indice = TriangleTable[ConfigIndex][EdgeIndex];
                    if (Indice == -1)
                    {
                        return;
                    }
                    Vector3 vert1 = new Vector3(Position.x, Position.y, Position.z) + IslandGen.EdgeTable[Indice, 0];
                    Vector3 vert2 = new Vector3(Position.x, Position.y, Position.z) + IslandGen.EdgeTable[Indice, 1];

                    Vector3 finalvert = (vert1 + vert2) / 2f;

                    Vertices.Add(finalvert);
                    Triangles.Add(Vertices.Length - 1);
                    EdgeIndex++;
                }
            }
        }
    }
        public void Dispose()
    {
        if (vertices.IsCreated)
            vertices.Dispose();

        if (triangles.IsCreated)
            triangles.Dispose();
    }

}
