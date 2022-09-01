using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class BiomesClass
{
    public Color BiomeColor;

    [Header("Noise")]
     public float CaveFrequency;
     public float TerrainFrequency;   
     Texture2D CaveBaseNoiseTexture;
    [Header("MapSetting")]
    public float HightMapplus;
     public float HeighMulti;
     public bool CaveGenerate;
     public int DirtHeight;

    [Header("MapAndOther")]
     public int randomObjChange;
     public int TreeChange;
     public int treeMaxHeigt = 6;
     public int treeMinHeigt = 3;

    [Header("Material")]
     public MaterialClass[] MaterialClasses;
}
