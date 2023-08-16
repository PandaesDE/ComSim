using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Creature
{
    protected override void Awake()
    {
        base.Awake();
        weight = 130;
    }

}
