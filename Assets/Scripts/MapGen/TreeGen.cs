using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


public class TreeGen
{
    public static int TreeSpawnMap(int x,int z)
    {
        float noise = IslandGen.Noise(x, z);
        if (noise != 0)
        {
            if (noise > 0.5f)
            {
                return 2;
            }
            if (noise <= 0.5f && noise > 0.06f)
            {
                return 3;
            }
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

public class TreeStats : MonoBehaviour
{
    public float HealthPohon;
    public int WoodToSpawn;
    public GameObject[] WoodPrefabs;
    public void TreeIsChopped()
    {
        for (int i = 0; i < WoodToSpawn; i++)
        {
            GameObject SpawnedWood = 
                Instantiate(WoodPrefabs[UnityEngine.Random.Range(0,WoodPrefabs.Length)],
                transform.position,quaternion.identity);
            
        }
        Destroy(gameObject);
    }

}
public class TreeMoveDown : MonoBehaviour
{
    RaycastHit hit;
    private void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.transform.tag == "Terrain" && Vector3.Angle(hit.normal, Vector3.up) == 0)
            {
                this.transform.position = new Vector3(transform.position.x,hit.point.y,transform.position.z);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
