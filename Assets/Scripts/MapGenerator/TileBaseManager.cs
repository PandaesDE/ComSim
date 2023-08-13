using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBaseManager : MonoBehaviour
{ 
    [SerializeField] private TileBase tb_water;
    [SerializeField] private TileBase tb_sand;
    [SerializeField] private TileBase tb_grass;
    [SerializeField] private TileBase tb_stone;
    [SerializeField] private TileBase tb_snow;

    public TileBase getTileBase(float sample)
    {
        if (sample < .2f)
        {
            return tb_water;
        }
        if (sample < .23f)
        {
            return tb_sand;
        }
        if (sample < .6f)
        {
            return tb_grass;
        }
        if (sample < .8f)
        {
            return tb_stone;
        }
        if (sample <= 1f)
        {
            return tb_snow;
        }
        return null;
    }

    public bool isWater(TileBase tb)
    {
        return tb.Equals(tb_water);
    }
}


