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

    public PerlinSettingsObject pso_ground;
    public PerlinSettingsObject pso_bush;

    //[SerializeField] private float blackToWhiteThreshold = .5f;


    //Texture stuff
    private Texture2D noiseTex;
    private Color[] pix;
    private SpriteRenderer rend;
    private TileBaseManager tbm;

    private void Awake()
    {
        CELLS_HORIZONTAL = Gamevariables.playgroundSize.x;
        CELLS_VERTICAL = Gamevariables.playgroundSize.y;

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
        for (int x = 0; x < CELLS_HORIZONTAL; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < CELLS_VERTICAL; y++)
            {
                tbm.sample_bush = Util.MapGenerationHelper.OctavePerlin(x, y, pso_bush);
                tbm.sample_ground = Util.MapGenerationHelper.OctavePerlin(x, y, pso_ground);

                pix[(int)y * noiseTex.width + (int)x] = getColor();
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
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
