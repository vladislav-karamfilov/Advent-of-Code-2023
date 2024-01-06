namespace Day14ParabolicReflectorDish;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/14
    public static void Solve()
    {
        var platform = PuzzleHelper.ReadPlatform();

        PuzzleHelper.TiltPlatformToNorth(platform);

        var sum = PuzzleHelper.CalculateLoadOnNorthSupportBeams(platform);
        Console.WriteLine(sum);
    }
}
