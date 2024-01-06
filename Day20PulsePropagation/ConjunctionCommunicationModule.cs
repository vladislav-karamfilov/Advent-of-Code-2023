namespace Day20PulsePropagation;

public class ConjunctionCommunicationModule : CommunicationModule
{
    private readonly Dictionary<string, PulseType> memory = [];

    public override CommunicationModuleType Type => CommunicationModuleType.Conjunction;

    public override void AddInputModule(CommunicationModule module)
    {
        this.memory[module.Name] = PulseType.Low;
        base.AddInputModule(module);
    }

    public override IEnumerable<PulseToSend> ReceivePulse(PulseType pulseType, string sourceModuleName)
    {
        this.memory[sourceModuleName] = pulseType;

        var pulseTypeToSend = this.memory.Values.All(pt => pt == PulseType.High) ? PulseType.Low : PulseType.High;
        return base.ReceivePulse(pulseTypeToSend, sourceModuleName);
    }
}
