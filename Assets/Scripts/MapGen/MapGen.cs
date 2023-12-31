using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public string Seed;
    public bool RandomSeed = true;

    public static int SizeX = 1000;

    public static int SizeY = 100;

    public static int SizeZ = 1000;

    public static int _ChunkSize = 100;
    public int ChunkSize { get; set; } = _ChunkSize;

    public Trees[] TreeToSpawn;

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
        //generate island mesh and chunk
        Debug.Log("Generating Noise");
        IslandGen.IslandData();
        Debug.Log("Finished Generating Noise");

        Debug.Log("Generating Mesh");
        GenerateChunk();
        Debug.Log("Finished Generating Mesh");

        Debug.Log("Generating Trees");
        GenerateTrees();
        Debug.Log("Finished Generating Trees");

        PortGen portGenerator = GetComponent<PortGen>();
        portGenerator.SpawnPort(SizeX * (int)transform.localScale.x, SizeZ * (int)transform.localScale.z);
    }

    void GenerateTrees()
    {
        //generate trees
        for (int x = 0; x < SizeX; x++)
        {
            for (int z = 0; z < SizeZ; z++)
            {
                float Spawn = Random.Range(0f, 1f);
                float Noise = IslandGen.Noise(x, z);
                for (int i = 0; i < TreeToSpawn.Length; i++)
                {
                    if (Spawn <= TreeToSpawn[i].SpawnRate)
                    {
                        Trees ChoosenTreeToSpawn = TreeToSpawn[i];
                        int NumWoodToSpawn = Random.Range(ChoosenTreeToSpawn.MinWoodSpawn, ChoosenTreeToSpawn.MaxWoodSpawn);
                        int WoodPrefabToSpawn = Random.Range(0,ChoosenTreeToSpawn.PrefabPohon.Length);
                        if (Noise <= ChoosenTreeToSpawn.MaxVerticalSpawnRate 
                            && Noise >= ChoosenTreeToSpawn.MinVerticalSpawnRate)
                        {
                            GameObject InstantiatedTree = Instantiate(ChoosenTreeToSpawn.PrefabPohon[WoodPrefabToSpawn], new Vector3(x, transform.position.y + SizeY, z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                            InstantiatedTree.AddComponent<TreeStats>();
                            InstantiatedTree.GetComponent<TreeStats>().HealthPohon = ChoosenTreeToSpawn.HealthPohon;
                            InstantiatedTree.GetComponent<TreeStats>().MaxHealthPohon = ChoosenTreeToSpawn.HealthPohon;
                            InstantiatedTree.GetComponent<TreeStats>().WoodToSpawn = NumWoodToSpawn;
                            InstantiatedTree.GetComponent<TreeStats>().WoodPrefabs = ChoosenTreeToSpawn.PrefabKayu;
                            InstantiatedTree.GetComponent<TreeStats>().HargaWood = ChoosenTreeToSpawn.HargaKayu;
                            InstantiatedTree.AddComponent<MoveDown>();
                            InstantiatedTree.transform.SetParent(transform.GetChild(1));
                        }
                    }
                }
            }
        }
    }

    void GenerateChunk()
    {
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                Vector3Int Chunkpos = new Vector3Int(x * IslandGen.MapSizeX / ChunkSize, 0, z * IslandGen.MapSizeZ / ChunkSize);
                Chunks.Add(Chunkpos, new MeshGen(Chunkpos));
                Chunks[Chunkpos].ChunkObject.transform.SetParent(transform.GetChild(0));
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + SizeX / 2, transform.position.y + SizeY / 2, transform.position.z + SizeZ / 2), new Vector3(SizeX, SizeY, SizeZ));
    }
}