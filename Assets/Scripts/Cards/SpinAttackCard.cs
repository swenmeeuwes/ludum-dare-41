public class SpinAttackCard : Card
{
    protected override void Start()
    {
        base.Start();

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
