using System.Collections.Generic;
using UnityEngine;

public static class NullObject
{
    // Unity:
    public readonly static SpriteRenderer SpriteRenderer = new SpriteRenderer();
    public readonly static List<RectTransform> RectTransformList = new List<RectTransform>();
    public readonly static TrailRenderer TrailRenderer = new TrailRenderer();
    public readonly static GameObject GameObject = null;
    public readonly static Camera Camera = null;

    // MonoBehaviour:
    public readonly static LevelEditor LevelEditor = null;
    public readonly static LevelFileReader LevelFileReader = null;
    public readonly static PlayerVisualChangeManager PlayerVisualChangeManager = null;
    public readonly static EditorGridContainerVisual EditorGridContainerVisual = null;
    public readonly static Player Player = null;
    public readonly static TouchManager TouchManager = null;
    public readonly static Item Item = null;

    // TextMeshPro Related:
    public readonly static TMPro.TextMeshProUGUI TextMeshProUGUI = null;
    public readonly static TMPro.TMP_Dropdown TMP_Dropdown = null;
    public readonly static TMPro.TMP_InputField TMP_InputField = null; 
}
