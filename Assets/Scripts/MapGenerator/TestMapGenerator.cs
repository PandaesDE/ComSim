using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    [SerializeField] private int CELLS_HORIZONTAL;
    [SerializeField] private int CELLS_VERTICAL;

    // The origin of the sampled area in the plane.
    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;

    [SerializeField] private int octaves;
    [SerializeField] private float persistence;
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;

    [SerializeField] private float blackToWhiteThreshold = .5f;


    //Texture stuff
    private Texture2D noiseTex;
    private Color[] pix;
    private SpriteRenderer rend;
    private Dictionary<mapLayers, Color> mapColors;

    private void Awake()
    {
        CELLS_HORIZONTAL = 500;
        CELLS_VERTICAL = 500;
        persistence = .5f;
        frequency = 3f;
        octaves = 3;
        amplitude = 1;

        noiseTex = new Texture2D(CELLS_HORIZONTAL, CELLS_VERTICAL);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = Sprite.Create(noiseTex, new Rect(0, 0, CELLS_HORIZONTAL, CELLS_VERTICAL), new Vector2(0.5f, 0.5f));
        initializeMapColors();
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
                float xCoord = xOrg + x / noiseTex.width;
                float yCoord = yOrg + y / noiseTex.height;
                float sample = OctavePerlin(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = getNormalColor(sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
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

    private enum mapLayers
    {
        DARK_WATER,
        BRIGHT_WATER,
        SAND,
        BRIGHT_GRASS,
        DARK_GRASS,
        BRIGHT_STONE,
        DARK_STONE,
        SNOW
    }

    private Color getNormalColor(float sample)
    {
        if (sample < blackToWhiteThreshold)
        {
            return Color.white;
        } else
        {
            return Color.gray;
        }

        if (sample < .1f)
        {
            return mapColors[mapLayers.DARK_WATER];
        }
        if (sample < .2f)
        {
            return mapColors[mapLayers.BRIGHT_WATER];
        }
        if (sample < .23f)
        {
            return mapColors[mapLayers.SAND];
        }
        if (sample < .35f)
        {
            return mapColors[mapLayers.BRIGHT_GRASS];
        }
        if (sample < .6f)
        {
            return mapColors[mapLayers.DARK_GRASS];
        }
        if (sample < .65f)
        {
            return mapColors[mapLayers.BRIGHT_STONE];
        }
        if (sample < .8f)
        {
            return mapColors[mapLayers.DARK_STONE];
        }
        return mapColors[mapLayers.SNOW];
    }

    private void initializeMapColors()
    {
        mapColors = new Dictionary<mapLayers, Color>();
        mapColors.Add(mapLayers.DARK_WATER, new Color32(19, 49, 138, 255));
        mapColors.Add(mapLayers.BRIGHT_WATER, new Color32(135, 170, 232, 255));
        mapColors.Add(mapLayers.SAND, new Color32(247, 246, 195, 255));
        mapColors.Add(mapLayers.BRIGHT_GRASS, new Color32(121, 199, 99, 255));
        mapColors.Add(mapLayers.DARK_GRASS, new Color32(79, 121, 66, 255));
        mapColors.Add(mapLayers.BRIGHT_STONE, new Color32(87, 85, 80, 255));
        mapColors.Add(mapLayers.DARK_STONE, new Color32(163, 163, 163, 255));
        mapColors.Add(mapLayers.SNOW, new Color32(255, 255, 255, 255));

    }
}
