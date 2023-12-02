/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - this class creates special maps for different scenarios
 *  
 *  References:
 *      Scene:
 *          - test simulation scene(s)
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
                    tbm.SetTile(new Vector2Int(x - width / 2, y - height / 2), tbm.GetTileBase(TileBaseManager.TileType.grass));
                else
                    tbm.SetTile(new Vector2Int(x - width / 2, y - height / 2), tbm.GetTileBase(TileBaseManager.TileType.sand));
            }
        }
    }
}
