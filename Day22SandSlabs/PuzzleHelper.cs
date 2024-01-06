namespace Day22SandSlabs;

internal static class PuzzleHelper
{
    public static (SortedDictionary<int, List<SandBrick>>, Dictionary<int, List<SandBrick>>) PerformSandBrickFallings(
        List<SandBrick> sandBricksSnapshot)
    {
        var orderedSandBricksByMinZ = new SortedDictionary<int, List<SandBrick>>();
        var sandBricksOnZBelow = new Dictionary<int, List<SandBrick>>();

        var orderedSandBrickGroupsByStartZ = sandBricksSnapshot.GroupBy(b => b.StartCoordinate.Z).OrderBy(bg => bg.Key);
        foreach (var sandBricksOnCurrentZ in orderedSandBrickGroupsByStartZ)
        {
            var currentZ = sandBricksOnCurrentZ.Key;
            if (currentZ <= 1)
            {
                orderedSandBricksByMinZ[currentZ] = [.. sandBricksOnCurrentZ];
                sandBricksOnZBelow[currentZ] = [.. sandBricksOnCurrentZ];
                continue;
            }

            foreach (var sandBrick in sandBricksOnCurrentZ)
            {
                for (var zBelow = currentZ - 1; zBelow >= 1; zBelow--)
                {
                    sandBricksOnZBelow.TryGetValue(zBelow, out var sandBricksBelow);
                    if (sandBricksBelow is not null && !CanSandBrickFall(sandBrick, sandBricksBelow))
                    {
                        break;
                    }

                    sandBrick.StartCoordinate = sandBrick.StartCoordinate with { Z = sandBrick.StartCoordinate.Z - 1 };
                    sandBrick.EndCoordinate = sandBrick.EndCoordinate with { Z = sandBrick.EndCoordinate.Z - 1 };
                }

                orderedSandBricksByMinZ.TryGetValue(sandBrick.StartCoordinate.Z, out var resultSandBricks);
                if (resultSandBricks is null)
                {
                    resultSandBricks = [];
                    orderedSandBricksByMinZ[sandBrick.StartCoordinate.Z] = resultSandBricks;
                }

                resultSandBricks.Add(sandBrick);

                if (!sandBricksOnZBelow.TryGetValue(sandBrick.EndCoordinate.Z, out var sandBricksOnZ))
                {
                    sandBricksOnZ = [];
                    sandBricksOnZBelow[sandBrick.EndCoordinate.Z] = sandBricksOnZ;
                }

                sandBricksOnZ.Add(sandBrick);
            }
        }

        orderedSandBricksByMinZ
            .Where(x => x.Value.Count == 0)
            .Select(x => x.Key)
            .ToList()
            .ForEach(z => orderedSandBricksByMinZ.Remove(z));

        return (orderedSandBricksByMinZ, sandBricksOnZBelow);
    }

    public static bool CanSandBrickFall(SandBrick sandBrick, List<SandBrick> sandBricksBelow, SandBrick? sandBrickBelowToSkip = null)
    {
        foreach (var sandBrickBelow in sandBricksBelow)
        {
            if (sandBrickBelow != sandBrickBelowToSkip && DoBricksIntersect(sandBrick, sandBrickBelow))
            {
                return false;
            }
        }

        return true;
    }

    public static bool DoBricksIntersect(SandBrick first, SandBrick second)
    {
        var o1 = DetermineOrientation(first.StartCoordinate, first.EndCoordinate, second.StartCoordinate);
        var o2 = DetermineOrientation(first.StartCoordinate, first.EndCoordinate, second.EndCoordinate);
        var o3 = DetermineOrientation(second.StartCoordinate, second.EndCoordinate, first.StartCoordinate);
        var o4 = DetermineOrientation(second.StartCoordinate, second.EndCoordinate, first.EndCoordinate);

        return (o1 != o2 && o3 != o4) ||
            (o1 == 0 && AreOnSegment(first.StartCoordinate, second.StartCoordinate, first.EndCoordinate)) ||
            (o2 == 0 && AreOnSegment(first.StartCoordinate, second.EndCoordinate, first.EndCoordinate)) ||
            (o3 == 0 && AreOnSegment(second.StartCoordinate, first.StartCoordinate, second.EndCoordinate)) ||
            (o4 == 0 && AreOnSegment(second.StartCoordinate, first.EndCoordinate, second.EndCoordinate));
    }

    public static List<SandBrick> ReadSandBricksSnapshot()
    {
        var result = new List<SandBrick>();

        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var rawCoordinates = line.Split('~');
            var startCoordinates = rawCoordinates[0].Split(',').Select(int.Parse).ToList();
            var endCoordinates = rawCoordinates[1].Split(',').Select(int.Parse).ToList();

            result.Add(new SandBrick
            {
                StartCoordinate = new Coordinate3D { X = startCoordinates[0], Y = startCoordinates[1], Z = startCoordinates[2] },
                EndCoordinate = new Coordinate3D { X = endCoordinates[0], Y = endCoordinates[1], Z = endCoordinates[2] },
            });
        }

        return result;
    }

    private static int DetermineOrientation(Coordinate3D p, Coordinate3D q, Coordinate3D r)
    {
        var val = ((q.Y - p.Y) * (r.X - q.X)) - ((q.X - p.X) * (r.Y - q.Y));
        if (val == 0)
        {
            return 0; // collinear
        }

        return val > 0 ? 1 : 2; // clock or counter-clock wise
    }

    private static bool AreOnSegment(Coordinate3D p, Coordinate3D q, Coordinate3D r)
        => q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
}
