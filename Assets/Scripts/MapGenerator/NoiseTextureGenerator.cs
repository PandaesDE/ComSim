/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Calculate perlin noise texture
 *  
 *  References:
 *      Scene:
 *          - EditorMapGenerator
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    public PerlinSettingsObject PSO_Ground;
    public PerlinSettingsObject PSO_Bush;

    //[SerializeField] private float blackToWhiteThreshold = .5f;


    //Texture stuff
    private TileBaseManager tbm;

    private void Awake()
    {
        GameSettingsObject settings = ConfigManager.ReadSettings();
        PSO_Ground = settings.PSO_Ground;
        PSO_Bush = settings.PSO_Bush;

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
                float xOffset = (float)x;
                float yOffset = (float)y;
                tbm.bushSample = Util.MapGeneration.OctavePerlin(xOffset, yOffset, PSO_Bush);
                tbm.groundSample = Util.MapGeneration.OctavePerlin(xOffset, yOffset, PSO_Ground);

                pix[(int)y * width + (int)x] = tbm.GetColor("");
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

}
