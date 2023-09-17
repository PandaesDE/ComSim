// base: https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class MapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    [SerializeField] private int CELLS_HORIZONTAL;
    [SerializeField] private int CELLS_VERTICAL;

    //Tilemap
    [SerializeField] public Tilemap tilemap;
    [SerializeField] public TileBaseManager tbm;

    private float[,] map;



    // The origin of the sampled area in the plane.
    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;
    [SerializeField] private float zoom;

    [SerializeField] private int octaves;
    [SerializeField] private float persistence;
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.


    private void Awake()
    {
        CELLS_HORIZONTAL = Gamevariables.playgroundSize.x;
        CELLS_VERTICAL = Gamevariables.playgroundSize.y;
        zoom = 100; 
        persistence = -.5f;
        frequency = .6f;
        octaves = 8;
        amplitude = 1;

        tbm = GetComponent<TileBaseManager>();
        map = new float[CELLS_HORIZONTAL, CELLS_VERTICAL];

    }

    void Start()
    {
        convertSeed();
        RenderTileMap();
    }


    #region Tilemap approach
    //https://blog.unity.com/engine-platform/procedural-patterns-you-can-use-with-tilemaps-part-1
    private void RenderTileMap()
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < CELLS_HORIZONTAL; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < CELLS_VERTICAL; y++)
            {
                float xCoord = xOrg + (float)x / zoom;
                float yCoord = yOrg + (float)y / zoom;
                float sample = OctavePerlin(xCoord, yCoord);

                tilemap.SetTile(new Vector3Int(x - CELLS_HORIZONTAL/2, y - CELLS_VERTICAL / 2, 0), tbm.getTileBase(sample));
                
                map[x, y] = sample;
            }
        }
    }
    #endregion

    //CHATGPT
    public void convertSeed()
    {
        int range = 10000;
        xOrg = Mathf.Abs(Gamevariables.SEED.GetHashCode() % range);
        yOrg = Mathf.Abs(Gamevariables.SEED.GetHashCode() % range);
    }


    //https://adrianb.io/2014/08/09/perlinnoise.html
    float OctavePerlin(float x, float y)
    {
        float total = 0;
        float maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
        float tempAmp = amplitude;
        float tempFreq = frequency;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * tempFreq, y * tempFreq) * tempAmp;

            maxValue += tempAmp;

            tempAmp *= persistence;
            tempFreq *= 2f;
        }

        return Mathf.Clamp(total / maxValue, 0, 1);
    }
}
