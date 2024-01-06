namespace Day19Aplenty;

public class WorkflowRule
{
    public WorkflowCondition? Condition { get; set; }

    public string? TargetWorkflow { get; set; }

    public MachinePartStatus TargetStatus { get; set; }
}
