namespace Day19Aplenty;

public class WorkflowCondition
{
    public MachinePartRating MachinePartRating { get; init; }

    public WorkflowConditionOperator Operator { get; init; }

    public int MachinePartRatingTargetValue { get; init; }

    public bool Evaluate(MachinePart machinePart)
    {
        var rating = this.MachinePartRating switch
        {
            MachinePartRating.ExtremelyCoolLooking => machinePart.ExtremelyCoolLookingRating,
            MachinePartRating.Musical => machinePart.MusicalRating,
            MachinePartRating.Aerodynamic => machinePart.AerodynamicRating,
            MachinePartRating.Shiny => machinePart.ShinyRating,
            _ => throw new InvalidOperationException($"Invalid machine part rating: {this.MachinePartRating}"),
        };

        if (this.Operator == WorkflowConditionOperator.LessThan)
        {
            return rating < this.MachinePartRatingTargetValue;
        }

        return rating > this.MachinePartRatingTargetValue;
    }

    public WorkflowConditionEvaluationResult Evaluate(MachinePartRatingsRange machinePartRatingsRange)
    {
        MachinePartRatingsRange acceptedRange;
        MachinePartRatingsRange rejectedRange;

        var (extremelyCoolLookingRange, musicalRange, aerodynamicRange, shinyRange) = machinePartRatingsRange;

        switch (this.MachinePartRating)
        {
            case MachinePartRating.ExtremelyCoolLooking:
                acceptedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        ExtremelyCoolLookingRange = extremelyCoolLookingRange with { End = this.MachinePartRatingTargetValue - 1 },
                    }
                    : machinePartRatingsRange with
                    {
                        ExtremelyCoolLookingRange = extremelyCoolLookingRange with { Start = this.MachinePartRatingTargetValue + 1 },
                    };

                rejectedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        ExtremelyCoolLookingRange = extremelyCoolLookingRange with { Start = this.MachinePartRatingTargetValue },
                    }
                    : machinePartRatingsRange with
                    {
                        ExtremelyCoolLookingRange = extremelyCoolLookingRange with { End = this.MachinePartRatingTargetValue },
                    };
                break;

            case MachinePartRating.Musical:
                acceptedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        MusicalRange = musicalRange with { End = this.MachinePartRatingTargetValue - 1 },
                    }
                    : machinePartRatingsRange with
                    {
                        MusicalRange = musicalRange with { Start = this.MachinePartRatingTargetValue + 1 },
                    };

                rejectedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        MusicalRange = musicalRange with { Start = this.MachinePartRatingTargetValue },
                    }
                    : machinePartRatingsRange with
                    {
                        MusicalRange = musicalRange with { End = this.MachinePartRatingTargetValue },
                    };
                break;

            case MachinePartRating.Aerodynamic:
                acceptedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        AerodynamicRange = aerodynamicRange with { End = this.MachinePartRatingTargetValue - 1 },
                    }
                    : machinePartRatingsRange with
                    {
                        AerodynamicRange = aerodynamicRange with { Start = this.MachinePartRatingTargetValue + 1 },
                    };

                rejectedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        AerodynamicRange = aerodynamicRange with { Start = this.MachinePartRatingTargetValue },
                    }
                    : machinePartRatingsRange with
                    {
                        AerodynamicRange = aerodynamicRange with { End = this.MachinePartRatingTargetValue },
                    };
                break;

            case MachinePartRating.Shiny:
                acceptedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        ShinyRange = shinyRange with { End = this.MachinePartRatingTargetValue - 1 },
                    }
                    : machinePartRatingsRange with
                    {
                        ShinyRange = shinyRange with { Start = this.MachinePartRatingTargetValue + 1 },
                    };

                rejectedRange = this.Operator == WorkflowConditionOperator.LessThan
                    ? machinePartRatingsRange with
                    {
                        ShinyRange = shinyRange with { Start = this.MachinePartRatingTargetValue },
                    }
                    : machinePartRatingsRange with
                    {
                        ShinyRange = shinyRange with { End = this.MachinePartRatingTargetValue },
                    };
                break;

            default:
                throw new InvalidOperationException($"Invalid machine part rating: {this.MachinePartRating}");
        }

        return new WorkflowConditionEvaluationResult(acceptedRange, rejectedRange);
    }
}
