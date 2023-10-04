using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;

public class IslandGenEdit : MonoBehaviour
{
    public int TextureSize;

    private Color[] col;
    private Texture2D tex;
    public Gradient colGradient;

    public string Seed;
    public bool RandomSeed = true;
   

    private void Start()
    {
        tex = new Texture2D(TextureSize, TextureSize);
        col = new Color[tex.height * tex.width];
        Renderer renderer = GetComponent<MeshRenderer>();
        renderer.sharedMaterial.mainTexture = tex;
        if (RandomSeed)
        {
            Seed = Time.realtimeSinceStartup.ToString();
        }
        IslandGen.Seed = Seed;


    }
    private void Update()
    {
        for (int x = 0, i = 0; x < IslandGen.MapSizeX; x++)
        {
            for (int z = 0; z < IslandGen.MapSizeZ; z++, i++)
            {
                float a = IslandGen.Noise(x, z);
                col[i] = colGradient.Evaluate(a);
            }
        }
        tex.SetPixels(col);
        tex.Apply();
        tex.wrapMode = TextureWrapMode.Clamp;
    }

}
