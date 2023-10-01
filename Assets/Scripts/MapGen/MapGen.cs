using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using System.Threading;

using static IslandGen;
public class MapGen : MonoBehaviour
{
    public string Seed;
    public bool RandomSeed = true;
    public int SizeX = 10;
    public int SizeY = 10;
    public int SizeZ = 10;
    public int ChunkSize = 0;

    Dictionary<Vector3Int,MeshGen> Chunks = new Dictionary<Vector3Int,MeshGen>();

    private JobHandle generateJobHandle;
    private bool jobCompleted = false;
    // Start is called before the first frame update
    void Start()
    {
        IslandGen.MapSizeX = SizeX;
        IslandGen.MapSizeY = SizeY;
        IslandGen.MapSizeZ = SizeZ;
        if (RandomSeed)
        {
            IslandGen.Seed = Time.realtimeSinceStartup.ToString();
        }
        else
        {
            IslandGen.Seed = Seed;
        }
        //MeshGen.ChunkSizeX = IslandGen.MapSizeX / ChunkSize;
        //MeshGen.ChunkSizeZ = IslandGen.MapSizeZ / ChunkSize;
        GenerateChunk();    
    }


    void GenerateChunk()
    {
        foreach (var chunkPos in Chunks.Keys)
        {
            MeshGenJob job = new MeshGenJob();
            job.Initialize(chunkPos, IslandGen.IslandData());
            generateJobHandle = job.Schedule(generateJobHandle);
        }

        StartCoroutine(WaitForJob());
    }
    IEnumerator WaitForJob()
    {
        yield return new WaitUntil(() => generateJobHandle.IsCompleted);

        generateJobHandle.Complete();

        foreach (var chunkPos in Chunks.Keys)
        {
            Chunks[chunkPos].BuildMesh();
        }

        jobCompleted = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + SizeX / 2, transform.position.y + SizeY / 2, transform.position.z + SizeZ / 2), new Vector3(SizeX, SizeY, SizeZ));
    }

    public MeshGen GetChunkFromV3(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        return Chunks[new Vector3Int(x, y, z)];
    }
}
