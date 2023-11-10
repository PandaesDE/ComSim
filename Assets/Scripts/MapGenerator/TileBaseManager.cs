/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBaseManager : MonoBehaviour
{ 
    public Tilemap tilemap;

    [SerializeField] private GameObject go_water_deep;
    [SerializeField] private GameObject go_water;
    [SerializeField] private GameObject go_sand;
    [SerializeField] private GameObject go_grass;
    [SerializeField] private GameObject go_bush;
    [SerializeField] private GameObject go_stone;
    [SerializeField] private GameObject go_snow;

    [SerializeField] private TileBase tb_water_deep;
    [SerializeField] private TileBase tb_water;
    [SerializeField] private TileBase tb_sand;
    [SerializeField] private TileBase tb_grass;
    [SerializeField] private TileBase tb_bush;
    [SerializeField] private TileBase tb_stone;
    [SerializeField] private TileBase tb_snow;

    public enum tileType
    {
        WATER_DEEP,
        WATER,
        SAND,
        GRASS,
        BUSH,
        STONE,
        SNOW
    }

    public float sample_ground;
    public float sample_bush;

    public tileType getTileType()
    {
        if (sample_ground < .19f)
            return tileType.WATER_DEEP;
        if (sample_ground < .2f)
            return tileType.WATER;
        if (sample_ground < .23f)
            return tileType.SAND;
        if (sample_ground < .6f)
            if (sample_ground >= .28f  && sample_ground <.75f && sample_bush < .05f)
                return tileType.BUSH;
            else 
                return tileType.GRASS;
        if (sample_ground < .8f)
            return tileType.STONE;

        return tileType.SNOW;
    }

    public Color getColor()
    {
        tileType t = getTileType();
        if (t == tileType.WATER_DEEP) return go_water_deep.GetComponent<SpriteRenderer>().color;
        if (t == tileType.WATER) return go_water.GetComponent<SpriteRenderer>().color;
        if (t == tileType.SAND) return go_sand.GetComponent<SpriteRenderer>().color;
        if (t == tileType.BUSH) return go_bush.GetComponent<SpriteRenderer>().color;
        if (t == tileType.GRASS) return go_grass.GetComponent<SpriteRenderer>().color;
        if (t == tileType.STONE) return go_stone.GetComponent<SpriteRenderer>().color;
        if (t == tileType.SNOW) return go_snow.GetComponent<SpriteRenderer>().color;
        return Color.magenta;
    }

    public TileBase getTileBase()
    {
        return getTileBase(getTileType());
    }

    public TileBase getTileBase(tileType tt)
    {
        if (tt == tileType.WATER_DEEP) return tb_water_deep;
        if (tt == tileType.WATER) return tb_water;
        if (tt == tileType.SAND) return tb_sand;
        if (tt == tileType.BUSH) return tb_bush;
        if (tt == tileType.GRASS) return tb_grass;
        if (tt == tileType.STONE) return tb_stone;
        if (tt == tileType.SNOW) return tb_snow;
        return null;
    }

    #region Tilemap
    public void ClearAllTiles()
    {
        tilemap.ClearAllTiles();
    }

    public Tilemap GetTilemap()
    {
        return tilemap;
    }

    public void SetTile(Vector2 coord)
    {
        SetTile(new Vector2Int((int)coord.x, (int)coord.y));
    }

    public void SetTile(Vector2Int coord)
    {
        SetTile(coord, getTileBase());
    }

    public void SetTile(Vector2Int coord, TileBase tb)
    {
        tilemap.SetTile(new Vector3Int(coord.x, coord.y, 0), tb);
    }
    #endregion

    public bool isWater(TileBase tb)
    {
        //probably doesn't work
        return tb_water.Equals(tb);
    }

    public bool isWater(GameObject g)
    {
        return g.tag == "Water";
    }

    public bool isWater(Vector2 coord)
    {
        return tb_water.Equals(tilemap.GetTile(new Vector3Int((int)coord.x, (int)coord.y, 0)));
    }

    public bool isBerry(TileBase tb)
    {
        return tb_bush.Equals(tb);
    }

    public bool isBerry(Vector2 coord)
    {
        return tb_bush.Equals(tilemap.GetTile(new Vector3Int((int)coord.x, (int)coord.y, 0)));
    }
}


