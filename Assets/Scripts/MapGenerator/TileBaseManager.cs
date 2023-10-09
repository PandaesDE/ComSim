using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBaseManager : MonoBehaviour
{ 
    public Tilemap tilemap;

    [SerializeField] private GameObject go_water;
    [SerializeField] private GameObject go_sand;
    [SerializeField] private GameObject go_grass;
    [SerializeField] private GameObject go_bush;
    [SerializeField] private GameObject go_stone;
    [SerializeField] private GameObject go_snow;

    [SerializeField] private TileBase tb_water;
    [SerializeField] private TileBase tb_sand;
    [SerializeField] private TileBase tb_grass;
    [SerializeField] private TileBase tb_bush;
    [SerializeField] private TileBase tb_stone;
    [SerializeField] private TileBase tb_snow;

    public enum tileType
    {
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
        TileBaseManager.tileType t = getTileType();
        if (t == TileBaseManager.tileType.WATER) return go_water.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.SAND) return go_sand.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.BUSH) return go_bush.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.GRASS) return go_grass.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.STONE) return go_stone.GetComponent<SpriteRenderer>().color;
        if (t == TileBaseManager.tileType.SNOW) return go_snow.GetComponent<SpriteRenderer>().color;
        return Color.magenta;
    }

    public TileBase getTileBase()
    {
        tileType t = getTileType();
        if (t == tileType.WATER) return tb_water;
        if (t == tileType.SAND) return tb_sand;
        if (t == tileType.BUSH) return tb_bush;
        if (t == tileType.GRASS) return tb_grass;
        if (t == tileType.STONE) return tb_stone;
        if (t == tileType.SNOW) return tb_snow;
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
        tilemap.SetTile(new Vector3Int(coord.x, coord.y, 0), getTileBase());
    }
    #endregion

    public bool isWater(TileBase tb)
    {
        return tb_water.Equals(tb);
    }

    public bool isWater(GameObject g)
    {
        return go_water.Equals(g);
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


