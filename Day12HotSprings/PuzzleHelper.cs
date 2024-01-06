namespace Day12HotSprings;

internal static class PuzzleHelper
{
    public static List<(char[] SpringsConditionRecord, int[] GroupsOfDamagedSprings)> ReadSpringsConditionRecordsAndGroupsOfDamagedSprings()
    {
        var result = new List<(char[], int[])>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var separatorIndex = line.IndexOf(' ');
            var springsConditionRecord = line.AsSpan()[..separatorIndex].ToArray();
            var groupsOfDamagedSprings = line[(separatorIndex + 1)..]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            result.Add((springsConditionRecord, groupsOfDamagedSprings));
        }

        return result;
    }

    public static long CalculatePossibleArrangements(char[] springsConditionRecord, int[] groupsOfDamagedSprings)
        => CalculatePossibleArrangements(
            springsConditionRecord,
            groupsOfDamagedSprings,
            alreadyCalculatedPossibleArrangements: [],
            currentSpringIndex: 0,
            currentDamagedSprings: 0,
            currentDamagedSpringsGroup: 0);

    // Approach: dynamic programming
    private static long CalculatePossibleArrangements(
        char[] springsConditionRecord,
        int[] groupsOfDamagedSprings,
        Dictionary<(int, int, int), long> alreadyCalculatedPossibleArrangements,
        int currentSpringIndex,
        int currentDamagedSprings,
        int currentDamagedSpringsGroup)
    {
        if (currentSpringIndex >= springsConditionRecord.Length)
        {
            // All groups already filled OR just filled the last group
            if (currentDamagedSpringsGroup >= groupsOfDamagedSprings.Length ||
                (currentDamagedSpringsGroup == groupsOfDamagedSprings.Length - 1 &&
                    currentDamagedSprings == groupsOfDamagedSprings[currentDamagedSpringsGroup]))
            {
                return 1;
            }

            return 0;
        }

        if (springsConditionRecord[currentSpringIndex] == '.')
        {
            // In operational springs group
            if (currentDamagedSprings == 0)
            {
                return CalculatePossibleArrangements(
                    springsConditionRecord,
                    groupsOfDamagedSprings,
                    alreadyCalculatedPossibleArrangements,
                    currentSpringIndex + 1,
                    currentDamagedSprings,
                    currentDamagedSpringsGroup);
            }

            // More groups filled than needed OR the current group cannot be filled with current damaged springs
            if (currentDamagedSpringsGroup >= groupsOfDamagedSprings.Length ||
                currentDamagedSprings != groupsOfDamagedSprings[currentDamagedSpringsGroup])
            {
                return 0;
            }

            // Filled the group => process to next one
            return CalculatePossibleArrangements(
                springsConditionRecord,
                groupsOfDamagedSprings,
                alreadyCalculatedPossibleArrangements,
                currentSpringIndex + 1,
                currentDamagedSprings: 0,
                currentDamagedSpringsGroup + 1);
        }

        if (springsConditionRecord[currentSpringIndex] == '#')
        {
            // Hit a damaged spring but no more expected
            if (currentDamagedSpringsGroup >= groupsOfDamagedSprings.Length ||
                currentDamagedSprings + 1 > groupsOfDamagedSprings[currentDamagedSpringsGroup])
            {
                return 0;
            }

            return CalculatePossibleArrangements(
                springsConditionRecord,
                groupsOfDamagedSprings,
                alreadyCalculatedPossibleArrangements,
                currentSpringIndex + 1,
                currentDamagedSprings + 1,
                currentDamagedSpringsGroup);
        }

        // unknown (?) spring
        var cacheKey = (currentSpringIndex, currentDamagedSprings, currentDamagedSpringsGroup);
        if (alreadyCalculatedPossibleArrangements.TryGetValue(cacheKey, out var possibleArrangements))
        {
            return possibleArrangements;
        }

        // Currently not in a damaged springs group so consider current one operational
        if (currentDamagedSprings == 0)
        {
            possibleArrangements += CalculatePossibleArrangements(
                springsConditionRecord,
                groupsOfDamagedSprings,
                alreadyCalculatedPossibleArrangements,
                currentSpringIndex + 1,
                currentDamagedSprings,
                currentDamagedSpringsGroup);
        }

        // Consider adding a damaged spring to the current group
        if (currentDamagedSpringsGroup < groupsOfDamagedSprings.Length &&
            currentDamagedSprings < groupsOfDamagedSprings[currentDamagedSpringsGroup])
        {
            possibleArrangements += CalculatePossibleArrangements(
                springsConditionRecord,
                groupsOfDamagedSprings,
                alreadyCalculatedPossibleArrangements,
                currentSpringIndex + 1,
                currentDamagedSprings + 1,
                currentDamagedSpringsGroup);
        }

        // Consider filling the current group with a damaged spring and moving to the next group
        if (currentDamagedSpringsGroup < groupsOfDamagedSprings.Length &&
            currentDamagedSprings == groupsOfDamagedSprings[currentDamagedSpringsGroup])
        {
            possibleArrangements += CalculatePossibleArrangements(
                springsConditionRecord,
                groupsOfDamagedSprings,
                alreadyCalculatedPossibleArrangements,
                currentSpringIndex + 1,
                currentDamagedSprings: 0,
                currentDamagedSpringsGroup + 1);
        }

        alreadyCalculatedPossibleArrangements[cacheKey] = possibleArrangements;

        return possibleArrangements;
    }
}
