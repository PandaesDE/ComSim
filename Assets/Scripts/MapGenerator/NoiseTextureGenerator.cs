/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    public PerlinSettingsObject pso_ground;
    public PerlinSettingsObject pso_bush;

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
                float xOffset = (float)x;
                float yOffset = (float)y;
                tbm.sample_bush = Util.MapGeneration.OctavePerlin(xOffset, yOffset, pso_bush);
                tbm.sample_ground = Util.MapGeneration.OctavePerlin(xOffset, yOffset, pso_ground);

                pix[(int)y * width + (int)x] = tbm.getColor();
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

}
