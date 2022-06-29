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
        { 90, 90, 90, 90, 80, 80, 70, 70, 60, 60, 50, 50 }
    };

    /// <summary>
    /// Resuls matrix determines the percentage chance of hitting the opponent. The value returned is whether the Wound Test passed.
    /// </summary>
    /// <param name="defence">Unit's defence value.</param>
    /// <param name="strength">Strength of attacking unit or his weapon.</param>
    /// <returns> True if Wound Test passed, otherwise false.</returns>
    public static bool GetWoundTest(int defence, int strength)
    {
        var requiredResult = result[strength - 1, defence - 1];
        var woundResult = UnityEngine.Random.Range(1, 101);

        //UnityEngine.Debug.Log($"Wound chance: {requiredResult}% wound result: {woundResult <= requiredResult}");
        return woundResult <= requiredResult;
    }

    public static bool FearTest(int courage)
    {
        var testResult = UnityEngine.Random.Range(1, 11);
        return testResult <= courage;
    }
}