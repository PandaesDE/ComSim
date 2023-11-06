using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager
{
    public enum Status
    {
        WANDERING,
        SLEEPING,
        HUNTING,
        FLEEING,
        THIRSTY,
        DEHYDRATED,
        HUNGRY,
        STARVING,
        LOOKING_FOR_PARTNER,
        GIVING_BIRTH
    }

    public Status status = Status.WANDERING;
}
