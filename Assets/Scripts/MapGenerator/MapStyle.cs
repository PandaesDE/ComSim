using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStyle
{
    //used colors
    private Dictionary<mapLayers, Color> mapColors;

    private Gradient mapGradient;

    private enum mapLayers
    {
        DARK_WATER,
        BRIGHT_WATER,
        SAND,
        BRIGHT_GRASS,
        DARK_GRASS,
        BRIGHT_STONE,
        DARK_STONE,
        SNOW
    }

    public enum styles
    {
        NORMAL,
        GRADIENT
    }

    public MapStyle()
    {
        initializeMapColors();
        initializeMapGradient();
    }

    public Color getColor(float sample, styles ms)
    {
        if (ms.Equals(styles.GRADIENT))
        {
            return getGradientColor(sample);
        }

        return getNormalColor(sample); ;
    }
    #region
    private Color getNormalColor(float sample)
    {
        if (sample < .1f)
        {
            return mapColors[mapLayers.DARK_WATER];
        }
        if (sample < .2f)
        {
            return mapColors[mapLayers.BRIGHT_WATER];
        }
        if (sample < .23f)
        {
            return mapColors[mapLayers.SAND];
        }
        if (sample < .35f)
        {
            return mapColors[mapLayers.BRIGHT_GRASS];
        }
        if (sample < .6f)
        {
            return mapColors[mapLayers.DARK_GRASS];
        }
        if (sample < .65f)
        {
            return mapColors[mapLayers.BRIGHT_STONE];
        }
        if (sample < .8f)
        {
            return mapColors[mapLayers.DARK_STONE];
        }
        return mapColors[mapLayers.SNOW];
    }
    #endregion

    #region Gradient
    private Color getGradientColor(float sample)
    {
        return mapGradient.Evaluate(sample);
    }

    void initializeMapGradient()
    {
        mapGradient = new Gradient();
        GradientColorKey[] colorKey = new GradientColorKey[8];
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[8];
        int index = 0;

        addColorKey(mapColors[mapLayers.DARK_WATER], 0.0f);
        addColorKey(mapColors[mapLayers.BRIGHT_WATER], 0.2f);
        addColorKey(mapColors[mapLayers.SAND], 0.21f);
        addColorKey(mapColors[mapLayers.BRIGHT_GRASS], 0.3f);
        addColorKey(mapColors[mapLayers.DARK_GRASS], 0.6f);
        addColorKey(mapColors[mapLayers.BRIGHT_STONE], 0.61f);
        addColorKey(mapColors[mapLayers.DARK_STONE], 0.8f);
        addColorKey(mapColors[mapLayers.SNOW], 1.0f);

        mapGradient.SetKeys(colorKey, alphaKey);

        void addColorKey(Color c, float t)
        {
            colorKey[index].color = c;
            colorKey[index].time = t;
            alphaKey[index].alpha = 1.0f;
            alphaKey[index].time = t;
            index++;
        }
    }
    #endregion

    private void initializeMapColors()
    {
        mapColors = new Dictionary<mapLayers, Color>();
        mapColors.Add(mapLayers.DARK_WATER, new Color32(19, 49, 138, 255));
        mapColors.Add(mapLayers.BRIGHT_WATER, new Color32(135, 170, 232, 255));
        mapColors.Add(mapLayers.SAND, new Color32(247, 246, 195, 255));
        mapColors.Add(mapLayers.BRIGHT_GRASS, new Color32(121, 199, 99, 255));
        mapColors.Add(mapLayers.DARK_GRASS, new Color32(79, 121, 66, 255));
        mapColors.Add(mapLayers.BRIGHT_STONE, new Color32(87, 85, 80, 255));
        mapColors.Add(mapLayers.DARK_STONE, new Color32(163, 163, 163, 255));
        mapColors.Add(mapLayers.SNOW, new Color32(255, 255, 255, 255));

    }
}
