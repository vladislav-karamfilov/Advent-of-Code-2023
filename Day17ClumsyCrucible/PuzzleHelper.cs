namespace Day17ClumsyCrucible;

internal static class PuzzleHelper
{
    public static List<byte[]> ReadCityMap()
    {
        var result = new List<byte[]>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var cityBlockHeats = new byte[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                cityBlockHeats[i] = (byte)(line[i] - '0');
            }

            result.Add(cityBlockHeats);
        }

        return result;
    }

    // Implementation of A* search algorithm: https://en.wikipedia.org/wiki/A*_search_algorithm
    public static int CalculateMinHeatLossToMachinePartsFactory(List<byte[]> cityMap, int minBlocksInDirection, int maxBlocksInDirection)
    {
        var rightStartingBlock = new CityBlock(
            new CityBlockLocation(Row: 0, Col: 1),
            AccumulatedHeatLoss: cityMap[0][1],
            MoveDirection: MoveDirection.Right,
            CurrentBlocksInDirection: 1);

        var downStartingBlock = new CityBlock(
            new CityBlockLocation(Row: 0, Col: 0),
            AccumulatedHeatLoss: cityMap[1][0],
            MoveDirection: MoveDirection.Down,
            CurrentBlocksInDirection: 1);

        var destination = new CityBlockLocation(cityMap.Count - 1, cityMap[0].Length - 1);
        rightStartingBlock.SetEstimatedDistanceToDestination(destination);
        downStartingBlock.SetEstimatedDistanceToDestination(destination);

        var cityBlocksToVisit = new PriorityQueue<CityBlock, int>();
        cityBlocksToVisit.Enqueue(rightStartingBlock, rightStartingBlock.Score);
        cityBlocksToVisit.Enqueue(downStartingBlock, downStartingBlock.Score);

        var visited = new HashSet<(CityBlockLocation, MoveDirection, int)>();
        var neighborsInQueue = new HashSet<(CityBlockLocation, MoveDirection, int)>();

        while (cityBlocksToVisit.Count > 0)
        {
            var currentBlock = cityBlocksToVisit.Dequeue();
            if (currentBlock.Location == destination && currentBlock.CurrentBlocksInDirection >= minBlocksInDirection)
            {
                // PrintCityMapWithVisitedLocations(cityMap, currentBlock);
                return currentBlock.AccumulatedHeatLoss;
            }

            visited.Add((currentBlock.Location, currentBlock.MoveDirection, currentBlock.CurrentBlocksInDirection));

            var neighborBlocks = GetNeighborBlocks(cityMap, currentBlock, destination, minBlocksInDirection, maxBlocksInDirection);
            foreach (var neighborBlock in neighborBlocks)
            {
                var neighborInfo = (neighborBlock.Location, neighborBlock.MoveDirection, neighborBlock.CurrentBlocksInDirection);
                if (visited.Contains(neighborInfo) || neighborsInQueue.Contains(neighborInfo))
                {
                    continue;
                }

                neighborBlock.Parent = currentBlock;
                cityBlocksToVisit.Enqueue(neighborBlock, neighborBlock.Score);
                neighborsInQueue.Add(neighborInfo);
            }
        }

        return -1;
    }

    private static List<CityBlock> GetNeighborBlocks(
        List<byte[]> cityMap,
        CityBlock currentBlock,
        CityBlockLocation destination,
        int minBlocksInDirection,
        int maxBlocksInDirection)
    {
        var result = new List<CityBlock>();

        var (currentLocation, accumulatedHeatLoss, currentDirection, currentBlocksInDirection) = currentBlock;

        if (currentBlocksInDirection < maxBlocksInDirection)
        {
            if (currentDirection == MoveDirection.Right)
            {
                if (currentLocation.Col < cityMap[0].Length - 1)
                {
                    var nextLocation = currentLocation with { Col = currentLocation.Col + 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Right,
                        currentBlocksInDirection + 1));
                }
            }
            else if (currentDirection == MoveDirection.Left)
            {
                if (currentLocation.Col > 0)
                {
                    var nextLocation = currentLocation with { Col = currentLocation.Col - 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Left,
                        currentBlocksInDirection + 1));
                }
            }
            else if (currentDirection == MoveDirection.Up)
            {
                if (currentLocation.Row > 0)
                {
                    var nextLocation = currentLocation with { Row = currentLocation.Row - 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Up,
                        currentBlocksInDirection + 1));
                }
            }
            else if (currentDirection == MoveDirection.Down)
            {
                if (currentLocation.Row < cityMap.Count - 1)
                {
                    var nextLocation = currentLocation with { Row = currentLocation.Row + 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Down,
                        currentBlocksInDirection + 1));
                }
            }
        }

        if (currentBlocksInDirection >= minBlocksInDirection)
        {
            if (currentDirection == MoveDirection.Right || currentDirection == MoveDirection.Left)
            {
                if (currentLocation.Row < cityMap.Count - 1)
                {
                    var nextLocation = currentLocation with { Row = currentLocation.Row + 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Down,
                        CurrentBlocksInDirection: 1));
                }

                if (currentLocation.Row > 0)
                {
                    var nextLocation = currentLocation with { Row = currentLocation.Row - 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Up,
                        CurrentBlocksInDirection: 1));
                }
            }
            else // if (currentDirection == MoveDirection.Up || currentDirection == MoveDirection.Down)
            {
                if (currentLocation.Col < cityMap[0].Length - 1)
                {
                    var nextLocation = currentLocation with { Col = currentLocation.Col + 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Right,
                        CurrentBlocksInDirection: 1));
                }

                if (currentLocation.Col > 0)
                {
                    var nextLocation = currentLocation with { Col = currentLocation.Col - 1 };
                    var nextHeatLoss = cityMap[nextLocation.Row][nextLocation.Col];

                    result.Add(new CityBlock(
                        nextLocation,
                        accumulatedHeatLoss + nextHeatLoss,
                        MoveDirection.Left,
                        CurrentBlocksInDirection: 1));
                }
            }
        }

        result.ForEach(b => b.SetEstimatedDistanceToDestination(destination));

        return result;
    }

#pragma warning disable IDE0051 // Remove unused private members
    private static void PrintCityMapWithVisitedLocations(List<byte[]> cityMap, CityBlock destinationBlock)
#pragma warning restore IDE0051 // Remove unused private members
    {
        var visitedBlockLocations = new HashSet<CityBlockLocation>();
        var block = destinationBlock;
        while (block is not null)
        {
            visitedBlockLocations.Add(block.Location);
            block = block.Parent;
        }

        for (var row = 0; row < cityMap.Count; row++)
        {
            for (var col = 0; col < cityMap[0].Length; col++)
            {
                if (visitedBlockLocations.Contains(new CityBlockLocation(row, col)))
                {
                    Console.Write('-');
                }
                else
                {
                    Console.Write(cityMap[row][col]);
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}
