namespace Day10PipeMaze;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/10
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

        var steps = loopCoordinates.Count / 2;
        Console.WriteLine(steps);
    }
}
