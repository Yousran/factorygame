using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tree",menuName ="Trees")]
public class Trees : ScriptableObject
{
    public GameObject[] PrefabPohon;
    public float HealthPohon;
    [Range(0f,1f)]
    public float SpawnRate;
    [Range(0f, 1f)]
    public float MaxVerticalSpawnRate;
    [Range(0f, 1f)]
    public float MinVerticalSpawnRate;

    [Header("Woods")]
    public GameObject[] PrefabKayu;
    public int MinWoodSpawn;
    public int MaxWoodSpawn;
    public int HargaKayu;

}
