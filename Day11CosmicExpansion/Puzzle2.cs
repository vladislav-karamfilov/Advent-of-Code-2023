namespace Day11CosmicExpansion;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/11#part2
    public static void Solve()
    {
        var originalUniverse = PuzzleHelper.ReadUniverseInput();
        var originalUniverseGalaxyCoordinates = PuzzleHelper.FindGalaxyCoordinates(originalUniverse);
        var originalUniverseShortestPaths = PuzzleHelper.FindShortestPathsBetweenPairGalaxies(
            originalUniverse,
            originalUniverseGalaxyCoordinates);

        var expandedUniverse = PuzzleHelper.ExpandUniverse(originalUniverse);
        var expandedUniverseGalaxyCoordinates = PuzzleHelper.FindGalaxyCoordinates(expandedUniverse);
        var expandedUniverseShortestPaths = PuzzleHelper.FindShortestPathsBetweenPairGalaxies(
            expandedUniverse,
            expandedUniverseGalaxyCoordinates);

        var sum = ExtrapolateSumOfShortestPathsBetweenPairGalaxies(
            originalUniverseShortestPaths,
            expandedUniverseShortestPaths,
            extrapolationStep: 999_999);

        Console.WriteLine(sum);
    }

    private static long ExtrapolateSumOfShortestPathsBetweenPairGalaxies(
        List<int> originalUniverseShortestPaths,
        List<int> expandedUniverseShortestPaths,
        int extrapolationStep)
    {
        var sum = 0L;
        for (var i = 0; i < originalUniverseShortestPaths.Count; i++)
        {
            var originalUniversePath = originalUniverseShortestPaths[i];
            var expandedUniversePath = expandedUniverseShortestPaths[i];

            var diff = expandedUniversePath - originalUniversePath;
            sum += originalUniversePath + (diff * extrapolationStep);
        }

        return sum;
    }
}
