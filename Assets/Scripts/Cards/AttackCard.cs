using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : Card {
    public override void Accept(CardVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override void Randomize()
    {
        
    }
}
