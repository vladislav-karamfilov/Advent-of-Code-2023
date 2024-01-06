namespace Day20PulsePropagation;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/20
    public static void Solve()
    {
        var communicationSystem = PuzzleHelper.ReadCommunicationSystem(Console.In);

        var lowPulsesSent = 0L;
        var highPulsesSent = 0L;
        for (var i = 0; i < 1_000; i++)
        {
            var (currentLowPulsesSent, currentHighPulsesSent) = communicationSystem.HandleButtonPush();
            lowPulsesSent += currentLowPulsesSent;
            highPulsesSent += currentHighPulsesSent;
        }

        Console.WriteLine(lowPulsesSent * highPulsesSent);
    }
}
