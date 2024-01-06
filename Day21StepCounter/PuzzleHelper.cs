namespace Day21StepCounter;

internal static class PuzzleHelper
{
    // Implementation of BFS: https://en.wikipedia.org/wiki/Breadth-first_search
    public static int CalculateReachableGardenPlotsInSpecifiedSteps(List<string> gardenMap, int targetSteps)
    {
        var startLocation = FindStartLocation(gardenMap);

        var gardenPlotsToVisit = new Queue<GardenPlotToVisit>();
        gardenPlotsToVisit.Enqueue(new GardenPlotToVisit(startLocation, Step: 0));

        var gardenPlotsToVisitInQueue = new HashSet<GardenPlotToVisit>();
        var reachableGardenPlotsInSpecifiedSteps = 0;

        while (gardenPlotsToVisit.Count > 0)
        {
            var (location, step) = gardenPlotsToVisit.Dequeue();
            if (step == targetSteps)
            {
                reachableGardenPlotsInSpecifiedSteps++;
            }
            else if (step > targetSteps)
            {
                break;
            }

            var (row, col) = location;

            // Up
            if (row > 0 && gardenMap[row - 1][col] != '#')
            {
                var newGardenPlotToVisit = new GardenPlotToVisit(location with { Row = row - 1 }, step + 1);
                if (!gardenPlotsToVisitInQueue.Contains(newGardenPlotToVisit))
                {
                    gardenPlotsToVisit.Enqueue(newGardenPlotToVisit);
                    gardenPlotsToVisitInQueue.Add(newGardenPlotToVisit);
                }
            }

            // Left
            if (col > 0 && gardenMap[row][col - 1] != '#')
            {
                var newGardenPlotToVisit = new GardenPlotToVisit(location with { Col = col - 1 }, step + 1);
                if (!gardenPlotsToVisitInQueue.Contains(newGardenPlotToVisit))
                {
                    gardenPlotsToVisit.Enqueue(newGardenPlotToVisit);
                    gardenPlotsToVisitInQueue.Add(newGardenPlotToVisit);
                }
            }

            // Down
            if (row < gardenMap.Count - 1 && gardenMap[row + 1][col] != '#')
            {
                var newGardenPlotToVisit = new GardenPlotToVisit(location with { Row = row + 1 }, step + 1);
                if (!gardenPlotsToVisitInQueue.Contains(newGardenPlotToVisit))
                {
                    gardenPlotsToVisit.Enqueue(newGardenPlotToVisit);
                    gardenPlotsToVisitInQueue.Add(newGardenPlotToVisit);
                }
            }

            // Right
            if (col < gardenMap[0].Length - 1 && gardenMap[row][col + 1] != '#')
            {
                var newGardenPlotToVisit = new GardenPlotToVisit(location with { Col = col + 1 }, step + 1);
                if (!gardenPlotsToVisitInQueue.Contains(newGardenPlotToVisit))
                {
                    gardenPlotsToVisit.Enqueue(newGardenPlotToVisit);
                    gardenPlotsToVisitInQueue.Add(newGardenPlotToVisit);
                }
            }
        }

        return reachableGardenPlotsInSpecifiedSteps;
    }

    public static List<string> ReadGardenMap()
    {
        var gardenMap = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            gardenMap.Add(line);
        }

        return gardenMap;
    }

    private static GardenPlotLocation FindStartLocation(List<string> gardenMap)
    {
        var startRow = gardenMap.FindIndex(x => x.Contains('S'));
        var startCol = gardenMap[startRow].IndexOf('S');

        return new GardenPlotLocation(startRow, startCol);
    }
}
