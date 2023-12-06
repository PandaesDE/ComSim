/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - helper class with specialized subclasses
 *  
 *  References:
 *      Scene:
 *          - scene independent
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System.Text.RegularExpressions;
using UnityEngine;

public static class Util
{
    public static bool InRange(Vector2 position, Vector2 destination, float range = 1)
    {
        return
            position.x > destination.x - range &&
            position.x < destination.x + range &&
            position.y > destination.y - range &&
            position.y < destination.y + range;
    }

    public static int RoundFloatUpPositiveDownNegative(float val)
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
        public static bool IsMale()
        {
            return UnityEngine.Random.value > .5f;
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

        public static Vector2 CoordinateInAreaOfPlayground(int areaWidth, int areaHeight, Vector2 areaMiddle)
        {
            int halfWidth = areaWidth / 2;
            int halfHeight = areaHeight / 2;
            int playGroundLimiterX = Gamevariables.PLAYGROUND_SIZE.x / 2;
            int playGroundLimiterY = Gamevariables.PLAYGROUND_SIZE.y / 2;

            int xMin = Mathf.Clamp((int)areaMiddle.x - halfWidth, -playGroundLimiterX, playGroundLimiterX);
            int xMax = Mathf.Clamp((int)areaMiddle.x + halfWidth, -playGroundLimiterX, playGroundLimiterX);
            int yMin = Mathf.Clamp((int)areaMiddle.y - halfHeight, -playGroundLimiterY, playGroundLimiterY);
            int yMax = Mathf.Clamp((int)areaMiddle.y + halfHeight, -playGroundLimiterY, playGroundLimiterY);

            return new Vector2(
                UnityEngine.Random.Range(xMin, xMax),
                UnityEngine.Random.Range(yMin, yMax));
        }

        public static Vector2 CoordinateInPlayground()
        {
            int maxWidth = Gamevariables.PLAYGROUND_SIZE.x / 2;
            int maxHeight = Gamevariables.PLAYGROUND_SIZE.y / 2;
            return new Vector2(
                UnityEngine.Random.Range(-maxWidth, maxWidth), 
                UnityEngine.Random.Range(-maxHeight, maxHeight));
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
        public static string PreventNullOrEmptyInputString(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return "";
            return txt;
        }

        public static string PreventNullOrEmptyInputNumber(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return "0";
            return txt;
        }

        public static bool IsValidNumericEntry(string txt)
        {
            if (txt.Length <= 0) return false;
            if (!new Regex("[0-9-.]").IsMatch(txt)) return false;
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
