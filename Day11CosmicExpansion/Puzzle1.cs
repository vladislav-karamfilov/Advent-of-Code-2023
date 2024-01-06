namespace Day11CosmicExpansion;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/11
    public static void Solve()
    {
        var universe = PuzzleHelper.ReadUniverseInput();

        var expandedUniverse = PuzzleHelper.ExpandUniverse(universe);

        var galaxyCoordinates = PuzzleHelper.FindGalaxyCoordinates(expandedUniverse);

        var shortestPaths = PuzzleHelper.FindShortestPathsBetweenPairGalaxies(expandedUniverse, galaxyCoordinates);
        Console.WriteLine(shortestPaths.Sum());
    }
}
