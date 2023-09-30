using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public string Seed;
    public bool RandomSeed = true;
    public int SizeX = 10;
    public int SizeY = 10;
    public int SizeZ = 10;
    public int ChunkSize = 0;

    Dictionary<Vector3Int,MeshGen> Chunks = new Dictionary<Vector3Int,MeshGen>();
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
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                Vector3Int Chunkpos = new Vector3Int(x * IslandGen.MapSizeX/ChunkSize, 0, z * IslandGen.MapSizeZ/ChunkSize);
                Chunks.Add(Chunkpos, new MeshGen(Chunkpos));
                Chunks[Chunkpos].ChunkObject.transform.SetParent(transform);
            }
        }
    }

    public MeshGen GetChunkFromV3(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        return Chunks[new Vector3Int(x, y, z)];
    }
}
