namespace Day24NeverTellMeTheOdds;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/24
    public static void Solve()
    {
        var hailstoneTrajectories = ReadHailstoneTrajectories();

        var count = CountIntersectingHailstoneTrajectoriesInTestArea(
            hailstoneTrajectories,
            testAreaStart: 200_000_000_000_000, // 7
            testAreaEnd: 400_000_000_000_000); // 27

        Console.WriteLine(count);
    }

    private static int CountIntersectingHailstoneTrajectoriesInTestArea(
        List<HailstoneTrajectory> hailstoneTrajectories,
        double testAreaStart,
        double testAreaEnd)
    {
        var count = 0;

        for (var i = 0; i < hailstoneTrajectories.Count - 1; i++)
        {
            for (var j = i + 1; j < hailstoneTrajectories.Count; j++)
            {
                if (DoHailstoneTrajectoriesIntersectInTestArea(
                    hailstoneTrajectories[i],
                    hailstoneTrajectories[j],
                    testAreaStart,
                    testAreaEnd))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static bool DoHailstoneTrajectoriesIntersectInTestArea(
        HailstoneTrajectory firstHailstoneTrajectory,
        HailstoneTrajectory secondHailstoneTrajectory,
        double testAreaStart,
        double testAreaEnd)
    {
        // Approach: https://www.topcoder.com/thrive/articles/Geometry%20Concepts%20part%202:%20%20Line%20Intersection%20and%20its%20Applications#LineLineIntersection
        var (a1, b1, c1) = CalculateLineCoefficientsForHailstoneTrajectory(firstHailstoneTrajectory);
        var (a2, b2, c2) = CalculateLineCoefficientsForHailstoneTrajectory(secondHailstoneTrajectory);

        var determinant = (a1 * b2) - (a2 * b1);
        if (determinant == 0)
        {
            return false; // Parallel lines never intersect
        }

        var intersectionPointX = ((b2 * c1) - (b1 * c2)) / determinant;
        var intersectionPointY = ((a1 * c2) - (a2 * c1)) / determinant;
        if (testAreaStart > intersectionPointX ||
            testAreaStart > intersectionPointY ||
            testAreaEnd < intersectionPointX ||
            testAreaEnd < intersectionPointY)
        {
            return false; // Intersection point is outside test area
        }

        if (((intersectionPointX - firstHailstoneTrajectory.InitialPosition.X) / firstHailstoneTrajectory.Velocity.X) < 0 ||
            ((intersectionPointY - firstHailstoneTrajectory.InitialPosition.Y) / firstHailstoneTrajectory.Velocity.Y) < 0)
        {
            return false; // Intersection point is in test area but in the past for 1st hailstone trajectory
        }

        if (((intersectionPointX - secondHailstoneTrajectory.InitialPosition.X) / secondHailstoneTrajectory.Velocity.X) < 0 ||
            ((intersectionPointY - secondHailstoneTrajectory.InitialPosition.Y) / secondHailstoneTrajectory.Velocity.Y) < 0)
        {
            return false; // Intersection point is in test area but in the past for 2nd hailstone trajectory
        }

        return true;
    }

    private static (double A, double B, double C) CalculateLineCoefficientsForHailstoneTrajectory(HailstoneTrajectory trajectory)
    {
        var x1 = trajectory.InitialPosition.X;
        var y1 = trajectory.InitialPosition.Y;

        var x2 = trajectory.InitialPosition.X + trajectory.Velocity.X;
        var y2 = trajectory.InitialPosition.Y + trajectory.Velocity.Y;

        double a = y2 - y1;
        double b = x1 - x2;
        var c = (a * x1) + (b * y1);

        return (a, b, c);
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
