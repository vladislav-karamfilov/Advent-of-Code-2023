namespace Day22SandSlabs;

using System.Collections.Generic;
using System.Linq;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/22
    public static void Solve()
    {
        var sandBricksSnapshot = PuzzleHelper.ReadSandBricksSnapshot();

        var (orderedSandBricksByMinZ, sandBricksOnZBelow) = PuzzleHelper.PerformSandBrickFallings(sandBricksSnapshot);

        var safelyDisintegrableBricks = CountSafelyDisintegrableSandBricks(orderedSandBricksByMinZ, sandBricksOnZBelow);

        Console.WriteLine(safelyDisintegrableBricks);
    }

    private static int CountSafelyDisintegrableSandBricks(
        SortedDictionary<int, List<SandBrick>> orderedBricksByMinZ,
        Dictionary<int, List<SandBrick>> sandBricksOnZBelow)
    {
        var (topZ, topSandBricks) = orderedBricksByMinZ.LastOrDefault();
        if (topSandBricks is null)
        {
            return 0;
        }

        var safelyDisintegrableBricks = topSandBricks.Count; // All bricks on top are disintegrable

        foreach (var (currentZ, sandBricksOnCurrentZ) in orderedBricksByMinZ)
        {
            if (currentZ <= 1)
            {
                continue;
            }

            var sandBricksBelow = sandBricksOnZBelow[currentZ - 1];
            foreach (var sandBrickToPotentiallyDisintegrate in sandBricksBelow)
            {
                var willFall = false;
                foreach (var sandBrick in sandBricksOnCurrentZ)
                {
                    if (PuzzleHelper.CanSandBrickFall(sandBrick, sandBricksBelow, sandBrickToPotentiallyDisintegrate))
                    {
                        willFall = true;
                        break;
                    }
                }

                if (!willFall)
                {
                    safelyDisintegrableBricks++;
                }
            }
        }

        // Finally, add the vertical bricks with end Z > the top row Z
        safelyDisintegrableBricks += sandBricksOnZBelow
            .Where(x => x.Key > topZ)
            .SelectMany(x => x.Value.Where(b => !topSandBricks.Contains(b)))
            .Count();

        return safelyDisintegrableBricks;
    }
}
