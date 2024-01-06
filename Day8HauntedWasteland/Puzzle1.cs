namespace Day8HauntedWasteland;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/8
    public static void Solve()
    {
        var (directionInstructions, networkNodes) = ReadInput();

        var steps = CalculateStepsToEndNode(directionInstructions, networkNodes);

        Console.WriteLine(steps);
    }

    private static int CalculateStepsToEndNode(string directionInstructions, Dictionary<string, DesertNetworkNode> networkNodes)
    {
        const char LeftDirection = 'L';
        const string StartNodeId = "AAA";
        const string EndNodeId = "ZZZ";

        var steps = 0;
        var currentDirectionIndex = 0;
        var currentNode = networkNodes[StartNodeId];
        while (currentNode.NodeId != EndNodeId)
        {
            var currentDirection = directionInstructions[currentDirectionIndex];
            var nextNodeId = currentDirection == LeftDirection ? currentNode.LeftPathNodeId : currentNode.RightPathNodeId;

            steps++;
            currentDirectionIndex = (currentDirectionIndex + 1) % directionInstructions.Length;
            currentNode = networkNodes[nextNodeId];
        }

        return steps;
    }

    private static (string DirectionInstructions, Dictionary<string, DesertNetworkNode> NetworkNodes) ReadInput()
    {
        var directionInstructions = string.Empty;
        var networkNodes = new Dictionary<string, DesertNetworkNode>();
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

            networkNodes[nodeId] = new DesertNetworkNode(nodeId, leftNodeToGo, rightNodeToGo);
        }

        return (directionInstructions, networkNodes);
    }
}
