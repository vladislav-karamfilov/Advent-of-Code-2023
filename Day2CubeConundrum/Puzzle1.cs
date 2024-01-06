namespace Day2CubeConundrum;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/2
    public static void Solve()
    {
        var setSeparators = new char[] { ' ', ',' };

        var sum = 0L;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var gamePartEndIndex = line.IndexOf(':');
            var gamePart = line[..gamePartEndIndex];
            var gameId = int.Parse(gamePart.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

            var isPossibleGame = true;
            var setsOfCubesPart = line[(gamePartEndIndex + 1)..];
            var setsOfCubes = setsOfCubesPart.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var setLine in setsOfCubes)
            {
                var (redCubes, greenCubes, blueCubes) = ParseSetOfCubes(setLine, setSeparators);

                isPossibleGame = IsPossibleGame(redCubes, greenCubes, blueCubes);
                if (!isPossibleGame)
                {
                    break;
                }
            }

            if (isPossibleGame)
            {
                sum += gameId;
            }
        }

        Console.WriteLine(sum);
    }

    private static (int RedCubes, int GreenCubes, int BlueCubes) ParseSetOfCubes(string setLine, char[] setSeparators)
    {
        var redCubes = 0;
        var greenCubes = 0;
        var blueCubes = 0;

        var setParts = setLine.Split(setSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < setParts.Length; i += 2)
        {
            var count = int.Parse(setParts[i]);
            var color = setParts[i + 1];

            switch (color)
            {
                case "red":
                    redCubes = count;
                    break;
                case "green":
                    greenCubes = count;
                    break;
                case "blue":
                    blueCubes = count;
                    break;
            }
        }

        return (redCubes, greenCubes, blueCubes);
    }

    private static bool IsPossibleGame(int redCubes, int greenCubes, int blueCubes)
        => redCubes <= 12 && greenCubes <= 13 && blueCubes <= 14;
}
