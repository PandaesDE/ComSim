using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGender
{
    public bool isReadyForMating { get; }
    public bool isMale { get; }
}
