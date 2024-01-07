namespace Day24NeverTellMeTheOdds;

using System;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/24#part2
    public static void Solve()
    {
        var hailstoneTrajectories = ReadHailstoneTrajectories();

        var rockVelocity = CalculateRockVelocity(hailstoneTrajectories, maxVelocity: 300);
        if (!rockVelocity.HasValue)
        {
            Console.WriteLine("Couldn't find rock velocity. Try increasing the 'maxVelocity' parameter");
            return;
        }

        var rockPosition = CalculateCollisionPoint(
            hailstoneTrajectories[0].InitialPosition,
            hailstoneTrajectories[0].InitialPosition + hailstoneTrajectories[0].Velocity + rockVelocity.Value,
            hailstoneTrajectories[1].InitialPosition,
            hailstoneTrajectories[1].InitialPosition + hailstoneTrajectories[1].Velocity + rockVelocity.Value);

        if (!rockPosition.HasValue)
        {
            Console.WriteLine("Couldn't find rock position.");
            return;
        }

        Console.WriteLine(rockPosition.Value.X + rockPosition.Value.Y + rockPosition.Value.Z);
    }

    private static Vector3D? CalculateRockVelocity(List<HailstoneTrajectory> hailstoneTrajectories, int maxVelocity)
    {
        var trajectoriesToCollide = Math.Min(5, hailstoneTrajectories.Count - 1);

        for (var x = -1 * maxVelocity; x <= maxVelocity; x++)
        {
            for (var y = -1 * maxVelocity; y <= maxVelocity; y++)
            {
                for (var z = -1 * maxVelocity; z <= maxVelocity; z++)
                {
                    var potentialRockVelocity = new Vector3D(x, y, z);
                    var isValidRockVelocity = true;

                    Vector3D? collisionPoint = null;
                    for (var i = 1; i <= trajectoriesToCollide; i++)
                    {
                        var currentCollisionPoint = CalculateCollisionPoint(
                            hailstoneTrajectories[0].InitialPosition,
                            hailstoneTrajectories[0].InitialPosition + hailstoneTrajectories[0].Velocity + potentialRockVelocity,
                            hailstoneTrajectories[i].InitialPosition,
                            hailstoneTrajectories[i].InitialPosition + hailstoneTrajectories[i].Velocity + potentialRockVelocity);

                        if (!currentCollisionPoint.HasValue)
                        {
                            isValidRockVelocity = false;
                            break;
                        }

                        if (!collisionPoint.HasValue)
                        {
                            collisionPoint = currentCollisionPoint;
                        }
                        else if (collisionPoint != currentCollisionPoint)
                        {
                            isValidRockVelocity = false;
                            break;
                        }
                    }

                    if (isValidRockVelocity)
                    {
                        return potentialRockVelocity;
                    }
                }
            }
        }

        return null;
    }

    // Approach: https://paulbourke.net/geometry/pointlineplane/ (The shortest line between two lines in 3D)
    private static Vector3D? CalculateCollisionPoint(Vector3D p1, Vector3D p2, Vector3D p3, Vector3D p4)
    {
        var p43 = p4 - p3;
        if (p43 == default)
        {
            return null;
        }

        var p21 = p2 - p1;
        if (p21 == default)
        {
            return null;
        }

        var p13 = p1 - p3;

        var d1343 = ((decimal)p13.X * p43.X) + ((decimal)p13.Y * p43.Y) + ((decimal)p13.Z * p43.Z);
        var d4321 = ((decimal)p43.X * p21.X) + ((decimal)p43.Y * p21.Y) + ((decimal)p43.Z * p21.Z);
        var d1321 = ((decimal)p13.X * p21.X) + ((decimal)p13.Y * p21.Y) + ((decimal)p13.Z * p21.Z);
        var d4343 = ((decimal)p43.X * p43.X) + ((decimal)p43.Y * p43.Y) + ((decimal)p43.Z * p43.Z);
        var d2121 = ((decimal)p21.X * p21.X) + ((decimal)p21.Y * p21.Y) + ((decimal)p21.Z * p21.Z);

        var denominator = (d2121 * d4343) - (d4321 * d4321);
        if (denominator == 0)
        {
            return null;
        }

        var numerator = (d1343 * d4321) - (d1321 * d4343);

        var mua = numerator / denominator;
        var mub = (d1343 + (d4321 * mua)) / d4343;

        var ax = Math.Round(p1.X + (mua * p21.X));
        var ay = Math.Round(p1.Y + (mua * p21.Y));
        var az = Math.Round(p1.Z + (mua * p21.Z));
        var bx = Math.Round(p3.X + (mub * p43.X));
        var by = Math.Round(p3.Y + (mub * p43.Y));
        var bz = Math.Round(p3.Z + (mub * p43.Z));
        if (ax != bx || ay != by || az != bz)
        {
            return null;
        }

        return new Vector3D(Convert.ToInt64(ax), Convert.ToInt64(ay), Convert.ToInt64(az));
    }

    private static List<HailstoneTrajectory> ReadHailstoneTrajectories()
    {
        Span<Range> coordinateRanges = stackalloc Range[3];

        var result = new List<HailstoneTrajectory>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var lineSpan = line.AsSpan();

            var initialPositionAndVelocityCoordinatesSeparatorIndex = lineSpan.IndexOf('@');

            var initialPositionCoordinatesSpan = lineSpan[..initialPositionAndVelocityCoordinatesSeparatorIndex];
            initialPositionCoordinatesSpan.Split(coordinateRanges, ',');
            var initialPosition = new Vector3D(
                long.Parse(initialPositionCoordinatesSpan[coordinateRanges[0]]),
                long.Parse(initialPositionCoordinatesSpan[coordinateRanges[1]]),
                long.Parse(initialPositionCoordinatesSpan[coordinateRanges[2]]));

            var velocityCoordinatesSpan = lineSpan[(initialPositionAndVelocityCoordinatesSeparatorIndex + 1)..];
            velocityCoordinatesSpan.Split(coordinateRanges, ',');
            var velocity = new Vector3D(
                long.Parse(velocityCoordinatesSpan[coordinateRanges[0]]),
                long.Parse(velocityCoordinatesSpan[coordinateRanges[1]]),
                long.Parse(velocityCoordinatesSpan[coordinateRanges[2]]));

            result.Add(new HailstoneTrajectory(initialPosition, velocity));
        }

        return result;
    }
}
