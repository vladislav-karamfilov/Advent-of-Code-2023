namespace Day19Aplenty;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/19#part2
    public static void Solve()
    {
        var machinePartsOrganizingSystem = PuzzleHelper.ReadMachinePartsOrganizingSystem(readMachineParts: false);

        Console.WriteLine(machinePartsOrganizingSystem.CalculateDistinctCombinationsOfMachinePartRatingsForMachinePartAcceptance());
    }
}
