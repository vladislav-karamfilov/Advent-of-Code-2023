namespace Day17ClumsyCrucible;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/17
    public static void Solve()
    {
        var cityMap = PuzzleHelper.ReadCityMap();

        var minHeatLoss = PuzzleHelper.CalculateMinHeatLossToMachinePartsFactory(
            cityMap,
            minBlocksInDirection: 1,
            maxBlocksInDirection: 3);

        Console.WriteLine(minHeatLoss);
    }
}
