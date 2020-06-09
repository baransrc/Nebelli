using UnityEngine;
using System.Collections;

public class PlayerVisualChangeManager : VisualChangeManager
{
    [SerializeField] private TrailRenderer _trailRenderer = NullObject.TrailRenderer;

    protected override void SetRendererColor(Color color)
    {
        base.SetRendererColor(color);
        _trailRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
    }

}
