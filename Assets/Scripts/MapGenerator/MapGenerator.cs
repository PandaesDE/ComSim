// base: https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class MapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    public int octaves;
    public float persistence;
    public float frequency;
    public float amplitude;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.

    private Texture2D noiseTex;
    private Color[] pix;
    private SpriteRenderer rend;


    private MapStyle mapStyle;


    void Start()
    {
        persistence = -.5f;
        frequency = .6f;
        octaves = 8;
        amplitude = 1;
        rend = GetComponent<SpriteRenderer>();
        mapStyle = new MapStyle();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.sprite = Sprite.Create(noiseTex, new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));

    }

    

    void CalcNoise()
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
                pix[(int)y * noiseTex.width + (int)x] = mapStyle.getColor(sample, MapStyle.mapStyles.NORMAL);
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

        return total / maxValue;
    }



    void Update()
    {
        CalcNoise();
    }
}
