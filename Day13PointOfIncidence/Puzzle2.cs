namespace Day13PointOfIncidence;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/13#part2
    public static void Solve()
    {
        var patterns = PuzzleHelper.ReadPatterns();

        var sum = CalculateSumOfColsOrRowsFromNewReflectionLines(patterns);

        Console.WriteLine(sum);
    }

    private static long CalculateSumOfColsOrRowsFromNewReflectionLines(List<List<char[]>> patterns)
    {
        var sum = 0L;
        foreach (var pattern in patterns)
        {
            var originalColsToTheLeft = PuzzleHelper.CalculateColsToTheLeftOfVerticalReflectionLine(pattern);
            var originalRowsAbove = originalColsToTheLeft == 0
                ? PuzzleHelper.CalculateRowsAboveHorizontalReflectionLine(pattern)
                : -1;

            var newReflectionLineFound = false;
            for (var row = 0; row < pattern.Count; row++)
            {
                for (var col = 0; col < pattern[row].Length; col++)
                {
                    if (pattern[row][col] == '.')
                    {
                        pattern[row][col] = '#';

                        var colsOrRows = CalculateColsOrRowsFromNewReflectionLine(pattern, originalColsToTheLeft, originalRowsAbove);

                        pattern[row][col] = '.';

                        if (colsOrRows > 0)
                        {
                            sum += colsOrRows;
                            newReflectionLineFound = true;
                            break;
                        }
                    }
                    else
                    {
                        pattern[row][col] = '.';

                        var colsOrRows = CalculateColsOrRowsFromNewReflectionLine(pattern, originalColsToTheLeft, originalRowsAbove);

                        pattern[row][col] = '#';

                        if (colsOrRows > 0)
                        {
                            sum += colsOrRows;
                            newReflectionLineFound = true;
                            break;
                        }
                    }
                }

                if (newReflectionLineFound)
                {
                    break;
                }
            }
        }

        return sum;
    }

    private static int CalculateColsOrRowsFromNewReflectionLine(List<char[]> pattern, int originalColsToTheLeft, int originalRowsAbove)
    {
        var colsToTheLeft = PuzzleHelper.CalculateColsToTheLeftOfVerticalReflectionLine(pattern, originalColsToTheLeft - 1);
        if (colsToTheLeft > 0 && colsToTheLeft != originalColsToTheLeft)
        {
            return colsToTheLeft;
        }

        var rowsAbove = PuzzleHelper.CalculateRowsAboveHorizontalReflectionLine(pattern, originalRowsAbove - 1);
        if (rowsAbove > 0 && rowsAbove != originalRowsAbove)
        {
            return 100 * rowsAbove;
        }

        return 0;
    }
}
