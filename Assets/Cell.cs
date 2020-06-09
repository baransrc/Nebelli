using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;

public class Cell : MonoBehaviour
{
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
            return Item == NullObject.Item;
        }
        
        private set { }
    }

    public void React(Player player)
    {
        if (Empty) return;

        Item.Activate(player);
    }

    public bool AcceptPlayer(Player player)
    {
        if (Empty) return true;

        return Item.CanCollide(player);
    }
}
