namespace Day18LavaductLagoon;

using System.Collections.Frozen;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/18
    public static void Solve()
    {
        var digPlan = ReadDigPlan();

        var trenchEdgeCoordinates = GetTrenchEdgeCoordinates(digPlan);

        var cubicMetersLava = CalculateTrenchCubicMetersLava(trenchEdgeCoordinates);

        Console.WriteLine(cubicMetersLava);
    }

    private static int CalculateTrenchCubicMetersLava(List<LagoonCoordinate> trenchEdgeCoordinates)
    {
        var result = 0;

        var minX = trenchEdgeCoordinates.Min(c => c.X);
        var maxX = trenchEdgeCoordinates.Max(c => c.X);
        var minY = trenchEdgeCoordinates.Min(c => c.Y);
        var maxY = trenchEdgeCoordinates.Max(c => c.Y);

        var trenchEdgeCoordinatesSet = trenchEdgeCoordinates.ToFrozenSet();

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var coordinate = new LagoonCoordinate(x, y);
                if (trenchEdgeCoordinatesSet.Contains(coordinate) || IsInsideTrench(coordinate, trenchEdgeCoordinates))
                {
                    result++;
                }
            }
        }

        return result;
    }

    private static bool IsInsideTrench(LagoonCoordinate coordinate, List<LagoonCoordinate> trenchEdgeCoordinates)
    {
        // https://wrfranklin.org/Research/Short_Notes/pnpoly.html
        var inside = false;
        for (int i = 0, j = trenchEdgeCoordinates.Count - 1; i < trenchEdgeCoordinates.Count; j = i++)
        {
            if ((trenchEdgeCoordinates[i].Y > coordinate.Y) != (trenchEdgeCoordinates[j].Y > coordinate.Y) &&
                 coordinate.X < ((trenchEdgeCoordinates[j].X - trenchEdgeCoordinates[i].X) * (coordinate.Y - trenchEdgeCoordinates[i].Y) / (trenchEdgeCoordinates[j].Y - trenchEdgeCoordinates[i].Y)) + trenchEdgeCoordinates[i].X)
            {
                inside = !inside;
            }
        }

        return inside;
    }

    private static List<LagoonCoordinate> GetTrenchEdgeCoordinates(List<(char Direction, int MetersToDig)> digPlan)
    {
        var trenchEdgeCoordinates = new List<LagoonCoordinate>(capacity: digPlan.Count) // Will increase size less often
        {
            new(0, 0)
        };

        foreach (var (direction, metersToDig) in digPlan)
        {
            var currentCoordinate = trenchEdgeCoordinates[^1];
            if (direction == 'R')
            {
                for (var i = 1; i <= metersToDig; i++)
                {
                    trenchEdgeCoordinates.Add(currentCoordinate with { X = currentCoordinate.X + i });
                }
            }
            else if (direction == 'L')
            {
                for (var i = 1; i <= metersToDig; i++)
                {
                    trenchEdgeCoordinates.Add(currentCoordinate with { X = currentCoordinate.X - i });
                }
            }
            else if (direction == 'U')
            {
                for (var i = 1; i <= metersToDig; i++)
                {
                    trenchEdgeCoordinates.Add(currentCoordinate with { Y = currentCoordinate.Y - i });
                }
            }
            else // if (direction == 'D')
            {
                for (var i = 1; i <= metersToDig; i++)
                {
                    trenchEdgeCoordinates.Add(currentCoordinate with { Y = currentCoordinate.Y + i });
                }
            }
        }

        return trenchEdgeCoordinates;
    }

    private static List<(char Direction, int MetersToDig)> ReadDigPlan()
    {
        var result = new List<(char, int)>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var direction = line[0];
            var metersToDig = int.Parse(line.AsSpan()[1..line.IndexOf(' ', 2)]);

            result.Add((direction, metersToDig));
        }

        return result;
    }
}
