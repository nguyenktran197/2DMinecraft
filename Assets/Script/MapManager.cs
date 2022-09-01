using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] public string Biomesname;
    [SerializeField] public BiomesClass[] BiomesClass;
    [Header("BaseMaterial Setting")]
    [SerializeField] public float Seed;
    [SerializeField] public SpriteAtlas SpriteAtlas;
    [SerializeField] public BiomesClass NormalLand;
    [SerializeField] public BiomesClass DesertLand;
    [SerializeField] public BiomesClass IceLand;



    [Header("MapSize Setting")]
    // Ph?n này setting size c?a Map
    [SerializeField] public int ChunkSize;
    [SerializeField] public int MapSize;   
    [SerializeField] public float HightMapplus;
    [SerializeField] public float HeighMulti;
    [SerializeField] public bool CaveGenerate;
    [SerializeField] public int DirtHeight; // s? l?p ??t t?i ?a

    [Header("Biomes")]
    [SerializeField] public float BiomesFrequency;
    [SerializeField] public Texture2D BiomesMap;
    [SerializeField] public Gradient BiomesColor;





    [Header("Tree Setting")]
    // Ph?n này setting cây c? 
    [SerializeField] public int randomObjChange;
    [SerializeField] public int TreeChange;
    [SerializeField] public int treeMaxHeigt = 6;
    [SerializeField] public int treeMinHeigt = 3;

    [Header("NoiseTextureSetting")]
    // Ph?n này liên quan chút ??n shader a c?ng m?i tìm hi?u c? t?m hi?u thôi nhé
    [SerializeField] public float FlatFrequency;
    [SerializeField] public float CaveFrequency;
    [SerializeField] public float TerrainFrequency;

    [SerializeField] Texture2D CaveBaseNoiseTexture;





    [Header("MoreMaterial Setting")]

    [SerializeField] public MaterialClass[] MaterialClasses;


    //[SerializeField] public float GoldFrequency, GoldSize;   
    //[SerializeField] public float DiamondFrequency, DiamondSize;
    //[SerializeField] public Texture2D GoldTextureNois;
    //[SerializeField] public Texture2D DiamondTextureNoise;



     private GameObject[] ListChunk;
     private List<Vector2>WorldMaterial = new List<Vector2> ();


    private void OnValidate()
    {
        //if (CaveBaseNoiseTexture == null)
        //{
        //    CaveBaseNoiseTexture = new Texture2D(MapSize, MapSize);
        //    GoldTextureNois = new Texture2D(MapSize, MapSize);
        //    DiamondTextureNoise = new Texture2D(MapSize, MapSize);

        //}
        //GenerateNoiseTexture(Seed,CaveFrequency, CaveBaseNoiseTexture, FlatFrequency);
        //GenerateNoiseTexture(Seed, GoldFrequency, GoldTextureNois, GoldSize);
        //GenerateNoiseTexture(Seed, DiamondFrequency, DiamondTextureNoise, DiamondSize);
    }

    void Start()
    {
        Seed = Random.Range(-10000, 10000);
       
        GenerateTextureMap();
      
        ChunkCreator();
        GenerateMapTerrain();
     
    }

    public void GenerateBiomesZone()
    {
        BiomesMap = new Texture2D(MapSize, MapSize);
        float x;
        for (int i = 0; i < BiomesMap.width; i++)
        {
            for (int j = 0; j < BiomesMap.height; j++)
            {
                x = Mathf.PerlinNoise(((i + Seed) * BiomesFrequency), ((j + Seed) * BiomesFrequency));
                Color Col = BiomesColor.Evaluate(x);

                BiomesMap.SetPixel(i, j, Col);

            }
        }
        BiomesMap.Apply();
    }




    private void GenerateTextureMap() 
    {
        BiomesMap = new Texture2D(MapSize, MapSize);
        GenerateBiomesZone();


        CaveBaseNoiseTexture = new Texture2D(MapSize, MapSize);
        MaterialClasses[0].Texture = new Texture2D(MapSize, MapSize);
        MaterialClasses[1].Texture = new Texture2D(MapSize, MapSize);


        GenerateNoiseTexture(Seed, CaveFrequency, CaveBaseNoiseTexture, FlatFrequency);
        GenerateNoiseTexture(Seed, MaterialClasses[0].frequency, MaterialClasses[0].Texture, MaterialClasses[0].size);
        GenerateNoiseTexture(Seed, MaterialClasses[1].frequency, MaterialClasses[1].Texture, MaterialClasses[1].size);
    }

    private void GenerateMapTerrain()
    {
        for (int i = 0; i < MapSize; i++)
        {
            float heighmap = Mathf.PerlinNoise((i + Seed) * TerrainFrequency, Seed * TerrainFrequency) * HeighMulti + HightMapplus;
            for (int j = 0; j < heighmap; j++)
            {
                Sprite[] BaseMaterial;
                if (j < heighmap - DirtHeight) // n?u duy?t ??n j mà bé h?n chi?u cao c?a map tr? ?i v?i l?p ??t t?i thi?u thì ra ?á
                {
                    if (MaterialClasses[0].Texture.GetPixel(i, j).r > 0.5f && heighmap - j > MaterialClasses[0].MaxHeightSpawm) 
                    {
                        BaseMaterial = SpriteAtlas.Gold.Sprites;
                    }
                    else if (MaterialClasses[1].Texture.GetPixel(i,j).r > 0.5f && heighmap - j > MaterialClasses[1].MaxHeightSpawm)
                    {
                        BaseMaterial = SpriteAtlas.Diamond.Sprites;
                    }
                    else
                    {
                        BaseMaterial = SpriteAtlas.Stone.Sprites;
                    }
                   
                }
                else if(j < heighmap -1)// n?u duy?t ??n j mà bé h?n 1 thì là d??i l?p cao nh?t 1 
                {
                    BaseMaterial = SpriteAtlas.Dirt.Sprites;                  
                }
                else   // thì ?ây là l?p j cao nh?t t?o ?c sprite
                {
                    BaseMaterial = SpriteAtlas.Grass.Sprites;                 
                }

                // n?u có hang ??ng thì dùng noise
                if (CaveGenerate)
                {
                    if (CaveBaseNoiseTexture.GetPixel(i, j).r > FlatFrequency)
                    {
                        PlaceBox2D(BaseMaterial, i, j);
                    }
                }
                else
                {
                    PlaceBox2D(BaseMaterial, i, j); 
                }

                // N?u là l?p trên cùng
                if (j >= heighmap -1)
                {
                    int treeFrequency = Random.Range(0, TreeChange);
                    if (treeFrequency < 2)
                    {
                        if (WorldMaterial.Contains(new Vector2(i, j)))// tìm trong list xem ? v? trí i,j này có sprite ?c t?o k
                        {
                            GenerateTree(i, j + 1);
                        } 

                    }
                    else
                    {
                        int ObjRndChange = Random.Range(0, randomObjChange);
                        if (ObjRndChange < 3)
                        {
                            if (WorldMaterial.Contains(new Vector2(i, j)))// tìm trong list xem ? v? trí i,j này có sprite ?c t?o k
                            {
                                if (SpriteAtlas.RandomObj != null)
                                {
                                    PlaceBox2D(SpriteAtlas.RandomObj.Sprites, i, j + 1);
                                }
                               
                            }
                        }
                                              
                    }
                }
            }
        }

    }

    void GenerateTree(int i, int j)
    {
        // d?ng cây theo i và j
        int treeHeight = Random.Range(treeMinHeigt, treeMaxHeigt); 
        for (int k = 0; k < treeHeight; k++)
        {
            PlaceBox2D(SpriteAtlas.Log.Sprites, i, j +k);
        }
        // d?ng lá cây theo i và j
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i, j + treeHeight);
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i, j + treeHeight + 1);
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i, j + treeHeight + 2);

        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i + 1, j + treeHeight);
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i+1, j + treeHeight + 1);
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i + 1, j + treeHeight + 2);

        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i - 1, j + treeHeight);
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i - 1, j + treeHeight + 1);
        PlaceBox2D(SpriteAtlas.Leaf.Sprites, i - 1, j + treeHeight + 2);


    }
    void PlaceBox2D(Sprite[] BaseMaterial, int i, int j)
    {
        if (!WorldMaterial.Contains(new Vector2Int(i,j)))
        {
            GameObject newSprite = new GameObject();

            int chunkCoord = Mathf.RoundToInt(Mathf.Round(i / ChunkSize) * ChunkSize);
            chunkCoord /= ChunkSize;

            newSprite.transform.parent = ListChunk[chunkCoord].transform;
            int SpriteRndIndex = Random.Range(0, BaseMaterial.Length);

            newSprite.AddComponent<SpriteRenderer>();
            newSprite.GetComponent<SpriteRenderer>().sprite = BaseMaterial[SpriteRndIndex];
            newSprite.name = BaseMaterial[0].name;
            newSprite.transform.position = new Vector2(i, j);
            WorldMaterial.Add(newSprite.transform.position);
        }
        // Cái này Instance Sprite thooi ??n gi?n
       
    }
    private void GenerateNoiseTexture(float seed,float NoiseFrequency , Texture2D NoiseTexture, float sizeMax)
    {
        // chỉ là mix shader ra texture nhiễu random thôi từ từ học
        for (int i = 0; i < NoiseTexture.width; i++)
        {
            for (int j = 0; j < NoiseTexture.height; j++)
            {
                float x = Mathf.PerlinNoise(((i+ seed) * NoiseFrequency), ((j+ seed) * NoiseFrequency));
                NoiseTexture.SetPixel(i, j,new Color(x,x,x));
                if (x > sizeMax)
                {
                    NoiseTexture.SetPixel(i, j, Color.white);
                }
                else
                {
                    NoiseTexture.SetPixel(i, j, Color.black);
                }
            }
        }
        NoiseTexture.Apply();
    }

  

    public void ChunkCreator() // chia map than tung phan nho de de optimize thoi
    {
        int chunkcount = MapSize / ChunkSize;
        ListChunk = new GameObject[chunkcount];    
        for (int i = 0; i < chunkcount; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            ListChunk[i] = newChunk;
        }
    }


    void Update()
    {
        
    }
}
