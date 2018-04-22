public class SpinAttackCard : Card
{
    private void Awake()
    {
        Cost = 2;
    }

    public override void Accept(CardVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override void Randomize()
    {

    }
}
