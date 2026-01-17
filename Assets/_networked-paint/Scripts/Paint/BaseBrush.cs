using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush", menuName = "Painting/Brush")]
public class BaseBrush : ScriptableObject
{

    public string DisplayName;
    public Texture2D BrushTexture;
    public bool bIsTintable = true;

    public Color Apply(Color InCurrentColor, Color InBrushColor, Color InTintColor, float InWeight)
    {
        Color DesiredColor = bIsTintable ? InTintColor : InBrushColor;
        float Intensity = InWeight * ( bIsTintable ? InBrushColor.r : 1f);
        return Color.Lerp(InCurrentColor, DesiredColor, Intensity);
    }

}
