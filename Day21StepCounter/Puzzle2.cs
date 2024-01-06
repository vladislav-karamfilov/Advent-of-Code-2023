namespace Day21StepCounter;

using System;
using System.Text;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/21#part2
    public static void Solve()
    {
        var gardenMap = PuzzleHelper.ReadGardenMap();

        var reachableGardenPlotsInSpecifiedSteps = CalculateReachableGardenPlotsInSpecifiedSteps(gardenMap);

        Console.WriteLine(reachableGardenPlotsInSpecifiedSteps);
    }

    // Lagrange's Interpolation formula (https://en.wikipedia.org/wiki/Lagrange_polynomial) for ax^2 + bx + c
    // with x=[0,1,2] and y=[y0,y1,y2] is f(x) = (x^2 - 3x + 2) * y0/2 - (x^2 - 2x) * y1 + (x^2 - x) * y2/2
    // Coefficients:
    // a = y0/2 - y1 + y2/2
    // b = -3 * y0/2 + 2 * y1 - y2/2
    // c = y0
    private static long CalculateReachableGardenPlotsInSpecifiedSteps(List<string> gardenMap)
    {
        const long TargetSteps = 26_501_365L;

        var enlargedGardenMap = EnlargeGardenMap(gardenMap);

        var y0 = PuzzleHelper.CalculateReachableGardenPlotsInSpecifiedSteps(enlargedGardenMap, targetSteps: 65 + (131 * 0));
        var y1 = PuzzleHelper.CalculateReachableGardenPlotsInSpecifiedSteps(enlargedGardenMap, targetSteps: 65 + (131 * 1));
        var y2 = PuzzleHelper.CalculateReachableGardenPlotsInSpecifiedSteps(enlargedGardenMap, targetSteps: 65 + (131 * 2));

        var a = (y0 / 2) - y1 + (y2 / 2);
        var b = (-3 * y0 / 2) + (2 * y1) - (y2 / 2);
        var c = y0;

        var target = (TargetSteps - 65) / 131;
        return (a * target * target) + (b * target) + c;
    }

    private static List<string> EnlargeGardenMap(List<string> gardenMap)
    {
        const int Size = 5;

        var result = new List<string>(capacity: gardenMap.Count * 5);
        for (var i = 0; i < Size; i++)
        {
            foreach (var line in gardenMap)
            {
                var sb = new StringBuilder();
                for (var j = 0; j < Size; j++)
                {
                    if (i == Size / 2 && j == Size / 2)
                    {
                        sb.Append(line);
                    }
                    else
                    {
                        sb.Append(line.Replace('S', '.'));
                    }
                }

                result.Add(sb.ToString());
            }
        }

        return result;
    }
}
