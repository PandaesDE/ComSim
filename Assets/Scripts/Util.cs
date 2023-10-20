/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine;

public static class Util
{
    public static bool inRange(Vector2 position, Vector2 destination, float range = 1)
    {
        return
            position.x > destination.x - range &&
            position.x < destination.x + range &&
            position.y > destination.y - range &&
            position.y < destination.y + range;
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

    public static class Random
    {
        public static gender Gender()
        {
            int r = UnityEngine.Random.Range(0, 2);
            if (r < 1) return gender.MALE;
            return gender.FEMALE;
        }

        public static float Float(float max)
        {
            return Float(0, max);
        }

        public static float Float(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static int Int(int max)
        {
            return Int(0, max);
        }

        public static int Int(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }


        public static Vector2 CoordinateInPlayground()
        {
            int halfW = Gamevariables.playgroundSize.x / 2;
            int halfH = Gamevariables.playgroundSize.y / 2;
            return new Vector2(UnityEngine.Random.Range(-halfW, halfW), UnityEngine.Random.Range(-halfH, halfH));
        }

        public static animalType AnimalType()
        {
            System.Array vals = System.Enum.GetValues(typeof(animalType));
            int i = UnityEngine.Random.Range(0, vals.Length);
            return (animalType)vals.GetValue(i);
        }
    }

    public static class Conversion
    {
        public static Vector2Int Vector3ToVector2Int(Vector3 v)
        {
            return new Vector2Int((int)v.x, (int)v.y);
        }

        public static Vector2 Vector3ToVector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 SeedToCoordinates(string seed)
        {
            int range = 10000;
            return new Vector2(seed.GetHashCode() % range,
                                 seed.GetHashCode() % range);
        }
    }

    public static class UI
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

    public static class MapGeneration {

        //https://adrianb.io/2014/08/09/perlinnoise.html
        public static float OctavePerlin(float xOffset, float yOffset, PerlinSettingsObject pso)
        {
            float x = pso.xOrg + xOffset / pso.zoom;
            float y = pso.yOrg + yOffset / pso.zoom;

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
}
