namespace Day21StepCounter;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/21
    public static void Solve()
    {
        var gardenMap = PuzzleHelper.ReadGardenMap();

        var reachableGardenPlotsInSpecifiedSteps = PuzzleHelper.CalculateReachableGardenPlotsInSpecifiedSteps(gardenMap, targetSteps: 64);

        Console.WriteLine(reachableGardenPlotsInSpecifiedSteps);
    }
}
