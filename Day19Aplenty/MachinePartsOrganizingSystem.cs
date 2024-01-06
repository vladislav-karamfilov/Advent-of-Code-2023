namespace Day19Aplenty;

public class MachinePartsOrganizingSystem
{
    private const string StartingWorkflowName = "in";

    private readonly Dictionary<string, Workflow> workflowsByName = [];
    private readonly List<MachinePart> machineParts = [];

    private bool isOrganized;

    public void AddWorkflow(Workflow workflow) => this.workflowsByName[workflow.Name] = workflow;

    public void AddMachinePart(MachinePart machinePart) => this.machineParts.Add(machinePart);

    public int CalculateSumOfRatingsOfAcceptedParts()
    {
        if (!this.isOrganized)
        {
            this.Organize();

            this.isOrganized = true;
        }

        return this.machineParts.Where(p => p.Status == MachinePartStatus.Accepted).Sum(p => p.TotalRating);
    }

    public long CalculateDistinctCombinationsOfMachinePartRatingsForMachinePartAcceptance()
    {
        var range = new MachinePartRatingsRange(new Range(1, 4_000), new Range(1, 4_000), new Range(1, 4_000), new Range(1, 4_000));
        var startingWorkflow = this.workflowsByName[StartingWorkflowName];
        var acceptedRanges = new List<MachinePartRatingsRange>();

        FindAcceptedRangesForMachinePartAcceptance(range, startingWorkflow, acceptedRanges);

        return acceptedRanges.Sum(r => r.UniqueCombinations);
    }

    private void FindAcceptedRangesForMachinePartAcceptance(
        MachinePartRatingsRange range,
        Workflow workflow,
        List<MachinePartRatingsRange> acceptedRanges)
    {
        foreach (var rule in workflow.Rules)
        {
            if (rule.Condition is not null)
            {
                var (acceptedRange, rejectedRange) = rule.Condition.Evaluate(range);
                if (rule.TargetStatus == MachinePartStatus.Accepted)
                {
                    acceptedRanges.Add(acceptedRange);
                }
                else if (rule.TargetWorkflow is not null)
                {
                    FindAcceptedRangesForMachinePartAcceptance(acceptedRange, this.workflowsByName[rule.TargetWorkflow], acceptedRanges);
                }

                // Continue checking rules with the rejected part of the range used in the condition above
                range = rejectedRange;
            }
            else if (rule.TargetStatus == MachinePartStatus.Accepted)
            {
                acceptedRanges.Add(range);
            }
            else if (rule.TargetWorkflow is not null)
            {
                FindAcceptedRangesForMachinePartAcceptance(range, this.workflowsByName[rule.TargetWorkflow], acceptedRanges);
            }
        }
    }

    private void Organize()
    {
        foreach (var machinePart in this.machineParts)
        {
            var workflow = this.workflowsByName[StartingWorkflowName];

            while (true)
            {
                var workflowExecutionResult = workflow.Execute(machinePart);
                if (workflowExecutionResult.MachinePartStatus != MachinePartStatus.Unknown)
                {
                    machinePart.Status = workflowExecutionResult.MachinePartStatus;
                    break;
                }

                workflow = this.workflowsByName[workflowExecutionResult.TargetWorkflow!];
            }
        }
    }
}
