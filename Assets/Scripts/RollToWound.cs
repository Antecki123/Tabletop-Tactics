public static class RollToWound
{
    // X - DEFENCE, Y - STRENGTH
    private static int[,] result =
    {
        { 4, 5, 5, 6, 6, 7, 8, 9, 0, 0 },
        { 4, 4, 5, 5, 6, 6, 7, 8, 9, 0 },
        { 3, 4, 4, 5, 5, 6, 6, 7, 8, 9 },
        { 3, 3, 4, 4, 5, 5, 6, 6, 7, 8 },
        { 3, 3, 3, 4, 4, 5, 5, 6, 6, 7 },
        { 3, 3, 3, 3, 4, 4, 5, 5, 6, 6 },
        { 3, 3, 3, 3, 3, 4, 4, 5, 5, 6 },
        { 3, 3, 3, 3, 3, 3, 4, 4, 5, 5 },
        { 3, 3, 3, 3, 3, 3, 3, 4, 4, 5 },
        { 3, 3, 3, 3, 3, 3, 3, 3, 4, 4 }
    };

    public static bool GetWoundTest(int defence, int strength)
    {
        var requiredResult = result[strength - 1, defence - 1];
        var rollResult = RollTest.RollDiceD6();
        var secondRollResult = RollTest.RollDiceD6();

        //RollResultsPanel.instance.ShowResult(rollResult, "Roll to wound");

        if (requiredResult <= 6)
        {
            if (requiredResult <= rollResult)
                return true;
        }

        else
        {
            //RollResultsPanel.instance.ShowResult(secondRollResult, "Roll to wound");

            if (requiredResult == 7 && rollResult == 6 && secondRollResult >= 4)
                return true;

            else if (requiredResult == 8 && rollResult == 6 && secondRollResult >= 5)
                return true;

            else if (requiredResult == 9 && rollResult == 6 && secondRollResult >= 6)
                return true;
        }
        return false;
    }

    public static bool IsPossibleToAttack(int defence, int strength)
    {
        var possibility = result[strength - 1, defence - 1] != 0;
        return possibility;
    }
}