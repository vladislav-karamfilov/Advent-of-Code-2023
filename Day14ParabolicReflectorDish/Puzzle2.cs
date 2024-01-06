namespace Day14ParabolicReflectorDish;

using System;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/14#part2
    public static void Solve()
    {
        var platform = PuzzleHelper.ReadPlatform();

        SpinPlatform(platform, totalTimesToSpin: 1_000_000_000);

        var sum = PuzzleHelper.CalculateLoadOnNorthSupportBeams(platform);
        Console.WriteLine(sum);
    }

    private static void SpinPlatform(List<char[]> platform, int totalTimesToSpin)
    {
        var spinCyclesDone = new List<string>();
        var repeatStartIndex = -1;
        for (var i = 0; i < totalTimesToSpin; i++)
        {
            SpinPlatformOnce(platform);

            var currentCycleResult = string.Join('\n', platform.Select(x => new string(x)));
            repeatStartIndex = spinCyclesDone.IndexOf(currentCycleResult);
            if (repeatStartIndex < 0)
            {
                spinCyclesDone.Add(currentCycleResult);
            }
            else
            {
                break;
            }
        }

        var repeatCycles = spinCyclesDone.Count - repeatStartIndex;

        var spinsLeftToDo = totalTimesToSpin - spinCyclesDone.Count - 1;
        spinsLeftToDo -= spinsLeftToDo / repeatCycles * repeatCycles;

        for (var j = 0; j < spinsLeftToDo; j++)
        {
            SpinPlatformOnce(platform);
        }
    }

    private static void SpinPlatformOnce(List<char[]> platform)
    {
        PuzzleHelper.TiltPlatformToNorth(platform);
        PuzzleHelper.TiltPlatformToWest(platform);
        PuzzleHelper.TiltPlatformToSouth(platform);
        PuzzleHelper.TiltPlatformToEast(platform);
    }
}
