using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MaterialClass

{
    public string name;
    [Range(0f, 1f)]
    public float frequency;
    public int MaxHeightSpawm;
    [Range(0f, 1f)]
    public float size;
    public Texture2D Texture; 

}
