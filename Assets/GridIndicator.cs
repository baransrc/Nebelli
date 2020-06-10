using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class GridIndicator : MonoBehaviour
{
    [SerializeField] private EditorGridContainerVisual _cellVisual;
    [SerializeField] private EditorGridContainerVisual _itemVisual;

    private LevelEditor _levelEditor;


    public Vector2 LocalPosition 
    { 
        get
        {
            return transform.localPosition;
        }

        set
        {
            transform.localPosition = value;
        }
    }

    private bool _enabled;
    public bool Enabled 
    {
        get
        {
            return _enabled;
        }
        set
        {
            _enabled = value;

            _cellVisual.EnableSprite(value);
            _itemVisual.EnableSprite(value);

            if (_enabled) return;

            ItemType = ItemType.None;
            Color = PredefinedColor.Colorless;
        }
    }

    private ItemType _itemType;
    public ItemType ItemType
    {
        get
        {
            return _itemType;
        }

        set 
        {
            _itemType = value;
            _itemVisual.SetSprite(_levelEditor.GetSpriteByItemType(_itemType));
        }
    }

    private PredefinedColor _color;
    public PredefinedColor Color
    {
        get
        {
            return _color;
        }

        set
        {
            _color = value;
            _itemVisual.SetColor(_color);
        }
    }

    public void Initialize(LevelEditor levelEditor)
    {
        _levelEditor = levelEditor;
        Enabled = false;
    }
}
