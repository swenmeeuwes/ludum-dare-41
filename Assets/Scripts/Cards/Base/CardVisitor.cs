public interface CardVisitor
{
    void Visit(MoveCard card);
    void Visit(AttackCard card);
}
