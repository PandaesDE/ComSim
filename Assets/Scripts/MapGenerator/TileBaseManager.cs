using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBaseManager : MonoBehaviour
{ 
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
        {
            return tileType.WATER;
        }
        if (sample_ground < .23f)
        {
            return tileType.SAND;
        }
        if (sample_ground < .6f)
        {
            if (sample_ground >= .4f && sample_bush < .5f)
            {
                return tileType.BUSH;
            }
            return tileType.GRASS;
        }
        if (sample_ground < .8f)
        {
            return tileType.STONE;
        }

        return tileType.SNOW;
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

    public bool isWater(TileBase tb)
    {
        return tb.Equals(tb_water);
    }
}


