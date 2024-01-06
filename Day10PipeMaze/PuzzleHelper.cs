namespace Day10PipeMaze;

internal static class PuzzleHelper
{
    public static List<string> ReadMaze()
    {
        var maze = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            maze.Add(line);
        }

        return maze;
    }

    public static MazeCoordinate FindStartCoordinate(List<string> maze)
    {
        var startY = maze.FindIndex(x => x.Contains('S'));
        var startX = maze[startY].IndexOf('S');

        return new MazeCoordinate(startX, startY);
    }

    public static List<MazeCoordinate>? FindLoopCoordinates(List<string> maze, MazeCoordinate startCoordinate)
    {
        if (maze.Count == 0 || maze[0].Length == 0)
        {
            return null;
        }

        if (startCoordinate.Y > 0 &&
            TryFindLoopCoordinates(
                maze,
                startCoordinate with { Y = startCoordinate.Y - 1 },
                MoveDirection.North,
                out var loopCoordinates))
        {
            return loopCoordinates;
        }

        if (startCoordinate.Y < maze.Count - 1 &&
            TryFindLoopCoordinates(
                maze,
                startCoordinate with { Y = startCoordinate.Y + 1 },
                MoveDirection.South,
                out loopCoordinates))
        {
            return loopCoordinates;
        }

        if (startCoordinate.X > 0 &&
            TryFindLoopCoordinates(
                maze,
                startCoordinate with { X = startCoordinate.X - 1 },
                MoveDirection.West,
                out loopCoordinates))
        {
            return loopCoordinates;
        }

        if (startCoordinate.X < maze[0].Length - 1 &&
            TryFindLoopCoordinates(
                maze,
                startCoordinate with { X = startCoordinate.X + 1 },
                MoveDirection.East,
                out loopCoordinates))
        {
            return loopCoordinates;
        }

        return null;
    }

    private static bool TryFindLoopCoordinates(
        List<string> maze,
        MazeCoordinate startCoordinate,
        MoveDirection direction,
        out List<MazeCoordinate>? loopCoordinates)
    {
        loopCoordinates = [startCoordinate];
        var nextCoordinate = startCoordinate;
        var nextDirection = direction;
        while (true)
        {
            (nextCoordinate, nextDirection) = CalculateNextCoordinateAndDirection(maze, nextCoordinate!, nextDirection);
            if (nextCoordinate is null)
            {
                loopCoordinates = null;
                return false;
            }

            if (maze[nextCoordinate.Y][nextCoordinate.X] == 'S')
            {
                loopCoordinates.Insert(0, nextCoordinate);
                return true;
            }

            loopCoordinates.Add(nextCoordinate);
        }
    }

    private static (MazeCoordinate? NextCoordinate, MoveDirection NextDirection) CalculateNextCoordinateAndDirection(
        List<string> maze,
        MazeCoordinate coordinate,
        MoveDirection direction)
    {
        var tile = maze[coordinate.Y][coordinate.X];
        if (tile == '-')
        {
            if (direction == MoveDirection.East)
            {
                var nextCoordinate = coordinate with { X = coordinate.X + 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, direction)
                    : (null, direction);
            }

            if (direction == MoveDirection.West)
            {
                var nextCoordinate = coordinate with { X = coordinate.X - 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, direction)
                    : (null, direction);
            }
        }

        if (tile == '|')
        {
            if (direction == MoveDirection.North)
            {
                var nextCoordinate = coordinate with { Y = coordinate.Y - 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, direction)
                    : (null, direction);
            }

            if (direction == MoveDirection.South)
            {
                var nextCoordinate = coordinate with { Y = coordinate.Y + 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, direction)
                    : (null, direction);
            }
        }

        if (tile == 'L')
        {
            if (direction == MoveDirection.South)
            {
                var nextCoordinate = coordinate with { X = coordinate.X + 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.East)
                    : (null, direction);
            }

            if (direction == MoveDirection.West)
            {
                var nextCoordinate = coordinate with { Y = coordinate.Y - 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.North)
                    : (null, direction);
            }
        }

        if (tile == 'J')
        {
            if (direction == MoveDirection.South)
            {
                var nextCoordinate = coordinate with { X = coordinate.X - 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.West)
                    : (null, direction);
            }

            if (direction == MoveDirection.East)
            {
                var nextCoordinate = coordinate with { Y = coordinate.Y - 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.North)
                    : (null, direction);
            }
        }

        if (tile == '7')
        {
            if (direction == MoveDirection.North)
            {
                var nextCoordinate = coordinate with { X = coordinate.X - 1 };

                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.West)
                    : (null, direction);
            }

            if (direction == MoveDirection.East)
            {
                var nextCoordinate = coordinate with { Y = coordinate.Y + 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.South)
                    : (null, direction);
            }
        }

        if (tile == 'F')
        {
            if (direction == MoveDirection.North)
            {
                var nextCoordinate = coordinate with { X = coordinate.X + 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.East)
                    : (null, direction);
            }

            if (direction == MoveDirection.West)
            {
                var nextCoordinate = coordinate with { Y = coordinate.Y + 1 };
                return IsInMaze(maze, nextCoordinate)
                    ? (nextCoordinate, MoveDirection.South)
                    : (null, direction);
            }
        }

        return (null, direction);
    }

    private static bool IsInMaze(List<string> maze, MazeCoordinate coordinate)
        => coordinate.X >= 0 && coordinate.X < maze[0].Length && coordinate.Y >= 0 && coordinate.Y < maze.Count;
}
