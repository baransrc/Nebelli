using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public enum InsertMode
{
    AddCell,
    DeleteCell
}

public class LevelEditor : MonoBehaviour
{
    [System.Serializable] 
    public struct EditorItemType
    {
        public Sprite Sprite;
        public ItemType ItemType;
    }

    [SerializeField] private Camera _mainCamera = NullObject.Camera;
    [SerializeField] private GameObject _gridIndicatorPrefab = NullObject.GameObject;
    [SerializeField] public List<EditorItemType> _editorItemTypes;
    [SerializeField] private Vector2Int _defaultGridSize = new Vector2Int(10, 16);

    [SerializeField] private TMPro.TMP_Dropdown _itemTypeDropdown = NullObject.TMP_Dropdown;
    [SerializeField] private TMPro.TMP_Dropdown _itemColorDropdown = NullObject.TMP_Dropdown;
    [SerializeField] private TMPro.TMP_Dropdown _insertModeDropdown = NullObject.TMP_Dropdown;

    [SerializeField] private LevelFileReader _levelFileReader = NullObject.LevelFileReader;

    private InsertMode _insertMode;
    private PredefinedColor _itemColor;
    private ItemType _itemType;

    private GridIndicator[,] _gridIndicators;

    private void Awake()
    {
        transform.position = new Vector3(_defaultGridSize.x, _defaultGridSize.y, 0f) * -0.5f;
        _gridIndicators = new GridIndicator[_defaultGridSize.x, _defaultGridSize.y];
        
        InitializeGridIndicator();

        PopulateDropdown(_itemTypeDropdown, typeof(ItemType));
        PopulateDropdown(_itemColorDropdown, typeof(PredefinedColor));
        PopulateDropdown(_insertModeDropdown, typeof(InsertMode));
        UpdateItemType();
        UpdateItemColor();
        UpdateInsertMode();
    }

    private void OnEnable()
    {
        _itemTypeDropdown.onValueChanged.AddListener(delegate { UpdateItemType(); });
        _itemColorDropdown.onValueChanged.AddListener(delegate { UpdateItemColor(); });
        _insertModeDropdown.onValueChanged.AddListener(delegate { UpdateInsertMode(); });
    }

    private void OnDestroy()
    {
        _itemTypeDropdown.onValueChanged.RemoveAllListeners();
        _itemColorDropdown.onValueChanged.RemoveAllListeners();
        _insertModeDropdown.onValueChanged.RemoveAllListeners();
    }

    private void PopulateDropdown(TMPro.TMP_Dropdown dropdown, Type type)
    {
        var enums = Enum.GetNames(type).ToList<string>();
        dropdown.AddOptions(enums);
    }

    private GridIndicator InstantiateGridIndicator(int x, int y)
    {
        var gridIndicator = Instantiate(_gridIndicatorPrefab, transform).GetComponent<GridIndicator>();
        gridIndicator.Initialize(this);
        gridIndicator.LocalPosition = new Vector2(x, y);

        return gridIndicator;
    }

    private void InitializeGridIndicator()
    {
        for(var i = 0; i < 10; ++i)
        {
            for (var j = 0; j < 16; ++j)
            {
                _gridIndicators[i, j] = InstantiateGridIndicator(i, j);
            }    
        }
    }

    public Sprite GetSpriteByItemType(ItemType itemType)
    {
        return _editorItemTypes.Find(x => x.ItemType == itemType).Sprite;
    }

    private GridIndicator GetGridByPosition(Vector2Int position)
    {
        if (position.x >= _defaultGridSize.x || position.x < 0) 
            return null;
        if (position.y >= _defaultGridSize.y || position.y < 0) 
            return null;

        return _gridIndicators[position.x, position.y];
    }

    private bool CanDetectTouch()
    {
        return !_itemTypeDropdown.IsExpanded && !_itemColorDropdown.IsExpanded && !_insertModeDropdown.IsExpanded;
    }

    private void UpdateInsertMode()
    {
        _insertMode = (InsertMode)Enum.Parse((typeof(InsertMode)), _insertModeDropdown.options[_insertModeDropdown.value].text);
    }

    private void UpdateItemType()
    {
        _itemType = (ItemType)Enum.Parse((typeof(ItemType)), _itemTypeDropdown.options[_itemTypeDropdown.value].text);
    }

    private void UpdateItemColor()
    {
        _itemColor = (PredefinedColor)Enum.Parse((typeof(PredefinedColor)), _itemColorDropdown.options[_itemColorDropdown.value].text);
    }

    public void Play(string levelName)
    {
        Debug.Log("Play: " + levelName);        
    }

    public void Load(string levelName)
    {
        foreach (var grid in _gridIndicators)
        {
            grid.Enabled = false;
        }

        _levelFileReader.SetFileName(levelName);
        var levelProperties = _levelFileReader.GetCurrentLevelProperties();
        var gridWidth = levelProperties.GridWidth;
        var gridHeight = levelProperties.GridHeight;


        for (var x = 0; x < gridWidth; ++x)
        {
            for (var y = 0; y < gridHeight; ++y)
            {
                var enabled = levelProperties.GetCellEnabled(x, y);

                if (!enabled) continue;

                var current = GetGridByPosition(new Vector2Int(x, y));

                current.Enabled = enabled;

                current.Color = levelProperties.GetItemColor(x, y);

                current.ItemType = levelProperties.GetItemType(x, y);
            }
        }
    }

    public void Save(string levelName)
    {
        Debug.Log("Save: " + levelName);
    }

    private void ParseMouseEvent()
    {
        if (!Input.GetMouseButton(0))
            return;

        if (!CanDetectTouch())
            return;

        var touchPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var touchPositionInt = new Vector2Int((int)touchPosition.x, (int)touchPosition.y);
            
        var correspondingGrid = GetGridByPosition(touchPositionInt);

        if (correspondingGrid == null) 
            return;

        var inserting = (_insertMode != InsertMode.DeleteCell);

        correspondingGrid.Enabled = inserting;
        correspondingGrid.ItemType = (inserting) ? _itemType : ItemType.None;
        correspondingGrid.Color = (inserting) ? _itemColor : PredefinedColor.Colorless;
    }

    private void Update()
    {
        ParseMouseEvent();   
    }
}
