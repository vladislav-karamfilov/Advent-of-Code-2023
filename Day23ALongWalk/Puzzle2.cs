namespace Day23ALongWalk;

using System;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/23#part2
    public static void Solve()
    {
        var hikingMap = ReadHikingMap();

        var maxHikeSteps = CalculateMaxHikeSteps(hikingMap);

        Console.WriteLine(maxHikeSteps);
    }

    // Brute force DFS with stack instead of recursive one to prevent StackOverflowException (takes ~12 min to execute)
    private static int CalculateMaxHikeSteps(List<string> hikingMap)
    {
        var maxHikeSteps = -1;

        var visited = new bool[hikingMap.Count, hikingMap[0].Length];

        var startTileLocation = new TileLocation(1, hikingMap[0].IndexOf('.')); // Consider start to be on 2nd row
        var endTileLocation = new TileLocation(hikingMap.Count - 2, hikingMap[^1].IndexOf('.')); // Consider end to be on the row before the last

        var tilesToVisit = new Stack<(TileLocation, int, bool)>();
        tilesToVisit.Push((startTileLocation, 2, false)); // 2 because start and end steps are precalculated

        while (tilesToVisit.Count > 0)
        {
            var (currentTileLocation, currentStep, backtracking) = tilesToVisit.Pop();
            if (currentTileLocation == endTileLocation)
            {
                if (currentStep > maxHikeSteps)
                {
                    maxHikeSteps = currentStep;
                }

                continue;
            }

            var (currentRow, currentCol) = currentTileLocation;
            if (backtracking)
            {
                visited[currentRow, currentCol] = false;
                continue;
            }

            tilesToVisit.Push((currentTileLocation, currentStep, true));
            visited[currentRow, currentCol] = true;

            // Up
            if (currentRow > 1 && hikingMap[currentRow - 1][currentCol] != '#' && !visited[currentRow - 1, currentCol])
            {
                var newTileLocation = currentTileLocation with { Row = currentRow - 1 };
                tilesToVisit.Push((newTileLocation, currentStep + 1, false));
            }

            // Down
            if (currentRow < hikingMap.Count - 2 && hikingMap[currentRow + 1][currentCol] != '#' && !visited[currentRow + 1, currentCol])
            {
                var newTileLocation = currentTileLocation with { Row = currentRow + 1 };
                tilesToVisit.Push((newTileLocation, currentStep + 1, false));
            }

            // Left
            if (currentCol > 0 && hikingMap[currentRow][currentCol - 1] != '#' && !visited[currentRow, currentCol - 1])
            {
                var newTileLocation = currentTileLocation with { Col = currentCol - 1 };
                tilesToVisit.Push((newTileLocation, currentStep + 1, false));
            }

            // Right
            if (currentCol < hikingMap[0].Length - 1 && hikingMap[currentRow][currentCol + 1] != '#' && !visited[currentRow, currentCol + 1])
            {
                var newTileLocation = currentTileLocation with { Col = currentCol + 1 };
                tilesToVisit.Push((newTileLocation, currentStep + 1, false));
            }
        }

        return maxHikeSteps;
    }

    private static List<string> ReadHikingMap()
    {
        var hikingMap = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            hikingMap.Add(line);
        }

        return hikingMap;
    }
}
