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
        int halfW = Gamevariables.playgroundSize.x/2;
        int halfH = Gamevariables.playgroundSize.y/2;
        return new Vector2(Random.Range(-halfW, halfW), Random.Range(-halfH, halfH));
    }

    public static Vector2Int convertVector3ToVector2Int(Vector3 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }

    public static Vector2 convertVector3ToVector2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static List<System.Type> getFoodList(foodType ft, System.Type self)
    {
        List<System.Type> herbivore = new()
        {
            
        };

        List<System.Type> carnivore = new()
        {
            typeof(Human),
            typeof(Animal)
        };

        List<System.Type> omnivore = new();
        omnivore.AddRange(herbivore);
        omnivore.AddRange(carnivore);

        if (ft == foodType.HERBIVORE)
            return herbivore;

        //exclude self from foodlist
        if (carnivore.Contains(self))
            carnivore.Remove(self);

        if (ft == foodType.CARNIVORE)
            return carnivore;

        if (ft == foodType.OMNIVORE)
            return omnivore;

        Debug.LogError("Something went wrong in Util.geetFoodList");
        return null;
    }

    public static int roundFloatUpPositiveDownNegative(float val)
    {
        if (val < 0)
        {
            val -= .5f;
            return (int)val;
        } else
        {
            val += .5f;
            return (int)val;
        }
    }
}
