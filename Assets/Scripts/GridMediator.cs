using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMediator : IGridMediator
{
    private GridManager gridManager;
    private GridBuilder gridBuilder;



    public void Notify(Object sender, string text)
    {
        if (true)
        {

        }
    }
}

public interface IGridMediator
{
    public void Notify(Object sender, string text);
}