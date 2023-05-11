using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBundle
{
    [SerializeField]
    private GameObject _Human;
    public GameObject Human
    {
        get
        {
            return _Human;
        }

        set 
        {
            _Human = value;
        } 
    }
    public PrefabBundle(GameObject _Human)
    {
        this._Human = _Human;
    }
}
