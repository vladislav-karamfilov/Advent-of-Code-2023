namespace Day10PipeMaze;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/10#part2
    public static void Solve()
    {
        var maze = PuzzleHelper.ReadMaze();

        var startCoordinate = PuzzleHelper.FindStartCoordinate(maze);

        var loopCoordinates = PuzzleHelper.FindLoopCoordinates(maze, startCoordinate);
        if (loopCoordinates is null)
        {
            Console.WriteLine("No loop in maze!");
            return;
        }

        var tilesEnclosedByLoop = CalculateTilesEnclosedByLoop(maze, loopCoordinates);
        Console.WriteLine(tilesEnclosedByLoop);
    }

    private static int CalculateTilesEnclosedByLoop(List<string> maze, List<MazeCoordinate> loopCoordinates)
    {
        var result = 0;

        var minLoopX = loopCoordinates.Min(c => c.X);
        var maxLoopX = loopCoordinates.Max(c => c.X);
        var minLoopY = loopCoordinates.Min(c => c.Y);
        var maxLoopY = loopCoordinates.Max(c => c.Y);

        for (var y = 1; y < maze.Count - 1; y++)
        {
            for (var x = 1; x < maze[0].Length - 1; x++)
            {
                var coordinate = new MazeCoordinate(x, y);
                if (!loopCoordinates.Contains(coordinate) &&
                    IsCoordinateEnclosedByLoop(coordinate, loopCoordinates, minLoopX, maxLoopX, minLoopY, maxLoopY))
                {
                    result++;
                }
            }
        }

        return result;
    }

    private static bool IsCoordinateEnclosedByLoop(
        MazeCoordinate coordinate,
        List<MazeCoordinate> loopCoordinates,
        int minLoopX,
        int maxLoopX,
        int minLoopY,
        int maxLoopY)
    {
        if (coordinate.X < minLoopX || coordinate.X > maxLoopX || coordinate.Y < minLoopY || coordinate.Y > maxLoopY)
        {
            return false;
        }

        // Approach: Point Inclusion in Polygon (https://wrfranklin.org/Research/Short_Notes/pnpoly.html)
        var inside = false;
        for (int i = 0, j = loopCoordinates.Count - 1; i < loopCoordinates.Count; j = i++)
        {
            if ((loopCoordinates[i].Y > coordinate.Y) != (loopCoordinates[j].Y > coordinate.Y) &&
                 coordinate.X < ((loopCoordinates[j].X - loopCoordinates[i].X) * (coordinate.Y - loopCoordinates[i].Y) / (loopCoordinates[j].Y - loopCoordinates[i].Y)) + loopCoordinates[i].X)
            {
                inside = !inside;
            }
        }

        return inside;
    }
}
