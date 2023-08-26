using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    public static gender getRandomGender()
    {
        int r = Random.Range(0, 2);
        if (r < 1) return gender.MALE;
        return gender.FEMALE;
    }

    public static bool isDestinationReached(Vector2 position, Vector2 destination, int errorVal = 1)
    {
        return
            position.x > destination.x - errorVal &&
            position.x < destination.x + errorVal &&
            position.y > destination.y - errorVal &&
            position.y < destination.y + errorVal;
    }
    public static Vector2 getRandomCoordinateInPlayground()
    {
        int halfW = (int)Gamevariables.playgroundSize.x/2;
        int halfH = (int)Gamevariables.playgroundSize.y/2;
        return new Vector2(Random.Range(-halfW, halfW), Random.Range(-halfH, halfH));
    }

    public static Vector2Int convertVector3ToVector2Int(Vector3 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }
}
