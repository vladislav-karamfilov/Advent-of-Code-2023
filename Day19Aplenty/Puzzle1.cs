namespace Day19Aplenty;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/19
    public static void Solve()
    {
        var machinePartsOrganizingSystem = PuzzleHelper.ReadMachinePartsOrganizingSystem(readMachineParts: true);

        Console.WriteLine(machinePartsOrganizingSystem.CalculateSumOfRatingsOfAcceptedParts());
    }
}
