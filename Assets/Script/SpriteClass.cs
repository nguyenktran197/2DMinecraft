using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Class", menuName = "SpriteClass")] 
public class SpriteClass : ScriptableObject 
{
    public string SpriteName;
    public Sprite[] Sprites;


    public float Frequency;
}
