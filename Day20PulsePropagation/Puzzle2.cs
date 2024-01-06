namespace Day20PulsePropagation;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/20#part2
    public static void Solve()
    {
        CommunicationSystem communicationSystem;
        using (var reader = new StreamReader("input.txt"))
        {
            communicationSystem = PuzzleHelper.ReadCommunicationSystem(reader);
        }

        var rxModule = communicationSystem.GetModuleByName("rx");
        if (rxModule is null)
        {
            Console.WriteLine("No 'rx' module in communication system.");
            return;
        }

        var inputModules = rxModule.InputModules;
        if (inputModules.Count == 0)
        {
            Console.WriteLine("'rx' module doesn't have input modules.");
            return;
        }

        // NOTE: this is specific to the specific input and might need a different logic for another input but the algorithm will stay the same
        if (inputModules.Count == 1 &&
            inputModules[0].Type == CommunicationModuleType.Conjunction &&
            inputModules[0].InputModules.All(m => m.Type == CommunicationModuleType.Conjunction))
        {
            // The conjunction input modules of the single conjunction input module of the 'rx' module must receive LOW pulse
            // so the 'rx' module's single conjunction input module will receive a HIGH pulse which in turn will be converted to
            // LOW pulse being sent to the 'rx' module
            var buttonPushesForLowPulseOnConjunctionModules = communicationSystem.HandleButtonPushesUntilPulseReceivedForAllModules(
                PulseType.Low,
                inputModules[0].InputModules);

            Console.WriteLine(CalculateLeastCommonMultiple(buttonPushesForLowPulseOnConjunctionModules));
        }
    }

    private static long CalculateLeastCommonMultiple(List<int> numbers)
    {
        var result = numbers switch
        {
            [var first, var second] => CalculateLeastCommonMultiple(first, second),
            [var first, .. var rest] => CalculateLeastCommonMultiple(first, CalculateLeastCommonMultiple(rest)),
            _ => throw new InvalidOperationException($"Cannot calculate LCM of {numbers.Count} number(s)")
        };

        return result;
    }

    private static long CalculateLeastCommonMultiple(long a, long b)
        => a * (b / CalculateGreatestCommonDenominator(a, b));

    private static long CalculateGreatestCommonDenominator(long a, long b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }
            else
            {
                b %= a;
            }
        }

        return a | b;
    }
}
