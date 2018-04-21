using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardElements
{
    public Text Header;
    public Image Image;
    public Text Description;
    public Text StaminaCost;
}

public abstract class Card : Draggable
{
    [SerializeField] protected CardElements Elements;

    private int _cost;
    public int Cost {
        get { return _cost; }
        set
        {
            Elements.StaminaCost.text = value.ToString();
            _cost = value;
        }
    }

    public void Destroy()
    {
        OnEndDrag();
        CurrentDropTarget.Remove(this);
        Destroy(gameObject);
    }

    public abstract void Accept(CardVisitor visitor);
    public abstract void Randomize();
}
