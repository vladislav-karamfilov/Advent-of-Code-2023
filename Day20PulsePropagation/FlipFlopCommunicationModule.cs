namespace Day20PulsePropagation;

public class FlipFlopCommunicationModule : CommunicationModule
{
    private bool isOn;

    public override CommunicationModuleType Type => CommunicationModuleType.FlipFlop;

    public override IEnumerable<PulseToSend> ReceivePulse(PulseType pulseType, string sourceModuleName)
    {
        if (pulseType == PulseType.High)
        {
            return Enumerable.Empty<PulseToSend>(); // Ignore high pulses
        }

        this.isOn = !this.isOn;

        var pulseTypeToSend = this.isOn ? PulseType.High : PulseType.Low;
        return base.ReceivePulse(pulseTypeToSend, sourceModuleName);
    }
}
