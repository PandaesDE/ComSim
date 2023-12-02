/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Initializes the worlds tilemap
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - old: (https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html)
 *      - https://blog.unity.com/engine-platform/procedural-patterns-you-can-use-with-tilemaps-part-1
 */


using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    private int _cellsHorizontal;
    private int _cellsVertical;

    //Tilemap
    public TileBaseManager tbm;

    private void Awake()
    {
        _cellsHorizontal = Gamevariables.PLAYGROUND_SIZE.x;
        _cellsVertical = Gamevariables.PLAYGROUND_SIZE.y;

        tbm = GetComponent<TileBaseManager>();
        RenderTileMap();
    }

    private void RenderTileMap()
    {
        //Clear the map (ensures we dont overlap)
        tbm.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < _cellsHorizontal; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < _cellsVertical; y++)
            {
                tbm.groundSample = Util.MapGeneration.OctavePerlin(x, y, Gamevariables.PSO_Ground);
                tbm.bushSample = Util.MapGeneration.OctavePerlin(x, y, Gamevariables.PSO_Bush);

                tbm.SetTile(new Vector2Int(x - _cellsHorizontal/2, y - _cellsVertical / 2));
            }
        }
    }
}
