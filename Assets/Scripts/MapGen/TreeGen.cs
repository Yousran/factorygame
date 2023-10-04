using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public struct JobTreeGen : IJobParallelFor
{
    public NativeArray<float> IsSpawn;
    public void Execute(int index)
    {
        int x = index % MapGen.SizeX;
        int z = index / MapGen.SizeX;
        IsSpawn[index] = TreeGen.IsSpawn(x,z) < 50f ? 1 : 0;
    }
}
public class TreeGen
{
    public static float[,] TreesSpawnNoise;
    public static void TreeSpawnCoord()
    {
        int ArraySize = IslandGen.MapSizeX * IslandGen.MapSizeZ;

        NativeArray<float> nativeData = new NativeArray<float>(ArraySize, Allocator.TempJob);

        JobTreeGen job = new JobTreeGen()
        {
            IsSpawn = nativeData,
        };
        JobHandle jobHandle = job.Schedule(ArraySize, 32);
        jobHandle.Complete();
        TreesSpawnNoise = new float[IslandGen.MapSizeX, IslandGen.MapSizeZ];
        for (int x = 0; x < IslandGen.MapSizeX; x++)
        {
            for (int z = 0; z < IslandGen.MapSizeZ; z++)
            {
                int Index = x + z * IslandGen.MapSizeX;
                TreesSpawnNoise[x,z] = nativeData[Index];
            }
        }
        nativeData.Dispose();
    }
    public static float IsSpawn(int x,int z)
    {
        System.Random SeededRandom = new System.Random(46);
        float value2 = Mathf.PerlinNoise(x, z) + (float)SeededRandom.NextDouble() * 0.8f;
        return value2;
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
                this.transform.position = new Vector3(transform.position.x,hit.normal.y,transform.position.z);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
