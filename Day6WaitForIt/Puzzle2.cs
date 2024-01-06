namespace Day6WaitForIt;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/6#part2
    public static void Solve()
    {
        var (raceDurationInMs, recordRaceDistanceInMm) = ReadInput();

        var winningWays = CalculateWinningWaysForRace(raceDurationInMs, recordRaceDistanceInMm);

        Console.WriteLine(winningWays);
    }

    private static long CalculateWinningWaysForRace(long raceDurationInMs, long recordRaceDistanceInMm)
    {
        var inWinningSpan = false;
        var winningWays = 0L;
        for (long chargeMs = 1; chargeMs < raceDurationInMs; chargeMs++)
        {
            var moveMs = raceDurationInMs - chargeMs;
            var movedDistanceInMm = moveMs * chargeMs;

            if (movedDistanceInMm > recordRaceDistanceInMm)
            {
                winningWays++;
                inWinningSpan = true;
            }
            else if (inWinningSpan)
            {
                break;
            }
        }

        return winningWays;
    }

    private static (long RaceDurationsInMs, long RecordRaceDistancesInMm) ReadInput()
    {
        var raceDurationInMsLine = Console.ReadLine()!;
        var durationStartIndex = raceDurationInMsLine.IndexOf(':') + 1;
        var raceDurationInMs = long.Parse(raceDurationInMsLine[durationStartIndex..].Where(char.IsDigit).ToArray());

        var recordRaceDistanceInMmLine = Console.ReadLine()!;
        var distanceStartIndex = recordRaceDistanceInMmLine.IndexOf(':') + 1;
        var recordRaceDistanceInMm = long.Parse(recordRaceDistanceInMmLine[distanceStartIndex..].Where(char.IsDigit).ToArray());

        return (raceDurationInMs, recordRaceDistanceInMm);
    }
}
