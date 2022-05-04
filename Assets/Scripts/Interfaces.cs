using UnityEngine;

public interface IHighlightGrid
{
    public bool IsHighlighted { get; }
    public void HighlightGridMovement(Unit unit, int range, Color color);
    public void HighlightGridRange(Unit unit, int range, Color color);
    public void ClearHighlight();
}

public interface IMapBuilder
{
    public void Execute(BattlefieldCreator manager);
    public void Response();
}