using System.ComponentModel;
using UnityEngine;

public static class ColorReference
{
    private static readonly float _inv255 = 1f / 255f;
    private static Color AquaMarine = new Color(120f, 225f, 212f, 255f) * _inv255;
    private static Color Blue = new Color(0, 66f, 225f, 255f) * _inv255;
    private static Color Colorless = new Color(120f, 120f, 120f, 255f) * _inv255;
    private static Color Green = new Color(0, 143f, 0, 255f) * _inv255;
    private static Color Red = new Color(147f, 12f, 31f, 255f) * _inv255;
    private static Color Lime = new Color(176f, 228f, 85f, 255f) * _inv255;
    private static Color Purple = new Color(167f, 28f, 200f, 255f) * _inv255;
    private static Color Pink = new Color(230f, 0, 163f, 255f) * _inv255;
    private static Color Tangerine = new Color(121f, 255f, 150f, 255f) * _inv255;
    private static Color Teal = new Color(0, 128f, 128f, 255f) * _inv255;
    private static Color Turquoise = new Color(0, 229f, 212f, 255f) * _inv255;
    private static Color White = new Color(255f, 255f, 222f, 255f) * _inv255;
    private static Color Yellow = new Color(255f, 167f, 16f, 255f) * _inv255;

    public static Color GetColor(PredefinedColor color)
    {
        switch (color)
        {
            case PredefinedColor.AquaMarine:
                return AquaMarine;
            case PredefinedColor.Blue:
                return Blue;
            case PredefinedColor.Colorless:
                return Colorless;
            case PredefinedColor.Green:
                return Green;
            case PredefinedColor.Lime:
                return Lime;
            case PredefinedColor.Pink:
                return Pink;
            case PredefinedColor.Purple:
                return Purple;
            case PredefinedColor.Red:
                return Red;
            case PredefinedColor.Tangerine:
                return Tangerine;
            case PredefinedColor.Teal:
                return Teal;
            case PredefinedColor.Turquoise:
                return Turquoise;
            case PredefinedColor.White:
                return White;
            case PredefinedColor.Yellow:
                return Yellow;
            default:
                throw new System.InvalidOperationException("Color must be either one of Predefined Colors.");
        }
    }
}
