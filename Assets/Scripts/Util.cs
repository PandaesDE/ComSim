using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    public static gender getRandomGender()
    {
        int r = UnityEngine.Random.Range(0, 2);
        if (r < 1) return gender.MALE;
        return gender.FEMALE;
    }

    public static Vector2 getRandomCoordinateInPlayground()
    {
        int halfW = Gamevariables.playgroundSize.x / 2;
        int halfH = Gamevariables.playgroundSize.y / 2;
        return new Vector2(UnityEngine.Random.Range(-halfW, halfW), UnityEngine.Random.Range(-halfH, halfH));
    }

    public static animalType getRandomAnimalType()
    {
        System.Array vals = System.Enum.GetValues(typeof(animalType));
        int i = UnityEngine.Random.Range(0, vals.Length);
        return (animalType)vals.GetValue(i);
    }

    public static bool isDestinationReached(Vector2 position, Vector2 destination, int errorVal = 1)
    {
        return
            position.x > destination.x - errorVal &&
            position.x < destination.x + errorVal &&
            position.y > destination.y - errorVal &&
            position.y < destination.y + errorVal;
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
            typeof(Boar),
            typeof(Lion),
            typeof(Rabbit)
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

    public static class UIHelper
    {
        public static string preventNullOrEmptyInputString(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return "";
            return txt;
        }

        public static string preventNullOrEmptyInputNumber(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return "0";
            return txt;
        }

        public static bool isValidNumvericEntry(string txt)
        {
            if (txt.Length <= 0) return false;
            if (txt.Equals("-")) return false;
            if (txt.Equals(".")) return false;
            if (txt.Equals("-.")) return false;
            return true;
        }
    }

    public static class MapGenerationHelper {

        //https://adrianb.io/2014/08/09/perlinnoise.html
        public static float OctavePerlin(int xOffset, int yOffset, PerlinSettingsObject pso)
        {
            float x = pso.xOrg + (float)xOffset / pso.zoom;
            float y = pso.yOrg + (float)yOffset / pso.zoom;

            float total = 0;
            float maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
            float tempAmp = pso.amplitude;
            float tempFreq = pso.frequency;

            for (int i = 0; i < pso.octaves; i++)
            {
                total += Mathf.PerlinNoise(x * tempFreq, y * tempFreq) * tempAmp;

                maxValue += tempAmp;

                tempAmp *= pso.persistence;
                tempFreq *= 2f;
            }

            return Mathf.Clamp(total / maxValue, 0, 1);
        }
    }

    public static class SeedHelper {
        private static int range = 10000;

        public static Vector2 convertSeedToCoordinates(string seed)
        {
            return new Vector2(  seed.GetHashCode() % range,
                                 seed.GetHashCode() % range);
        }
    }

    
}
