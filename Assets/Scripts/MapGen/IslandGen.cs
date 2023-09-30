using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class IslandGen : MonoBehaviour
{
    public GameObject kubus;
    public int MapSizeX;
    public int MapSizeY;
    public int MapSizeZ;

    public string Seed;
    public bool RandomSeed = true;
    public float Scale = 1;
    public float OffsetX;
    public float OffsetZ;

    public float Scale2 = 1;
    public float OffsetX2;
    public float OffsetZ2;

    public float Scale3 = 1;
    public float OffsetX3;
    public float OffsetZ3;

    public float exponent;

    //public float Noise(int x, int z)
    //{ 
    //   System.Random SeededRandom = new System.Random(Seed.GetHashCode());
    //    float value = Mathf.PerlinNoise(MapSizeX * (x / Scale) + OffsetX, MapSizeZ * (z / Scale) + OffsetZ) * (float)SeededRandom.NextDouble();
    //    float value2 = Mathf.PerlinNoise(MapSizeX * (x / Scale2) + OffsetX2, MapSizeZ * (z / Scale2) + OffsetZ2) * (float)SeededRandom.NextDouble();
    //     float finalvalue = value + value2;
    //     return finalvalue ;

    //  }

    public float Noise(int x,int z)
    {
        System.Random SeededRandom = new System.Random(Seed.GetHashCode());
        float e = 0;
        float FallX = (float)(x / (Scale * MapSizeX) + OffsetX);
        float FallZ = (float)(z / (Scale * MapSizeZ) + OffsetZ);

        float FallX2 = (float)(x / (Scale2 * MapSizeX) + OffsetX2);
        float FallZ2 = (float)(z / (Scale2 * MapSizeZ) + OffsetZ2);

        float FallX3 = (float)(x / (Scale3 * MapSizeX) + OffsetX3);
        float FallZ3 = (float)(z / (Scale3 * MapSizeZ) + OffsetZ3);

        float value1 = noise.snoise(new float2(FallX, FallZ)) * (float)SeededRandom.NextDouble();
        float value2 = noise.snoise(new float2(FallX2, FallZ2)) * (float)SeededRandom.NextDouble();
        float value3 = noise.snoise(new float2(FallX3, FallZ3)) * (float)SeededRandom.NextDouble();
        e += Mathf.Lerp(1,0,value1);
        e *= Mathf.Lerp(1, 0, value2);
        e *= Mathf.Lerp(1, 0, value3);
        return (e - FallofMap(x,z));
    }

    public float FallofMap(int x, int z)
    {
        float FallX = (float)2 * x / MapSizeX - 1f;
        float FallZ = (float)2 * z / MapSizeZ - 1f;

        float Distance = 1f - (1f - Mathf.Pow(FallX,exponent)) * (1f - Mathf.Pow(FallZ,exponent));
        return Distance;
    }
    // Start is called before the first frame update
    //id Start()
    //
    //  float[,,] Data = IslandData();
    //  for (int x = 0; x < MapSizeX; x++)
    //  {
    //      for (int z = 0; z < MapSizeZ; z++)
    //      {
    //          for (int y = 0; y < MapSizeY; y++)
    //          {
    //              if (Data[x,y,z] == 1)
    //              { 
    //                  GameObject InstanKubus = Instantiate(kubus, new Vector3(x, y, z), Quaternion.identity);
    //              }
    //          }
   //       }
   //   }
  //}

    public float[,,] IslandData()
    {
        float[,,] Data = new float[MapSizeX + 1,MapSizeY + 1,MapSizeZ + 1];
        if (RandomSeed)
        {
            Seed = (Time.realtimeSinceStartup * 2).ToString();
        }

        for (int x = 0; x < MapSizeX + 1; x++)
        {
            for (int z = 0; z < MapSizeZ + 1; z++)
            {
                for (int y = 0; y < MapSizeY + 1; y++)
                {
                    if (y < MapSizeY * Noise(x, z))
                    {
                        Data[x, y, z] = 0;
                    }
                    else
                    {
                        Data[x, y, z] = 1;
                    }
                }
            }
        }
        return Data;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
