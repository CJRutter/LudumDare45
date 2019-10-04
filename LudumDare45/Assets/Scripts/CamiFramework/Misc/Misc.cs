using UnityEngine;
using System.Collections;
using System.Text;

public class PlatformHelper
{

    public static bool IsMobile()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.BlackBerryPlayer ||
            Application.platform == RuntimePlatform.WP8Player;
    }
}

public class ColourHelper
{
    public static Color RandomColour()
    {
        return RandomColour(0, 1, 0, 1, 0, 1, 1, 1);
    }

    public static Color RandomColour(
        float minRed, float maxRed, 
        float minGreen, float maxGreen, 
        float minBlue, float maxBlue,
        float minAlpha, float maxAlpha)
    {
        Color colour = new Color(
            Random.Range(minRed, maxRed),
            Random.Range(minGreen, maxGreen),
            Random.Range(minBlue, maxBlue),
            Random.Range(minAlpha, maxAlpha));
        return colour;
    }
}