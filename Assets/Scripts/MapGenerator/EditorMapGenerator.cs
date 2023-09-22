/*
 * 
 * Comment: This script is only for the EditorMapGeneration Scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject bush;
    [SerializeField] private GameObject stone;
    [SerializeField] private GameObject snow;

    // Width and height of the texture in pixels.
    [SerializeField] private int CELLS_HORIZONTAL;
    [SerializeField] private int CELLS_VERTICAL;

    // The origin of the sampled area in the plane.
    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;
    [SerializeField] private float zoom;

    public PerlinSettingsObject pso_ground;
    public PerlinSettingsObject pso_bush;

    [SerializeField] private float blackToWhiteThreshold = .5f;


    //Texture stuff
    private Texture2D noiseTex;
    private Color[] pix;
    private SpriteRenderer rend;
    private TileBaseManager tbm;

    private void Awake()
    {
        CELLS_HORIZONTAL = Gamevariables.playgroundSize.x;
        CELLS_VERTICAL = Gamevariables.playgroundSize.y;
        zoom = 100;

        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        pso_ground = settings.Pso_Ground;
        pso_bush = settings.Pso_Bush;

        noiseTex = new Texture2D(CELLS_HORIZONTAL, CELLS_VERTICAL);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = Sprite.Create(noiseTex, new Rect(0, 0, CELLS_HORIZONTAL, CELLS_VERTICAL), new Vector2(0.5f, 0.5f));
        tbm = GetComponent<TileBaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CalcNoise();
    }

    private void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / zoom;
                float yCoord = yOrg + y / zoom;

                tbm.sample_bush = OctavePerlin(xCoord, yCoord, pso_bush);
                tbm.sample_ground = OctavePerlin(xCoord, yCoord, pso_ground);

                pix[(int)y * noiseTex.width + (int)x] = getColor();
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    //https://adrianb.io/2014/08/09/perlinnoise.html
    float OctavePerlin(float x, float y, PerlinSettingsObject pso)
    {
        float total = 0;
        float maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
        float tempAmp = pso.amplitude;
        float tempFreq = pso.frequency;

        for (int i = 0; i < pso.octaves; i++)
        {
            total += Mathf.PerlinNoise(x * tempFreq, y * tempFreq) * tempAmp;

            maxValue += tempAmp;

            tempAmp *= pso.persistence;
            tempFreq *= 2f;
        }

        return Mathf.Clamp(total / maxValue, 0, 1);
    }

    private Color getColor()
    {
        TileBaseManager.tileType t = tbm.getTileType();
        if (t == TileBaseManager.tileType.WATER) return water.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.SAND) return sand.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.BUSH) return bush.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.GRASS) return grass.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.STONE) return stone.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.SNOW) return snow.GetComponent<SpriteRenderer>().color;
        return Color.magenta;
    }
}
