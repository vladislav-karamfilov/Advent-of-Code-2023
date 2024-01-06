namespace Day19Aplenty;

internal static class PuzzleHelper
{
    public static MachinePartsOrganizingSystem ReadMachinePartsOrganizingSystem(bool readMachineParts)
    {
        var result = new MachinePartsOrganizingSystem();

        var isReadingMachineParts = false;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                if (isReadingMachineParts || !readMachineParts)
                {
                    break;
                }

                isReadingMachineParts = true;
                continue;
            }

            if (isReadingMachineParts)
            {
                result.AddMachinePart(ParseMachinePart(line));
            }
            else
            {
                result.AddWorkflow(ParseWorkflow(line));
            }
        }

        return result;
    }

    private static Workflow ParseWorkflow(string line)
    {
        var nameEndIndex = line.IndexOf('{');
        var name = line[..nameEndIndex];

        var result = new Workflow { Name = name };

        var rawRules = line[(nameEndIndex + 1)..^1].Split(',');
        foreach (var rawRule in rawRules)
        {
            if (rawRule == "A")
            {
                result.AddRule(new WorkflowRule { TargetStatus = MachinePartStatus.Accepted });
            }
            else if (rawRule == "R")
            {
                result.AddRule(new WorkflowRule { TargetStatus = MachinePartStatus.Rejected });
            }
            else
            {
                var conditionAndTargetWorkflowOrStatusParts = rawRule.Split(':');
                if (conditionAndTargetWorkflowOrStatusParts.Length != 2)
                {
                    result.AddRule(new WorkflowRule { TargetWorkflow = rawRule });
                }
                else
                {
                    var rawCondition = conditionAndTargetWorkflowOrStatusParts[0];
                    var machinePartRating = rawCondition[0] switch
                    {
                        'x' => MachinePartRating.ExtremelyCoolLooking,
                        'm' => MachinePartRating.Musical,
                        'a' => MachinePartRating.Aerodynamic,
                        's' => MachinePartRating.Shiny,
                        _ => throw new InvalidOperationException($"Invalid machine part rating: {rawCondition[0]}"),
                    };

                    var @operator = rawCondition[1] == '>' ? WorkflowConditionOperator.GreaterThan : WorkflowConditionOperator.LessThan;

                    var targetValue = int.Parse(rawCondition.AsSpan()[2..]);

                    var targetWorkflowOrStatus = conditionAndTargetWorkflowOrStatusParts[1];
                    var targetStatus = targetWorkflowOrStatus == "A"
                        ? MachinePartStatus.Accepted
                        : targetWorkflowOrStatus == "R"
                            ? MachinePartStatus.Rejected
                            : MachinePartStatus.Unknown;

                    var targetWorkflow = targetStatus == MachinePartStatus.Unknown ? targetWorkflowOrStatus : null;

                    result.AddRule(new WorkflowRule
                    {
                        Condition = new WorkflowCondition
                        {
                            MachinePartRating = machinePartRating,
                            MachinePartRatingTargetValue = targetValue,
                            Operator = @operator,
                        },
                        TargetStatus = targetStatus,
                        TargetWorkflow = targetWorkflow,
                    });
                }
            }
        }

        return result;
    }

    private static MachinePart ParseMachinePart(string line)
    {
        var result = new MachinePart();

        var rawRatings = line[1..^1].Split(',');
        foreach (var rawRating in rawRatings)
        {
            var ratingValue = int.Parse(rawRating.AsSpan()[2..]);

            if (rawRating[0] == 'x')
            {
                result.ExtremelyCoolLookingRating = ratingValue;
            }
            else if (rawRating[0] == 'm')
            {
                result.MusicalRating = ratingValue;
            }
            else if (rawRating[0] == 'a')
            {
                result.AerodynamicRating = ratingValue;
            }
            else if (rawRating[0] == 's')
            {
                result.ShinyRating = ratingValue;
            }
        }

        return result;
    }
}
