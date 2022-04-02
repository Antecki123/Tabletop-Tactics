public static class WoundTest
{
    // X - DEFENCE, Y - STRENGTH
    private static int[,] result =
    {
        { 50, 40, 40, 30, 30, 20, 20, 10, 10,  0,  0,  0 },
        { 50, 50, 40, 40, 30, 30, 20, 20, 10, 10,  0,  0 },
        { 60, 50, 50, 40, 40, 30, 30, 20, 20, 10, 10,  0 },
        { 60, 60, 50, 50, 40, 40, 30, 30, 20, 20, 10, 10 },
        { 70, 60, 60, 50, 50, 40, 40, 30, 30, 20, 20, 10 },
        { 70, 70, 60, 60, 50, 50, 40, 40, 30, 30, 20, 20 },
        { 80, 70, 70, 60, 60, 50, 50, 40, 40, 30, 30, 20 },
        { 80, 80, 70, 70, 60, 60, 50, 50, 40, 40, 30, 30 },
        { 90, 80, 80, 70, 70, 60, 60, 50, 50, 40, 40, 30 },
        { 90, 90, 80, 80, 70, 70, 60, 60, 50, 50, 40, 40 },
        { 90, 90, 90, 80, 80, 70, 70, 60, 60, 50, 50, 40 },
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