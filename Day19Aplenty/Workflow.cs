namespace Day19Aplenty;

public class Workflow
{
    private readonly List<WorkflowRule> rules = [];

    public string Name { get; init; } = null!;

    public IEnumerable<WorkflowRule> Rules => this.rules;

    public void AddRule(WorkflowRule rule) => this.rules.Add(rule);

    public WorkflowExecutionResult Execute(MachinePart machinePart)
    {
        foreach (var rule in this.rules)
        {
            if (rule.Condition is null || rule.Condition.Evaluate(machinePart))
            {
                return new WorkflowExecutionResult { MachinePartStatus = rule.TargetStatus, TargetWorkflow = rule.TargetWorkflow };
            }
        }

        return default; // Shouldn't come to this point
    }
}
