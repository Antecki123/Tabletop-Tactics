public static class WoundTest
{
    /// <summary>
    /// TODO: explain wound matrix
    /// </summary>
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
        { 90, 90, 90, 90, 80, 80, 70, 70, 60, 60, 50, 50 }
    };

    public static bool GetWoundTest(int defence, int strength)
    {
        var requiredResult = result[strength - 1, defence - 1];
        var woundResult = UnityEngine.Random.Range(1, 101);

        UnityEngine.Debug.Log($"Wound chance: {requiredResult}% wound result: {woundResult <= requiredResult}");
        return (woundResult <= requiredResult);
    }

    public static bool IsPossibleToAttack(int defence, int strength)
    {
        var possibility = result[strength - 1, defence - 1] != 0;
        return possibility;
    }
}