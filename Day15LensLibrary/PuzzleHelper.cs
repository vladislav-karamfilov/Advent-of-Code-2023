namespace Day15LensLibrary;

internal static class PuzzleHelper
{
    public static int CalculateHashValue(ReadOnlySpan<char> characters)
    {
        var currentValue = 0;
        for (var i = 0; i < characters.Length; i++)
        {
            currentValue += characters[i];
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }
}
