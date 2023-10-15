using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class TreeStats : MonoBehaviour
{
    public float HealthPohon;
    public float MaxHealthPohon;
    public int WoodToSpawn;
    public GameObject[] WoodPrefabs;
    public int HargaWood;
    public void TreeIsChopped()
    {
        for (int i = 0; i < WoodToSpawn; i++)
        {
            GameObject SpawnedWood = 
                Instantiate(WoodPrefabs[UnityEngine.Random.Range(0,WoodPrefabs.Length)],
                new Vector3(transform.position.x, transform.position.y+1, transform.position.z),quaternion.identity);
            SpawnedWood.AddComponent<WoodStat>();
            SpawnedWood.AddComponent<ObjectDragging>();
            SpawnedWood.GetComponent<WoodStat>().HargaWood = HargaWood;
        }
        Destroy(gameObject);
    }

}
public class WoodStat : MonoBehaviour
{
    public int HargaWood;
}
public class MoveDown : MonoBehaviour
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
