using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinSettingsObject
{
    public int octaves;
    public float persistence;
    public float frequency;
    public float amplitude;

    public PerlinSettingsObject(float persistence, float frequency, int octaves, float amplitude)
    {
        this.persistence = persistence;
        this.frequency = frequency;
        this.octaves = octaves;
        this.amplitude = amplitude;
    }
}
