using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InsertMode
{
    AddCell,
    DeleteCell,
    AddPlayer,
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
    [SerializeField] private TMPro.TMP_InputField _levelNameInputField = NullObject.TMP_InputField;

    private InsertMode _insertMode;
    private PredefinedColor _itemColor;
    private ItemType _itemType;

    private GridIndicator[,] _gridIndicators;
    private Vector2Int _lastPlayerLocation = new Vector2Int(0,0);

    private void Awake()
    {
        transform.position = new UnityEngine.Vector3(_defaultGridSize.x, _defaultGridSize.y, 0f) * -0.5f;
        _gridIndicators = new GridIndicator[_defaultGridSize.x, _defaultGridSize.y];
        
        InitializeGridIndicator();

        PopulateDropdown(_itemTypeDropdown, typeof(ItemType));
        PopulateDropdown(_itemColorDropdown, typeof(PredefinedColor));
        PopulateDropdown(_insertModeDropdown, typeof(InsertMode));
        UpdateItemType();
        UpdateItemColor();
        UpdateInsertMode();

        GetGridByPosition(_lastPlayerLocation).HasPlayer = true;

        var currentLevel = PlayerPrefs.GetString(Strings.NextLevel_PlayerPref_String, "denemeLevel.lvl");
        _levelNameInputField.text = currentLevel;
        Load(currentLevel);
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
        gridIndicator.LocalPosition = new UnityEngine.Vector2(x, y);

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
        PlayerPrefs.SetString(Strings.NextLevel_PlayerPref_String, levelName);
        SceneManager.LoadScene(SceneNames.GameScene);
    }

    public void Load(string levelName)
    {
        foreach (var grid in _gridIndicators)
        {
            grid.Enabled = false;
            grid.HasPlayer = false;
        }

        var levelProperties = LevelFileReader.ConvertFileToLevelProperties(levelName);
        var gridWidth = levelProperties.GridWidth;
        var gridHeight = levelProperties.GridHeight;

        var playerPos = new Vector2Int(levelProperties.PlayerPosX, levelProperties.PlayerPosY);
        var playerGrid = GetGridByPosition(playerPos);
        
        playerGrid.HasPlayer = true;
        playerGrid.Color = levelProperties.PlayerColor;

        _lastPlayerLocation = playerPos;

        for (var x = 0; x < gridWidth; ++x)
        {
            for (var y = 0; y < gridHeight; ++y)
            {
                var enabled = levelProperties.GetCellEnabled(x, y);

                if (!enabled) 
                    continue;

                if (GetGridByPosition(new Vector2Int(x, y)).HasPlayer)
                    continue;

                var current = GetGridByPosition(new Vector2Int(x, y));

                current.Enabled = enabled;

                current.Color = levelProperties.GetItemColor(x, y);

                current.ItemType = levelProperties.GetItemType(x, y);
            }
        }
    }

    public string GetSaveFile(string levelName)
    {
        var minY = _defaultGridSize.y;
        var maxY = 0;
        var minX = _defaultGridSize.x;
        var maxX = 0;
        var isEmpty = true;
        var hasPlayer = false;
        var playerPos = Vector2Int.zero;

        for (var x = 0; x < _defaultGridSize.x; ++x)
        {
            for (var y = 0; y < _defaultGridSize.y; ++y)
            {
                var currentPos = new Vector2Int(x, y);
                var grid = GetGridByPosition(currentPos);

                if (!grid.Enabled)
                    continue;

                isEmpty = false;

                if (grid.HasPlayer)
                {
                    hasPlayer = true;
                    playerPos = currentPos;
                }
                
                if (x > maxX) 
                    maxX = x;
                if (x < minX)
                    minX = x;
                if (y > maxY)
                    maxY = y;
                if (y < minY)
                    minY = y;
            }
        }

        if (isEmpty) 
            return Strings.Empty;

        if (!hasPlayer)
            return Strings.NoPlayer;

        var gridWidth = maxX - minX + 1;
        var gridHeight = maxY - minY + 1;

        var levelString = "Width:" + gridWidth + 
                          "\nHeight:" + gridHeight + 
                          "\nPlayer:" + (playerPos.x-minX) + "," + (playerPos.y-minY) + " " + (int)GetGridByPosition(playerPos).Color;

        for (var x = minX; x <= maxX; ++x)
        {
            for (var y = minY; y <= maxY; ++y)
            {
                var grid = GetGridByPosition(new Vector2Int(x, y));
                
                if (grid.HasPlayer)
                    continue;

                levelString += "\n" + (x - minX) + "," + (y - minY) + ":" + (grid.Enabled ? 1 : 0) + " " + (int)grid.ItemType + " " + (int)grid.Color;
            }
        }
        
        return levelString;
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

        correspondingGrid.ItemType = (_insertMode == InsertMode.AddCell) ? _itemType : ItemType.None;
        
        correspondingGrid.HasPlayer = (_insertMode == InsertMode.AddPlayer);
        
        correspondingGrid.Color = (inserting) ? _itemColor : PredefinedColor.Colorless;

        if (_insertMode != InsertMode.AddPlayer)
            return;

        if (_lastPlayerLocation == touchPositionInt)
            return;

        var previousPlayerGrid = GetGridByPosition(_lastPlayerLocation);

        previousPlayerGrid.HasPlayer = false;
        previousPlayerGrid.Enabled = true;

        _lastPlayerLocation = touchPositionInt;
    }

    private void Update()
    {
        ParseMouseEvent();   
    }
}
