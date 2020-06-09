using System.Collections;
using UnityEngine;

public class VisualChangeManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer = NullObject.SpriteRenderer;
    [SerializeField] private float _colorChangeDuration = 0.2f;

    private Color _color;

    private void Awake()
    {
        
    }

    public void InitializeVisuals(PredefinedColor predefinedColor)
    {
        SetRendererColor(ColorReference.GetColor(predefinedColor));
    }

    protected virtual void SetRendererColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void ChangeColor(PredefinedColor predefinedColor)
    {
        StartCoroutine(ChangeColorCoroutine(ColorReference.GetColor(predefinedColor)));
    }

    public void ChangeColorImmediate(PredefinedColor predefinedColor)
    {
        SetRendererColor(ColorReference.GetColor(predefinedColor));
    }

    private IEnumerator ChangeColorCoroutine(Color endColor)
    {
        _spriteRenderer.color = _color;
        var startColor = _spriteRenderer.color;
        
        _color = endColor;

        var step = 0f;
        while (step < 1f)
        {
            step += Time.deltaTime / _colorChangeDuration;

            step = (step > 1f) ? 1f : step;

            SetRendererColor(Color.Lerp(startColor, endColor, step));

            yield return 0;
        }

        SetRendererColor(_color);
    }

    private void Update()
    {

    }
}
