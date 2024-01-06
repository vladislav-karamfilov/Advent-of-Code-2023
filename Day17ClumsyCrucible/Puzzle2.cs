namespace Day17ClumsyCrucible;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/17#part2
    public static void Solve()
    {
        var cityMap = PuzzleHelper.ReadCityMap();

        var minHeatLoss = PuzzleHelper.CalculateMinHeatLossToMachinePartsFactory(
            cityMap,
            minBlocksInDirection: 4,
            maxBlocksInDirection: 10);

        Console.WriteLine(minHeatLoss);
    }
}
