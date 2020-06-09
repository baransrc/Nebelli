using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System.Linq;

public class LevelProperties
{
    public readonly int GridWidth;
    public readonly int GridHeight;
    
    public int PlayerPosX;
    public int PlayerPosY;
    public PredefinedColor PlayerColor;

    private bool[] _cellEnabled;
    private ItemType[] _itemType;
    private PredefinedColor[] _itemColor;

    public LevelProperties(int gridWidth, int gridHeight)
    {
        GridWidth = gridWidth;
        GridHeight = gridHeight;

        var size = GridHeight * GridWidth;

        _cellEnabled = Enumerable.Repeat(true, size).ToArray();
        _itemType = Enumerable.Repeat(ItemType.None, size).ToArray();
        _itemColor = Enumerable.Repeat(PredefinedColor.Colorless, size).ToArray();
    }

    public bool GetCellEnabled(int x, int y)
    {
        return _cellEnabled[GridHeight * x + y];
    }

    public ItemType GetItemType(int x, int y)
    {
        return _itemType[GridHeight *x + y];
    }

    public PredefinedColor GetItemColor(int x, int y)
    {
        return _itemColor[GridHeight * x + y];
    }

    public void SetCellEnabled(int x, int y, bool val)
    {
        _cellEnabled[GridHeight * x + y] = val;
    }

    public void SetItemType(int x, int y, ItemType val)
    {
        _itemType[GridHeight * x + y] = val;
    }

    public void SetItemColor(int x, int y, PredefinedColor val)
    {
        _itemColor[GridHeight * x + y] = val;
    }

}
