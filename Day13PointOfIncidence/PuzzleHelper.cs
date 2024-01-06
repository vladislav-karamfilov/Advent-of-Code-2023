namespace Day13PointOfIncidence;

internal static class PuzzleHelper
{
    public static List<List<char[]>> ReadPatterns()
    {
        var result = new List<List<char[]>>();

        var isReadingPattern = false;

        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                if (isReadingPattern)
                {
                    isReadingPattern = false;
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (!isReadingPattern)
            {
                result.Add([]);
            }

            isReadingPattern = true;

            result[^1].Add(line.ToCharArray());
        }

        return result;
    }

    public static int CalculateColsToTheLeftOfVerticalReflectionLine(List<char[]> pattern, int? skipCol = null)
    {
        var rowsCount = pattern.Count;
        var colsCount = pattern[0].Length;
        for (var col = 0; col < colsCount - 1; col++)
        {
            if (col == skipCol)
            {
                continue;
            }

            var potentiallyFormsVerticalReflectionLine = true;
            for (var row = 0; row < rowsCount; row++)
            {
                if (pattern[row][col] != pattern[row][col + 1])
                {
                    potentiallyFormsVerticalReflectionLine = false;
                    break;
                }
            }

            if (potentiallyFormsVerticalReflectionLine)
            {
                var leftCol = col - 1;
                var rightCol = col + 2;
                var formsVerticalReflectionLine = true;
                while (formsVerticalReflectionLine && leftCol >= 0 && rightCol < colsCount)
                {
                    for (var row = 0; row < rowsCount; row++)
                    {
                        if (pattern[row][leftCol] != pattern[row][rightCol])
                        {
                            formsVerticalReflectionLine = false;
                            break;
                        }
                    }

                    leftCol--;
                    rightCol++;
                }

                if (formsVerticalReflectionLine)
                {
                    return col + 1;
                }
            }
        }

        return 0;
    }

    public static int CalculateRowsAboveHorizontalReflectionLine(List<char[]> pattern, int? skipRow = null)
    {
        var rowsCount = pattern.Count;
        var colsCount = pattern[0].Length;
        for (var row = 0; row < rowsCount - 1; row++)
        {
            if (row == skipRow)
            {
                continue;
            }

            var potentiallyFormsHorizontalReflectionLine = true;
            for (var col = 0; col < colsCount; col++)
            {
                if (pattern[row][col] != pattern[row + 1][col])
                {
                    potentiallyFormsHorizontalReflectionLine = false;
                    break;
                }
            }

            if (potentiallyFormsHorizontalReflectionLine)
            {
                var upRow = row - 1;
                var downRow = row + 2;
                var formsHorizontalReflectionLine = true;
                while (formsHorizontalReflectionLine && upRow >= 0 && downRow < rowsCount)
                {
                    for (var col = 0; col < colsCount; col++)
                    {
                        if (pattern[upRow][col] != pattern[downRow][col])
                        {
                            formsHorizontalReflectionLine = false;
                            break;
                        }
                    }

                    upRow--;
                    downRow++;
                }

                if (formsHorizontalReflectionLine)
                {
                    return row + 1;
                }
            }
        }

        return 0;
    }
}
