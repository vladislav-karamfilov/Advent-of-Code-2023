namespace Day18LavaductLagoon;

using System.Globalization;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/18#part2
    public static void Solve()
    {
        var digPlan = ReadDigPlan();

        var trenchVertexCoordinates = GetTrenchVertexCoordinates(digPlan);

        // Approach: Pick's Theorem (https://en.wikipedia.org/wiki/Pick%27s_theorem) (impl https://codeforces.com/blog/entry/65888?#comment-498656)
        var pointsOnTrenchBoundary = CalculatePointsOnTrenchBoundary(trenchVertexCoordinates);
        var trenchArea = CalculateTrenchArea(trenchVertexCoordinates);
        var pointsInsideTrench = trenchArea + 1 - (pointsOnTrenchBoundary / 2);

        var cubicMetersLava = pointsInsideTrench + pointsOnTrenchBoundary;
        Console.WriteLine(cubicMetersLava);
    }

    private static long CalculatePointsOnTrenchBoundary(List<LagoonCoordinate> trenchVertexCoordinates)
    {
        var result = (long)trenchVertexCoordinates.Count;

        for (var i = 0; i < trenchVertexCoordinates.Count; i++)
        {
            var dx = trenchVertexCoordinates[i].X - trenchVertexCoordinates[(i + 1) % trenchVertexCoordinates.Count].X;
            var dy = trenchVertexCoordinates[i].Y - trenchVertexCoordinates[(i + 1) % trenchVertexCoordinates.Count].Y;

            result += CalculateGreatestCommonDenominator(dx, dy) - 1;
        }

        return result;
    }

    private static long CalculateTrenchArea(List<LagoonCoordinate> trenchVertexCoordinates)
    {
        var result = 0L;

        for (var i = 2; i < trenchVertexCoordinates.Count; i++)
        {
            result += CrossCoordinates(
                new LagoonCoordinate(
                    trenchVertexCoordinates[i].X - trenchVertexCoordinates[0].X,
                    trenchVertexCoordinates[i].Y - trenchVertexCoordinates[0].Y),
                new LagoonCoordinate(
                    trenchVertexCoordinates[i - 1].X - trenchVertexCoordinates[0].X,
                    trenchVertexCoordinates[i - 1].Y - trenchVertexCoordinates[0].Y));
        }

        return Math.Abs(result / 2);
    }

    private static long CrossCoordinates(LagoonCoordinate coordinate1, LagoonCoordinate coordinate2)
        => (coordinate1.X * coordinate2.Y) - (coordinate2.X * coordinate1.Y);

    private static long CalculateGreatestCommonDenominator(long first, long second)
    {
        var a = Math.Abs(first);
        var b = Math.Abs(second);

        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }
            else
            {
                b %= a;
            }
        }

        return a | b;
    }

    private static List<LagoonCoordinate> GetTrenchVertexCoordinates(List<(char Direction, int MetersToDig)> digPlan)
    {
        var trenchVertexCoordinates = new List<LagoonCoordinate>(capacity: digPlan.Count + 1) { new(0, 0) };

        foreach (var (direction, metersToDig) in digPlan)
        {
            var currentCoordinate = trenchVertexCoordinates[^1];
            if (direction == 'R')
            {
                trenchVertexCoordinates.Add(currentCoordinate with { X = currentCoordinate.X + metersToDig });
            }
            else if (direction == 'L')
            {
                trenchVertexCoordinates.Add(currentCoordinate with { X = currentCoordinate.X - metersToDig });
            }
            else if (direction == 'U')
            {
                trenchVertexCoordinates.Add(currentCoordinate with { Y = currentCoordinate.Y - metersToDig });
            }
            else // if (direction == 'D')
            {
                trenchVertexCoordinates.Add(currentCoordinate with { Y = currentCoordinate.Y + metersToDig });
            }
        }

        return trenchVertexCoordinates;
    }

    private static List<(char Direction, int MetersToDig)> ReadDigPlan()
    {
        var directionsMap = new Dictionary<char, char> { ['0'] = 'R', ['1'] = 'D', ['2'] = 'L', ['3'] = 'U' };

        var result = new List<(char, int)>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var directionDigit = line[^2];
            var direction = directionsMap[directionDigit];

            var metersToDigHex = line.AsSpan()[(line.IndexOf('#') + 1)..^2];
            var metersToDig = int.Parse(metersToDigHex, NumberStyles.HexNumber);

            result.Add((direction, metersToDig));
        }

        return result;
    }
}
