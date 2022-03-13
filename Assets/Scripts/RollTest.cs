using UnityEngine;

public class RollTest
{
    public static int RollDiceD3()
    {
        var result = Random.Range(1, 4);
        return result;
    }

    public static int RollDiceD6()
    {
        var result = Random.Range(1, 7);
        return result;
    }
}