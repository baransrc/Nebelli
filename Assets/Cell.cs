using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer = NullObject.SpriteRenderer;

    public Item Item { get; set; }

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

    public bool Empty 
    { 
        get
        {
            return Enabled && Item == NullObject.Item;
        }
        
        private set { }
    }

    private bool _enabled = true;
    public bool Enabled
    {
        get
        {
            return _enabled;
        }

        set
        {
            _enabled = value;
            _spriteRenderer.enabled = value;
        }
    }

    public void React(Player player)
    {
        if (Empty) return;

        Item.Activate(player);
    }

    public bool AcceptPlayer(Player player)
    {
        if (!Enabled) return false;
        if (Empty) return true;

        return Item.CanCollide(player);
    }
}
