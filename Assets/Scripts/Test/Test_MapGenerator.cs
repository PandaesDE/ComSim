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

public class Test_MapGenerator : MonoBehaviour
{
    //Tilemap
    public TileBaseManager tbm;

    private void Awake()
    {
        tbm = GetComponent<TileBaseManager>();
    }


    public void RenderNormalTestMap(int width, int height)
    {
        tbm.ClearAllTiles();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if ((x + y) % 2 == 0)
                    tbm.SetTile(new Vector2Int(x - width / 2, y - height / 2), tbm.getTileBase(TileBaseManager.tileType.GRASS));
                else
                    tbm.SetTile(new Vector2Int(x - width / 2, y - height / 2), tbm.getTileBase(TileBaseManager.tileType.SAND));
            }
        }
    }
}
