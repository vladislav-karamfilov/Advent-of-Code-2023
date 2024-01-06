namespace Day11CosmicExpansion;

internal static class PuzzleHelper
{
    public static List<List<char>> ReadUniverseInput()
    {
        var universe = new List<List<char>>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            universe.Add([.. line]);
        }

        return universe;
    }

    public static List<List<char>> ExpandUniverse(List<List<char>> universe)
    {
        var expandedUniverse = new List<List<char>>();

        for (var i = universe.Count - 1; i >= 0; i--)
        {
            var row = universe[i];
            expandedUniverse.Add([.. row]);

            if (!row.Contains('#'))
            {
                expandedUniverse.Add([.. row]);
            }
        }

        expandedUniverse.Reverse();

        for (var i = expandedUniverse[0].Count - 1; i >= 0; i--)
        {
            var expand = expandedUniverse.All(row => row[i] == '.');
            if (expand)
            {
                foreach (var row in expandedUniverse)
                {
                    row.Insert(i, '.');
                }
            }
        }

        return expandedUniverse;
    }

    public static List<UniverseCoordinate> FindGalaxyCoordinates(List<List<char>> universe)
    {
        var result = new List<UniverseCoordinate>();

        for (var y = 0; y < universe.Count; y++)
        {
            for (var x = 0; x < universe[y].Count; x++)
            {
                if (universe[y][x] == '#')
                {
                    result.Add(new UniverseCoordinate(x, y));
                }
            }
        }

        return result;
    }

    public static List<int> FindShortestPathsBetweenPairGalaxies(
        List<List<char>> universe,
        List<UniverseCoordinate> galaxyCoordinates)
    {
        var result = new List<int>();
        var cachedShortestPaths = new Dictionary<(UniverseCoordinate, UniverseCoordinate), int>();

        for (var i = 0; i < galaxyCoordinates.Count; i++)
        {
            for (var j = i + 1; j < galaxyCoordinates.Count; j++)
            {
                // Manhattan distance algorithm (much faster)
                //var path =
                //    Math.Abs(galaxyCoordinates[i].X - galaxyCoordinates[j].X) +
                //    Math.Abs(galaxyCoordinates[i].Y - galaxyCoordinates[j].Y);

                result.Add(FindShortestPathBetweenGalaxies(
                    universe,
                    galaxyCoordinates[i],
                    galaxyCoordinates[j],
                    cachedShortestPaths));
            }
        }

        return result;
    }

    // Implementation of BFS: https://en.wikipedia.org/wiki/Breadth-first_search
    private static int FindShortestPathBetweenGalaxies(
        List<List<char>> universe,
        UniverseCoordinate sourceGalaxyCoordinates,
        UniverseCoordinate targetGalaxyCoordinates,
        Dictionary<(UniverseCoordinate, UniverseCoordinate), int> cachedShortestPaths)
    {
        if (cachedShortestPaths.TryGetValue((sourceGalaxyCoordinates, targetGalaxyCoordinates), out var cachedDistance))
        {
            return cachedDistance;
        }

        var universeRows = universe.Count;
        var universeCols = universe[0].Count;
        var visited = new bool[universeRows, universeCols];
        visited[sourceGalaxyCoordinates.Y, sourceGalaxyCoordinates.X] = true;

        var nextCoordinatesToTraverse = new Queue<DistanceToCoordinate>();
        nextCoordinatesToTraverse.Enqueue(new DistanceToCoordinate(sourceGalaxyCoordinates, 0));

        while (nextCoordinatesToTraverse.Count > 0)
        {
            var (coordinate, distance) = nextCoordinatesToTraverse.Dequeue();
            if (coordinate == targetGalaxyCoordinates)
            {
                return distance;
            }

            var x = coordinate.X;
            var y = coordinate.Y;

            if (coordinate != sourceGalaxyCoordinates && universe[y][x] == '#')
            {
                cachedShortestPaths.TryAdd((sourceGalaxyCoordinates, coordinate), distance);
            }

            // Left
            if (x > 0 && !visited[y, x - 1])
            {
                nextCoordinatesToTraverse.Enqueue(new DistanceToCoordinate(coordinate with { X = x - 1 }, distance + 1));
                visited[y, x - 1] = true;
            }

            // Right
            if (x < universeCols - 1 && !visited[y, x + 1])
            {
                nextCoordinatesToTraverse.Enqueue(new DistanceToCoordinate(coordinate with { X = x + 1 }, distance + 1));
                visited[y, x + 1] = true;
            }

            // Up
            if (y > 0 && !visited[y - 1, x])
            {
                nextCoordinatesToTraverse.Enqueue(new DistanceToCoordinate(coordinate with { Y = y - 1 }, distance + 1));
                visited[y - 1, x] = true;
            }

            // Down
            if (y < universeRows - 1 && !visited[y + 1, x])
            {
                nextCoordinatesToTraverse.Enqueue(new DistanceToCoordinate(coordinate with { Y = y + 1 }, distance + 1));
                visited[y + 1, x] = true;
            }
        }

        return -1;
    }
}
