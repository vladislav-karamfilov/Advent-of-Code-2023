namespace Day12HotSprings;

using System;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/12#part2
    public static void Solve()
    {
        var springsConditionRecordsAndGroupsOfDamagedSprings = PuzzleHelper.ReadSpringsConditionRecordsAndGroupsOfDamagedSprings();

        var sum = 0L;
        foreach (var (springsConditionRecord, groupsOfDamagedSprings) in springsConditionRecordsAndGroupsOfDamagedSprings)
        {
            var unfoldedSpringsConditionRecord = Enumerable.Empty<char>();
            var unfoldedGroupsOfDamagedSprings = Enumerable.Empty<int>();
            for (var i = 0; i < 5; i++)
            {
                unfoldedSpringsConditionRecord = unfoldedSpringsConditionRecord.Concat(i == 0 ? [] : ['?']).Concat(springsConditionRecord);
                unfoldedGroupsOfDamagedSprings = unfoldedGroupsOfDamagedSprings.Concat(groupsOfDamagedSprings);
            }

            sum += PuzzleHelper.CalculatePossibleArrangements(
                unfoldedSpringsConditionRecord.ToArray(),
                unfoldedGroupsOfDamagedSprings.ToArray());
        }

        Console.WriteLine(sum);
    }
}
