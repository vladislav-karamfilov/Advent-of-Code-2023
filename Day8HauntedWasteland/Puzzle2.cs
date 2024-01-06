namespace Day8HauntedWasteland;

using System;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/8#part2
    public static void Solve()
    {
        var (directionInstructions, networkNodes) = ReadInput();

        var steps = CalculateStepsToEndNodes(directionInstructions, networkNodes);

        Console.WriteLine(steps);
    }

    private static long CalculateStepsToEndNodes(string directionInstructions, List<DesertNetworkNode> networkNodes)
    {
        const char LeftDirection = 'L';
        const char StartChar = 'A';
        const char EndChar = 'Z';

        var startNodeIndices = new List<int>();
        var leftPathNodeIndices = new int[networkNodes.Count];
        var rightPathNodeIndices = new int[networkNodes.Count];
        for (var i = 0; i < networkNodes.Count; i++)
        {
            var node = networkNodes[i];
            leftPathNodeIndices[i] = networkNodes.FindIndex(n => n.NodeId == node.LeftPathNodeId);
            rightPathNodeIndices[i] = networkNodes.FindIndex(n => n.NodeId == node.RightPathNodeId);

            if (node.NodeId.EndsWith(StartChar))
            {
                startNodeIndices.Add(i);
            }
        }

        var stepsForPaths = new long[startNodeIndices.Count];
        for (var i = 0; i < startNodeIndices.Count; i++)
        {
            var steps = 0L;
            var currentDirectionIndex = 0;
            var currentNodeIndex = startNodeIndices[i];

            while (!networkNodes[currentNodeIndex].NodeId.EndsWith(EndChar))
            {
                var currentDirection = directionInstructions[currentDirectionIndex];
                var nextNodeIndex = currentDirection == LeftDirection
                    ? leftPathNodeIndices[currentNodeIndex]
                    : rightPathNodeIndices[currentNodeIndex];

                steps++;
                currentDirectionIndex = (currentDirectionIndex + 1) % directionInstructions.Length;
                currentNodeIndex = nextNodeIndex;
            }

            stepsForPaths[i] = steps;
        }

        return CalculateLeastCommonMultiple(stepsForPaths);
    }

    private static (string DirectionInstructions, List<DesertNetworkNode> NetworkNodes) ReadInput()
    {
        var directionInstructions = string.Empty;
        var networkNodes = new List<DesertNetworkNode>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                if (networkNodes.Count != 0)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            if (string.IsNullOrWhiteSpace(directionInstructions))
            {
                directionInstructions = line;
                continue;
            }

            var nodeIdEndIndex = line.IndexOf(' ');
            var nodeId = line[..nodeIdEndIndex];

            var leftNodeToGoStartIndex = nodeIdEndIndex + 4;
            var leftNodeToGoEndIndex = line.IndexOf(',');
            var leftNodeToGo = line[leftNodeToGoStartIndex..leftNodeToGoEndIndex];

            var rightNodeToGoStartIndex = leftNodeToGoEndIndex + 2;
            var rightNodeToGoEndIndex = line.Length - 1;
            var rightNodeToGo = line[rightNodeToGoStartIndex..rightNodeToGoEndIndex];

            networkNodes.Add(new DesertNetworkNode(nodeId, leftNodeToGo, rightNodeToGo));
        }

        return (directionInstructions, networkNodes);
    }

    private static long CalculateLeastCommonMultiple(long[] numbers)
    {
        var result = numbers switch
        {
            [var first, var second] => CalculateLeastCommonMultiple(first, second),
            [var first, .. var rest] => CalculateLeastCommonMultiple(first, CalculateLeastCommonMultiple(rest)),
            _ => throw new InvalidOperationException($"Cannot calculate LCM of {numbers.Length} number(s)")
        };

        return result;
    }

    private static long CalculateLeastCommonMultiple(long a, long b)
        => a * (b / CalculateGreatestCommonDenominator(a, b));

    private static long CalculateGreatestCommonDenominator(long a, long b)
    {
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
}
