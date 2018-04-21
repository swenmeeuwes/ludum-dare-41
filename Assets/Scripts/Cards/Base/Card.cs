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
}

public abstract class Card : Draggable
{
    [SerializeField] protected CardElements Elements;

    public void Destroy()
    {
        OnEndDrag();
        Destroy(gameObject);
    }

    public abstract void Accept(CardVisitor visitor);
    public abstract void Randomize();
}
