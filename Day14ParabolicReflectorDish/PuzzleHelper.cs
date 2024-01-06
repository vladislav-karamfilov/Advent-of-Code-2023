namespace Day14ParabolicReflectorDish;

internal static class PuzzleHelper
{
    public static List<char[]> ReadPlatform()
    {
        var result = new List<char[]>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            result.Add(line.ToCharArray());
        }

        return result;
    }

    public static int CalculateLoadOnNorthSupportBeams(List<char[]> platform)
    {
        var sum = 0;
        for (var row = 0; row < platform.Count; row++)
        {
            for (var col = 0; col < platform[row].Length; col++)
            {
                if (platform[row][col] == 'O')
                {
                    sum += platform.Count - row;
                }
            }
        }

        return sum;
    }

    public static void TiltPlatformToWest(List<char[]> platform)
    {
        for (var col = 1; col < platform[0].Length; col++)
        {
            for (var row = 0; row < platform.Count; row++)
            {
                if (platform[row][col] == 'O')
                {
                    var colToMoveOn = col - 1;
                    while (colToMoveOn >= 0 && platform[row][colToMoveOn] == '.')
                    {
                        platform[row][colToMoveOn] = 'O';
                        platform[row][colToMoveOn + 1] = '.';

                        colToMoveOn--;
                    }
                }
            }
        }
    }

    public static void TiltPlatformToEast(List<char[]> platform)
    {
        for (var col = platform[0].Length - 2; col >= 0; col--)
        {
            for (var row = 0; row < platform.Count; row++)
            {
                if (platform[row][col] == 'O')
                {
                    var colToMoveOn = col + 1;
                    while (colToMoveOn < platform[0].Length && platform[row][colToMoveOn] == '.')
                    {
                        platform[row][colToMoveOn] = 'O';
                        platform[row][colToMoveOn - 1] = '.';

                        colToMoveOn++;
                    }
                }
            }
        }
    }

    public static void TiltPlatformToSouth(List<char[]> platform)
    {
        for (var row = platform.Count - 2; row >= 0; row--)
        {
            for (var col = 0; col < platform[row].Length; col++)
            {
                if (platform[row][col] == 'O')
                {
                    var rowToMoveOn = row + 1;
                    while (rowToMoveOn < platform.Count && platform[rowToMoveOn][col] == '.')
                    {
                        platform[rowToMoveOn][col] = 'O';
                        platform[rowToMoveOn - 1][col] = '.';

                        rowToMoveOn++;
                    }
                }
            }
        }
    }

    public static void TiltPlatformToNorth(List<char[]> platform)
    {
        for (var row = 1; row < platform.Count; row++)
        {
            for (var col = 0; col < platform[row].Length; col++)
            {
                if (platform[row][col] == 'O')
                {
                    var rowToMoveOn = row - 1;
                    while (rowToMoveOn >= 0 && platform[rowToMoveOn][col] == '.')
                    {
                        platform[rowToMoveOn][col] = 'O';
                        platform[rowToMoveOn + 1][col] = '.';

                        rowToMoveOn--;
                    }
                }
            }
        }
    }
}
