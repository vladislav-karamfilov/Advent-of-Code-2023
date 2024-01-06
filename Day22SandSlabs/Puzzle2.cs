namespace Day22SandSlabs;

using System;
using System.Linq;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/22#part2
    public static void Solve()
    {
        var sandBricksSnapshot = PuzzleHelper.ReadSandBricksSnapshot();

        var (orderedSandBricksByMinZ, _) = PuzzleHelper.PerformSandBrickFallings(sandBricksSnapshot);

        var sumOfBricksToFallOnDisintegration = CalculateSumOfBricksToFallOnDisintegration(orderedSandBricksByMinZ);

        Console.WriteLine(sumOfBricksToFallOnDisintegration);
    }

    private static int CalculateSumOfBricksToFallOnDisintegration(SortedDictionary<int, List<SandBrick>> orderedBricksByMinZ)
    {
        var maxZ = orderedBricksByMinZ.LastOrDefault().Key;

        var supportingBricksBelowForBrickMap = new Dictionary<SandBrick, List<SandBrick>>();
        for (var z = 1; z <= maxZ; z++)
        {
            var sandBricksOnCurrentZ = orderedBricksByMinZ[z];
            foreach (var sandBrick in sandBricksOnCurrentZ)
            {
                orderedBricksByMinZ.TryGetValue(sandBrick.EndCoordinate.Z + 1, out var sandBricksAbove);
                foreach (var sandBrickAbove in sandBricksAbove ?? Enumerable.Empty<SandBrick>())
                {
                    if (PuzzleHelper.DoBricksIntersect(sandBrick, sandBrickAbove))
                    {
                        if (!supportingBricksBelowForBrickMap.TryGetValue(sandBrickAbove, out var supportingSandBricksBelow))
                        {
                            supportingSandBricksBelow = [];
                            supportingBricksBelowForBrickMap[sandBrickAbove] = supportingSandBricksBelow;
                        }

                        supportingSandBricksBelow.Add(sandBrick);
                    }
                }
            }
        }

        var supportedBricksAboveByBrickMap = orderedBricksByMinZ
            .SelectMany(x => x.Value)
            .ToDictionary(b => b, b => supportingBricksBelowForBrickMap.Where(x => x.Value.Contains(b)).Select(x => x.Key).ToList());

        var sumOfBricksToFallOnDisintegration = supportedBricksAboveByBrickMap.Keys
            .Select(b => CountBricksToFallOnBrickDisintegration(b, supportedBricksAboveByBrickMap, supportingBricksBelowForBrickMap))
            .Sum();

        return sumOfBricksToFallOnDisintegration;
    }

    private static int CountBricksToFallOnBrickDisintegration(
        SandBrick sandBrick,
        Dictionary<SandBrick, List<SandBrick>> supportedBricksByBrickMap,
        Dictionary<SandBrick, List<SandBrick>> supportingBricksBelowForBrickMap)
    {
        var bricksToFall = new HashSet<SandBrick>();

        FindBricksToFall(sandBrick, supportedBricksByBrickMap, supportingBricksBelowForBrickMap, bricksToFall);

        bricksToFall.Remove(sandBrick);
        return bricksToFall.Count;
    }

    private static void FindBricksToFall(
        SandBrick sandBrick,
        Dictionary<SandBrick, List<SandBrick>> supportedBricksByBrickMap,
        Dictionary<SandBrick, List<SandBrick>> supportingBricksBelowForBrickMap,
        HashSet<SandBrick> bricksToFall)
    {
        if (!supportedBricksByBrickMap.TryGetValue(sandBrick, out var supportedBricks) || supportedBricks.Count == 0)
        {
            return;
        }

        bricksToFall.Add(sandBrick);

        var nextBricksToFall = new List<SandBrick>();
        foreach (var supportedBrick in supportedBricks)
        {
            if (supportingBricksBelowForBrickMap[supportedBrick].All(bricksToFall.Contains))
            {
                nextBricksToFall.Add(supportedBrick);
                bricksToFall.Add(supportedBrick);
            }
        }

        foreach (var nextBrickToFall in nextBricksToFall)
        {
            FindBricksToFall(nextBrickToFall, supportedBricksByBrickMap, supportingBricksBelowForBrickMap, bricksToFall);
        }
    }
}
