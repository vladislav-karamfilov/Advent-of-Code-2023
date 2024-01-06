namespace Day25Snowverload;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/25
    public static void Solve()
    {
        var wiredComponentsGraph = ReadWiredComponentsGraph();

        var (componentsInGroup1, componentsInGroup2) = CountComponentsInTwoComponentGroupsAfterTargetCuts(
            wiredComponentsGraph,
            targetCutsCount: 3);

        Console.WriteLine(componentsInGroup1 * componentsInGroup2);
    }

    // Approach: Karger's algorithm (https://en.wikipedia.org/wiki/Karger%27s_algorithm)
    public static (int ComponentsInGroup1, int ComponentsInGroup2) CountComponentsInTwoComponentGroupsAfterTargetCuts(
        Dictionary<string, HashSet<string>> wiredComponentsGraph,
        int targetCutsCount)
    {
        while (true)
        {
            var wiredComponentsGraphCopy = wiredComponentsGraph.ToDictionary(x => x.Key, x => x.Value.ToHashSet());
            while (wiredComponentsGraphCopy.Count > 2)
            {
                var (component1, component2) = GetRandomComponentsConnectionToContract(wiredComponentsGraphCopy);

                ContractComponentsConnection(wiredComponentsGraphCopy, component1, component2);
            }

            // At this point we only have 2 components representing the groups that left after the performed component connection contractions
            var (componentsInGroup1, componentsInGroup2) = CountComponentsInBothGroupsIfTargetCuts(
                wiredComponentsGraph,
                wiredComponentsGraphCopy.Keys.First(),
                wiredComponentsGraphCopy.Keys.Last(),
                targetCutsCount);

            if (componentsInGroup1 > 0 && componentsInGroup2 > 0)
            {
                return (componentsInGroup1, componentsInGroup2);
            }
        }
    }

    private static (int, int) CountComponentsInBothGroupsIfTargetCuts(
        Dictionary<string, HashSet<string>> originalWiredComponentsGraph,
        string mergedComponentsInGroup1,
        string mergedComponentsInGroup2,
        int targetCutsCount)
    {
        var cutConnectedComponents = new HashSet<(string, string)>(capacity: targetCutsCount + 1);

        var componentsInGroup1 = mergedComponentsInGroup1.Split('_');
        var componentsInGroup2 = mergedComponentsInGroup2.Split('_');

        // Count connections in the original graph between components from resulting 2 groups after component connection contractions
        foreach (var component in componentsInGroup1)
        {
            var connectedComponents = originalWiredComponentsGraph[component];
            foreach (var component2 in componentsInGroup2)
            {
                if (connectedComponents.Contains(component2))
                {
                    cutConnectedComponents.Add(string.CompareOrdinal(component, component2) < 0
                        ? (component, component2)
                        : (component2, component));

                    // Already have more cuts than target => ignore the result
                    if (cutConnectedComponents.Count > targetCutsCount)
                    {
                        return (0, 0);
                    }
                }
            }
        }

        foreach (var component in componentsInGroup2)
        {
            var connectedComponents = originalWiredComponentsGraph[component];
            foreach (var component2 in componentsInGroup1)
            {
                if (connectedComponents.Contains(component2))
                {
                    cutConnectedComponents.Add(string.CompareOrdinal(component, component2) < 0
                        ? (component, component2)
                        : (component2, component));

                    // Already have more cuts than target => ignore the result
                    if (cutConnectedComponents.Count > targetCutsCount)
                    {
                        return (0, 0);
                    }
                }
            }
        }

        if (cutConnectedComponents.Count != targetCutsCount)
        {
            return (0, 0);
        }

        return (componentsInGroup1.Length, componentsInGroup2.Length);
    }

    private static void ContractComponentsConnection(
        Dictionary<string, HashSet<string>> wiredComponentsGraph,
        string component1,
        string component2)
    {
        wiredComponentsGraph.Remove(component1, out var connectedComponentsOfComponent1);
        connectedComponentsOfComponent1!.Remove(component2);

        wiredComponentsGraph.Remove(component2, out var connectedComponentsOfComponent2);
        connectedComponentsOfComponent2!.Remove(component1);

        var newComponentName = $"{component1}_{component2}";
        var newComponentConnectedComponents = connectedComponentsOfComponent1.Concat(connectedComponentsOfComponent2).ToHashSet();
        wiredComponentsGraph[newComponentName] = newComponentConnectedComponents;

        foreach (var component in newComponentConnectedComponents)
        {
            var connectedComponents = wiredComponentsGraph[component];
            connectedComponents.Add(newComponentName);
            connectedComponents.Remove(component1);
            connectedComponents.Remove(component2);
        }
    }

    private static (string Component1, string Component2) GetRandomComponentsConnectionToContract(
        Dictionary<string, HashSet<string>> wiredComponentsGraph)
    {
        var component1Index = Random.Shared.Next(wiredComponentsGraph.Count);
        var component1 = wiredComponentsGraph.Keys.ElementAt(component1Index);

        var component2Index = Random.Shared.Next(wiredComponentsGraph[component1].Count);
        var component2 = wiredComponentsGraph[component1].ElementAt(component2Index);

        return (component1, component2);
    }

    private static Dictionary<string, HashSet<string>> ReadWiredComponentsGraph()
    {
        var wiredComponentsGraph = new Dictionary<string, HashSet<string>>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var componentNameAndConnectedComponentNamesSeparatorIndex = line.IndexOf(':');
            var componentName = line[..componentNameAndConnectedComponentNamesSeparatorIndex];
            var connectedComponentNames =
                line[(componentNameAndConnectedComponentNamesSeparatorIndex + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!wiredComponentsGraph.TryGetValue(componentName, out var component))
            {
                component = [];
                wiredComponentsGraph[componentName] = component;
            }

            foreach (var connectedComponentName in connectedComponentNames)
            {
                if (!wiredComponentsGraph.TryGetValue(connectedComponentName, out var connectedComponent))
                {
                    connectedComponent = [];
                    wiredComponentsGraph[connectedComponentName] = connectedComponent;
                }

                component.Add(connectedComponentName);
                connectedComponent.Add(componentName);
            }
        }

        return wiredComponentsGraph;
    }
}
