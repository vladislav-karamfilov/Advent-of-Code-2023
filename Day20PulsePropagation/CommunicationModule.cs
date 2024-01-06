namespace Day20PulsePropagation;

public abstract class CommunicationModule
{
    public string Name { get; init; } = null!;

    public abstract CommunicationModuleType Type { get; }

    public List<CommunicationModule> InputModules { get; } = [];

    public List<CommunicationModule> OutputModules { get; } = [];

#pragma warning disable CA1716 // Identifiers should not match keywords
    public virtual void AddInputModule(CommunicationModule module) => this.InputModules.Add(module);

    public virtual void AddOutputModule(CommunicationModule module) => this.OutputModules.Add(module);
#pragma warning restore CA1716 // Identifiers should not match keywords

    public virtual IEnumerable<PulseToSend> ReceivePulse(PulseType pulseType, string sourceModuleName)
    {
        foreach (var module in this.OutputModules)
        {
            yield return new PulseToSend(pulseType, this.Name, module);
        }
    }
}
