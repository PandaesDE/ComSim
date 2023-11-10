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

// base: https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html

using UnityEngine;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class MapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    private int CELLS_HORIZONTAL;
    private int CELLS_VERTICAL;

    //Tilemap
    public TileBaseManager tbm;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.


    private void Awake()
    {
        CELLS_HORIZONTAL = Gamevariables.playgroundSize.x;
        CELLS_VERTICAL = Gamevariables.playgroundSize.y;

        tbm = GetComponent<TileBaseManager>();
        RenderTileMap();
    }

    //https://blog.unity.com/engine-platform/procedural-patterns-you-can-use-with-tilemaps-part-1
    private void RenderTileMap()
    {
        //Clear the map (ensures we dont overlap)
        tbm.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < CELLS_HORIZONTAL; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < CELLS_VERTICAL; y++)
            {
                tbm.sample_ground = Util.MapGeneration.OctavePerlin(x, y, Gamevariables.PSO_GROUND);
                tbm.sample_bush = Util.MapGeneration.OctavePerlin(x, y, Gamevariables.PSO_BUSH);

                tbm.SetTile(new Vector2Int(x - CELLS_HORIZONTAL/2, y - CELLS_VERTICAL / 2));
            }
        }
    }
}
