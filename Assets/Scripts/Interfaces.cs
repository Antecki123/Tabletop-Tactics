using UnityEngine;

public interface IHighlightGrid
{
    public void HighlightGridMovement(Unit unit, int range, Color color);
    public void HighlightGridRange(Unit unit, int range, Color color);
    public void ClearHighlight();
}