namespace Day12HotSprings;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/12
    public static void Solve()
    {
        var springsConditionRecordsAndGroupsOfDamagedSprings = PuzzleHelper.ReadSpringsConditionRecordsAndGroupsOfDamagedSprings();

        var sum = 0L;
        foreach (var (springsConditionRecord, groupsOfDamagedSprings) in springsConditionRecordsAndGroupsOfDamagedSprings)
        {
            sum += PuzzleHelper.CalculatePossibleArrangements(springsConditionRecord, groupsOfDamagedSprings);
        }

        Console.WriteLine(sum);
    }
}
