using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject bush;
    [SerializeField] private GameObject stone;
    [SerializeField] private GameObject snow;



    public PerlinSettingsObject pso_ground;
    public PerlinSettingsObject pso_bush;

    //animation Offset, used to move around the map in the background
    private Vector2 offset = new Vector2(0, 0);

    //[SerializeField] private float blackToWhiteThreshold = .5f;


    //Texture stuff
    private TileBaseManager tbm;

    private void Awake()
    {
        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        pso_ground = settings.Pso_Ground;
        pso_bush = settings.Pso_Bush;

        tbm = GetComponent<TileBaseManager>();
    }

    public void CalcNoise(Texture2D noiseTex, int width, int height)
    {
        Color[] pix = new Color[width * height];

        for (int x = 0; x < width; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < height; y++)
            {
                float xOffset = (float)x + offset.x;
                float yOffset = (float)y + offset.y;
                tbm.sample_bush = Util.MapGenerationHelper.OctavePerlin(xOffset, yOffset, pso_bush);
                tbm.sample_ground = Util.MapGenerationHelper.OctavePerlin(xOffset, yOffset, pso_ground);

                pix[(int)y * width + (int)x] = getColor();
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    public void moveBy(Vector2 offset)
    {
        this.offset += offset;
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
