using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public string Seed;
    public bool RandomSeed = true;

    public static int SizeX = 200;

    public static int SizeY = 50;

    public static int SizeZ = 200;

    public int ChunkSize = 10;

    public GameObject[] TreePrefabs;

    Dictionary<Vector3Int, MeshGen> Chunks = new Dictionary<Vector3Int, MeshGen>();
    // Start is called before the first frame update
    void Start()
    {
        if (RandomSeed)
        {
            IslandGen.Seed = Time.realtimeSinceStartup.ToString();
        }
        else
        {
            IslandGen.Seed = Seed;
        }

        IslandGen.IslandData();

        GenerateChunk();
        //TreeGen.TreeSpawnCoord();
        //for (int x = 0; x < SizeX; x++)
        //{
        //    for (int z = 0; z < SizeZ; z++)
        //    {
        //        if (TreeGen.TreesSpawnNoise[x,z] == 1)
        //        {
        //            GameObject InstantiatedTree = Instantiate(TreePrefabs[0], new Vector3(x, transform.position.y + SizeY, z), Quaternion.identity);
        //            InstantiatedTree.AddComponent<TreeMoveDown>();
        //       }
        //    }
        //}
    }


    void GenerateChunk()
    {
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                Vector3Int Chunkpos = new Vector3Int(x * IslandGen.MapSizeX / ChunkSize, 0, z * IslandGen.MapSizeZ / ChunkSize);
                Chunks.Add(Chunkpos, new MeshGen(Chunkpos));
                Chunks[Chunkpos].ChunkObject.transform.SetParent(transform);
            }
        }
        Debug.Log("Map Loaded!");
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