using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector2 getRandomValueInPlayground()
    {
        float halfW = Gamevariables.playgroundSize.x/2;
        float halfH = Gamevariables.playgroundSize.y/2;
        return new Vector2(Random.Range(-halfW, halfW), Random.Range(-halfH, halfH));
    }
}
