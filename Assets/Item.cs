using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    public abstract PredefinedColor Color { get; protected set; }
    [SerializeField] protected VisualChangeManager _visualChangeManager;
    protected Cell _cell;
    protected GameController _gameController;

    public abstract Vector2 LocalPosition { set; get; }

    public abstract bool CanCollide(Player player);

    public abstract void Initialize(GameController gameController, Cell cell);

    public abstract void Activate(Player player);

    public abstract void SetColor(PredefinedColor color);
}
