using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Male : IGender
{
    public bool isReadyForMating
    {
        get
        {
            return false;
        }
    }
    
    public bool isMale
    {
        get
        {
            return true;
        }
    }
}
