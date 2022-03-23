public static class RollToWound
{
    private static int[,] result =
    {
        { 4, 5, 5, 6, 6, 7, 7, 7, 0, 0 },
        { 4, 4, 5, 5, 6, 6, 7, 7, 7, 0 },
        { 3, 4, 4, 5, 5, 6, 6, 7, 7, 7 },
        { 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 },
        { 3, 3, 3, 4, 4, 5, 5, 6, 6, 7 },
        { 3, 3, 3, 3, 4, 4, 5, 5, 6, 6 },
        { 3, 3, 3, 3, 3, 4, 4, 5, 5, 6 },
        { 3, 3, 3, 3, 3, 3, 4, 4, 5, 5 },
        { 3, 3, 3, 3, 3, 3, 3, 4, 4, 5 },
        { 3, 3, 3, 3, 3, 3, 3, 3, 4, 4 }
    };

    public static bool GetWoundTest(int defence, int strength)
    {
        var requiredResult = result[defence - 1, strength - 1];
        var rollResult = RollTest.RollDiceD6();

        RollResultsPanel.instance.ShowResult(rollResult, "Roll to wound");

        if (requiredResult <= 6 && requiredResult <= rollResult)
            return true;

        else return false;
    }
}
