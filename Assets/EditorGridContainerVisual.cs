using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGridContainerVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer = NullObject.SpriteRenderer;

    private void Awake()
    {
        EnableSprite(false);
    }

    public void EnableSprite(bool enabled)
    {
        _spriteRenderer.enabled = enabled;
    }

    public void SetColor(PredefinedColor predefinedColor)
    {
        _spriteRenderer.color = ColorReference.GetColor(predefinedColor);    
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
