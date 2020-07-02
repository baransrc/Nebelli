using System;
using UnityEngine;

public enum Direction
{
    Right, Left, Up, Down,
}

public class GameController : MonoBehaviour
{
    private Grid _grid;
    private int _gridWidth;
    private int _gridHeight;

    [SerializeField] private Player _player = NullObject.Player;
    [SerializeField] private TouchManager _touchManager = NullObject.TouchManager;

    [SerializeField] private GameObject _cellPrefab = NullObject.GameObject;
    [SerializeField] private GameObject _transformItemPrefab = NullObject.GameObject;
    [SerializeField] private GameObject _endGameItemPrefab = NullObject.GameObject;

    private void Awake()
    {
        _touchManager.OnDraggedRight += GoRight;
        _touchManager.OnDraggedLeft += GoLeft;
        _touchManager.OnDraggedUp += GoUp;
        _touchManager.OnDraggedDown += GoDown;

        _player.OnMovementEnd += ProcessPlayerCellInteraction;


        InitializeLevel();
    }

    private void InitializeLevel()
    {
        var levelProperties = LevelFileReader.ConvertFileToLevelProperties(PlayerPrefs.GetString(Strings.NextLevel_PlayerPref_String, "denemeLevel"));

        _gridWidth = levelProperties.GridWidth;
        _gridHeight = levelProperties.GridHeight;

        _grid = new Grid(_gridWidth, _gridHeight, _player);
        transform.position = (new Vector2(_gridWidth, _gridHeight) * -0.5f);

        for (var x = 0; x < _gridWidth; ++x)
        {
            for (var y = 0; y < _gridHeight; ++y)
            {
                _grid[x, y] = CreateCell();
                _grid[x, y].Enabled = levelProperties.GetCellEnabled(x, y);

                if (!_grid[x, y].Enabled) continue;

                var currentItemType = levelProperties.GetItemType(x, y);

                if (currentItemType == ItemType.None) continue;

                _grid[x, y].Item = GetItem(currentItemType, levelProperties.GetItemColor(x, y), _grid[x, y]);
            }
        }

        _player.LocalPosition = new Vector2(levelProperties.PlayerPosX, levelProperties.PlayerPosY);
        _player.SetColor(levelProperties.PlayerColor);
    }

    private void ProcessPlayerCellInteraction()
    {
        var currentCell = _grid[(int)_player.LocalPosition.x, (int)_player.LocalPosition.y];

        if (!currentCell.AcceptPlayer(_player))
            return;

        currentCell.React(_player);
    }

    private void GoLeft()
    {
        _player.Move(GetHorizontalPlayerDisplacement(-1), 0);
    }

    private void GoRight()
    {
        _player.Move(GetHorizontalPlayerDisplacement(1), 0);
    }

    private void GoUp()
    {
        _player.Move(0, GetVerticalPlayerDisplacement(1));
    }

    private void GoDown()
    {
        _player.Move(0, GetVerticalPlayerDisplacement(-1));
    }

    private float GetHorizontalPlayerDisplacement(int direction)
    {
        if (Mathf.Abs(direction) != 1)
            throw new InvalidOperationException("Direction must be either 1 or -1");
        
        var playerY = (int) _player.LocalPosition.y;
        var playerX = (int) _player.LocalPosition.x;
        var destination = playerX + direction;
        var currentCell = _grid[playerX, playerY];

        while (destination >= 0 && destination < _gridWidth)
        {
            currentCell = _grid[destination, playerY];

            if (!currentCell.Empty)
                break;

            destination += direction;
        }

        if (!CanPlayerActivateTheItemInCell(currentCell))
            destination -= direction;

        return Mathf.Abs(destination - playerX) * direction;
    }

    private float GetVerticalPlayerDisplacement(int direction)
    {
        if (Mathf.Abs(direction) != 1)
            throw new InvalidOperationException("Direction must be either 1 or -1");

        var playerY = (int) _player.LocalPosition.y;
        var playerX = (int) _player.LocalPosition.x;
        var destination = playerY + direction;
        var currentCell = _grid[playerX, playerY];

        while (destination >= 0 && destination < _gridHeight)
        {
            currentCell = _grid[playerX, destination];

            if (!currentCell.Empty)
                break;

            destination += direction;
        }

        if (!CanPlayerActivateTheItemInCell(currentCell))
            destination -= direction;

        return Mathf.Abs(destination - playerY) * direction;
    }

    //private float GetPlayerDisplacement(int direction, int threshold, bool isVertical)
    //{
    //    if (Mathf.Abs(direction) != 1)
    //        throw new InvalidOperationException("Direction must be either 1 or -1");

    //    var playerY = (int)_player.LocalPosition.y;
    //    var playerX = (int)_player.LocalPosition.x;
    //    var destination = playerY + direction;
    //    var currentCell = _grid[playerX, playerY];

    //    while (destination >= 0 && destination < threshold)
    //    {
    //        currentCell = _grid[playerX, destination];

    //        if (!currentCell.Empty)
    //            break;

    //        destination += direction;
    //    }

    //    if (!CanPlayerActivateTheItemInCell(currentCell))
    //        destination -= direction;

    //    return Mathf.Abs(destination - playerY) * direction;
    //}

    private bool CanPlayerActivateTheItemInCell(Cell currentCell)
    {
        return !currentCell.Empty && currentCell.AcceptPlayer(_player);
    }

    private Cell CreateCell()
    {
        return Instantiate(_cellPrefab, transform).GetComponent<Cell>();
    }

    #region Item Creation

    private Item GetItem(ItemType itemType, PredefinedColor color, Cell cell)
    {
        GameObject prefab = null;

        switch (itemType)
        {
            case ItemType.EndGameItem:
                prefab = _endGameItemPrefab;
                break;

            default:
            case ItemType.TransformItem:
                prefab = _transformItemPrefab;
                break;
        }

        var item = Instantiate(prefab, transform).GetComponent<Item>();
        item.Initialize(this, cell);
        item.SetColor(color);

        return item;
    }

    #endregion

    private void OnDestroy()
    {
        _touchManager.OnDraggedRight -= GoRight;
        _touchManager.OnDraggedLeft -= GoLeft;
        _touchManager.OnDraggedUp -= GoUp;
        _touchManager.OnDraggedDown -= GoDown;

        _player.OnMovementEnd -= ProcessPlayerCellInteraction;
    }
}
