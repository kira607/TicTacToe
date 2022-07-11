using UnityEngine;

public static class ColorMaker
{
    public static Color MakeColor(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }

    public static Color MakeGrey(float rgb)
    {
        return MakeColor(rgb, rgb, rgb);
    }
}