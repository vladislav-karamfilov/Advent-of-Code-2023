namespace Day13PointOfIncidence;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/13
    public static void Solve()
    {
        var patterns = PuzzleHelper.ReadPatterns();

        var sum = CalculateColsOrRowsFromReflectionLine(patterns);

        Console.WriteLine(sum);
    }

    private static long CalculateColsOrRowsFromReflectionLine(List<List<char[]>> patterns)
    {
        var sum = 0L;
        foreach (var pattern in patterns)
        {
            var colsToTheLeft = PuzzleHelper.CalculateColsToTheLeftOfVerticalReflectionLine(pattern);
            if (colsToTheLeft == 0)
            {
                sum += 100 * PuzzleHelper.CalculateRowsAboveHorizontalReflectionLine(pattern);
            }
            else
            {
                sum += colsToTheLeft;
            }
        }

        return sum;
    }
}
