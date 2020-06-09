using UnityEngine;
using System.Collections;


public class TransformItem : Item
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

    public override PredefinedColor Color { get; protected set ; }

    public override void Activate(Player player)
    {
        player.SetColor(Color);
        Destroy(gameObject);
    }

    public override bool CanCollide(Player player)
    {
        return player.Color != Color;
    }

    public override ItemType GetItemType()
    {
        return ItemType.TransformItem;
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
