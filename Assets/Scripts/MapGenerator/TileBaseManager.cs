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

    public float sample_ground;
    public float sample_bush;

    public TileBase getTileBase()
    {
        if (sample_ground < .2f)
        {
            return tb_water;
        }
        if (sample_ground < .23f)
        {
            return tb_sand;
        }
        if (sample_ground < .6f)
        {
            if (sample_ground >= .4f && sample_bush < .5f)
            {
                return tb_bush;
            }
            return tb_grass;
        }
        if (sample_ground < .8f)
        {
            return tb_stone;
        }
        if (sample_ground <= 1f)
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


