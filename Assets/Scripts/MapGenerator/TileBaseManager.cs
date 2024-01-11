/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Stores data about Tiles and it's GameObjects
 *  
 *  References:
 *      Scene:
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBaseManager : MonoBehaviour
{ 
    public Tilemap tilemap;

    [SerializeField] private GameObject _go_water_deep;
    [SerializeField] private GameObject _go_water;
    [SerializeField] private GameObject _go_sand;
    [SerializeField] private GameObject _go_grass;
    [SerializeField] private GameObject _go_bush;
    [SerializeField] private GameObject _go_stone;
    [SerializeField] private GameObject _go_snow;

    [SerializeField] private TileBase _tb_water_deep;
    [SerializeField] private TileBase _tb_water;
    [SerializeField] private TileBase _tb_sand;
    [SerializeField] private TileBase _tb_grass;
    [SerializeField] private TileBase _tb_bush;
    [SerializeField] private TileBase _tb_stone;
    [SerializeField] private TileBase _tb_snow;

    public enum TileType
    {
        water_deep,
        water,
        sand,
        grass,
        bush,
        stone,
        snow
    }

    public float groundSample;
    public float bushSample;

    public TileType GetTileType()
    {
        if (groundSample < .19f)
            return TileType.water_deep;
        if (groundSample < .2f)
            return TileType.water;
        if (groundSample < .23f)
            return TileType.sand;
        if (groundSample < .6f)
            if (groundSample >= .28f  && groundSample <.75f && bushSample < .05f)
                return TileType.bush;
            else 
                return TileType.grass;
        if (groundSample < .8f)
            return TileType.stone;

        return TileType.snow;
    }

    public Color GetColor(string color = "")
    {
        if (color == "grey")
        {
            return new Color(groundSample, groundSample, groundSample);
        }

        Color c = Color.white; //error

        TileType t = GetTileType();
        if (t == TileType.water_deep) c = _go_water_deep.GetComponent<SpriteRenderer>().color;
        else if (t == TileType.water) c = _go_water.GetComponent<SpriteRenderer>().color;
        else if (t == TileType.sand) c = _go_sand.GetComponent<SpriteRenderer>().color;
        else if (t == TileType.bush) c = _go_bush.GetComponent<SpriteRenderer>().color;
        else if (t == TileType.grass) c = _go_grass.GetComponent<SpriteRenderer>().color;
        else if (t == TileType.stone) c = _go_stone.GetComponent<SpriteRenderer>().color;
        else if (t == TileType.snow) c = _go_snow.GetComponent<SpriteRenderer>().color;

        if (color == "grey_pixel")
        {
            c = new Color(c.grayscale, c.grayscale, c.grayscale);
        }

        return c;
    }

    public Color GetColor()
    {

        return Color.magenta;
    }

    public TileBase GetTileBase()
    {
        return GetTileBase(GetTileType());
    }

    public TileBase GetTileBase(TileType tt)
    {
        if (tt == TileType.water_deep) return _tb_water_deep;
        if (tt == TileType.water) return _tb_water;
        if (tt == TileType.sand) return _tb_sand;
        if (tt == TileType.bush) return _tb_bush;
        if (tt == TileType.grass) return _tb_grass;
        if (tt == TileType.stone) return _tb_stone;
        if (tt == TileType.snow) return _tb_snow;
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
        SetTile(coord, GetTileBase());
    }

    public void SetTile(Vector2Int coord, TileBase tb)
    {
        tilemap.SetTile(new Vector3Int(coord.x, coord.y, 0), tb);
    }
    #endregion

    //public bool IsWater(TileBase tb)
    //{
    //    //probably doesn't work
    //    return _tb_water.Equals(tb);
    //}

    public bool IsWater(GameObject g)
    {
        return g.CompareTag("Water");
    }

    //public bool IsWater(Vector2 coord)
    //{
    //    return _tb_water.Equals(tilemap.GetTile(new Vector3Int((int)coord.x, (int)coord.y, 0)));
    //}

    //public bool IsBerry(TileBase tb)
    //{
    //    return _tb_bush.Equals(tb);
    //}

    //public bool IsBerry(Vector2 coord)
    //{
    //    return _tb_bush.Equals(tilemap.GetTile(new Vector3Int((int)coord.x, (int)coord.y, 0)));
    //}
}


