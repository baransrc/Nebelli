using UnityEngine;
using System.Collections;

public class EndGameItem : Item
{
    public override Vector2 LocalPosition
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

    public override PredefinedColor Color { get; protected set; }

    public override void Activate(Player player)
    {
        Debug.Log("Game has ended.");
        player.SetColor(Color);
    }

    public override bool CanCollide(Player player)
    {
        return Color == PredefinedColor.Colorless || player.Color != Color;
    }

    public override ItemType GetItemType()
    {
        return ItemType.EndGameItem;
    }

    public override void Initialize(GameController gameController, Cell cell)
    {
        _gameController = gameController;
        _cell = cell;
        Color = PredefinedColor.Colorless;
        LocalPosition = _cell.LocalPosition;
    }

    public override void SetColor(PredefinedColor color)
    {
        Color = color;
        _visualChangeManager.ChangeColor(Color);
    }
}
