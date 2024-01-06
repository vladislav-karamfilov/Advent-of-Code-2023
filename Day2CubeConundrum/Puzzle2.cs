namespace Day2CubeConundrum;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/2#part2
    public static void Solve()
    {
        var setSeparators = new char[] { ' ', ',' };

        long sum = 0;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var targetRedCubes = 0;
            var targetGreenCubes = 0;
            var targetBlueCubes = 0;

            var gamePartEndIndex = line.IndexOf(':');
            var setsOfCubesPart = line[(gamePartEndIndex + 1)..];
            var setsOfCubes = setsOfCubesPart.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var setLine in setsOfCubes)
            {
                ParseSetOfCubesAndUpdateTargetCubes(setLine, setSeparators, ref targetRedCubes, ref targetGreenCubes, ref targetBlueCubes);
            }

            var power = targetRedCubes * targetGreenCubes * targetBlueCubes;
            sum += power;
        }

        Console.WriteLine(sum);
    }

    private static void ParseSetOfCubesAndUpdateTargetCubes(
        string setLine,
        char[] setSeparators,
        ref int targetRedCubes,
        ref int targetGreenCubes,
        ref int targetBlueCubes)
    {
        var setParts = setLine.Split(setSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < setParts.Length; i += 2)
        {
            var count = int.Parse(setParts[i]);
            var color = setParts[i + 1];

            switch (color)
            {
                case "red" when targetRedCubes < count:
                    targetRedCubes = count;
                    break;
                case "green" when targetGreenCubes < count:
                    targetGreenCubes = count;
                    break;
                case "blue" when targetBlueCubes < count:
                    targetBlueCubes = count;
                    break;
            }
        }
    }
}
