using System.Collections;

using UnityEngine;
[CreateAssetMenu(fileName = "SpriteAtlas", menuName = "Sprite Atlas")]
public class SpriteAtlas : ScriptableObject
{
    [Header("BaseMaterial")]
    [SerializeField] public SpriteClass Dirt;
    [SerializeField] public SpriteClass Stone;
    [SerializeField] public SpriteClass Grass;
    [SerializeField] public SpriteClass Snow;
    [SerializeField] public SpriteClass Sand;
    [SerializeField] public SpriteClass Log;
    [SerializeField] public SpriteClass Leaf;
    [SerializeField] public SpriteClass RandomObj;
    [Header("UpradeMaterial")]
    [SerializeField] public SpriteClass Gold;
    [SerializeField] public SpriteClass Diamond;
}
