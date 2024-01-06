namespace Day23ALongWalk;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/23
    public static void Solve()
    {
        var hikingMap = ReadHikingMap();

        var startTileLocation = new TileLocation(0, hikingMap[0].IndexOf('.'));
        var endTileLocation = new TileLocation(hikingMap.Count - 1, hikingMap[^1].IndexOf('.'));
        var visited = new bool[hikingMap.Count, hikingMap[0].Length];
        var maxHikeSteps = -1;

        FindMaxHikeSteps(
            hikingMap,
            startTileLocation,
            endTileLocation,
            currentStep: 0,
            visited,
            ref maxHikeSteps);

        Console.WriteLine(maxHikeSteps);
    }

    private static void FindMaxHikeSteps(
        List<string> hikingMap,
        TileLocation currentTileLocation,
        TileLocation endTileLocation,
        int currentStep,
        bool[,] visited,
        ref int maxHikeSteps)
    {
        if (currentTileLocation == endTileLocation)
        {
            if (currentStep > maxHikeSteps)
            {
                maxHikeSteps = currentStep;
            }

            return;
        }

        var (currentRow, currentCol) = currentTileLocation;
        visited[currentRow, currentCol] = true;

        if (hikingMap[currentRow][currentCol] == 'v')
        {
            if (!visited[currentRow + 1, currentCol])
            {
                var newTileLocation = currentTileLocation with { Row = currentRow + 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }
        }
        else if (hikingMap[currentRow][currentCol] == '^')
        {
            if (!visited[currentRow - 1, currentCol])
            {
                var newTileLocation = currentTileLocation with { Row = currentRow - 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }
        }
        else if (hikingMap[currentRow][currentCol] == '>')
        {
            if (!visited[currentRow, currentCol + 1])
            {
                var newTileLocation = currentTileLocation with { Col = currentCol + 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }
        }
        else if (hikingMap[currentRow][currentCol] == '<')
        {
            if (!visited[currentRow, currentCol - 1])
            {
                var newTileLocation = currentTileLocation with { Col = currentCol - 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }
        }
        else
        {
            // Up
            if (currentRow > 0 && hikingMap[currentRow - 1][currentCol] != '#' && !visited[currentRow - 1, currentCol])
            {
                var newTileLocation = currentTileLocation with { Row = currentRow - 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }

            // Down
            if (currentRow < hikingMap.Count - 1 && hikingMap[currentRow + 1][currentCol] != '#' && !visited[currentRow + 1, currentCol])
            {
                var newTileLocation = currentTileLocation with { Row = currentRow + 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }

            // Left
            if (currentCol > 0 && hikingMap[currentRow][currentCol - 1] != '#' && !visited[currentRow, currentCol - 1])
            {
                var newTileLocation = currentTileLocation with { Col = currentCol - 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }

            // Right
            if (currentCol < hikingMap[0].Length - 1 && hikingMap[currentRow][currentCol + 1] != '#' && !visited[currentRow, currentCol + 1])
            {
                var newTileLocation = currentTileLocation with { Col = currentCol + 1 };
                FindMaxHikeSteps(hikingMap, newTileLocation, endTileLocation, currentStep + 1, visited, ref maxHikeSteps);
            }
        }

        visited[currentRow, currentCol] = false;
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
