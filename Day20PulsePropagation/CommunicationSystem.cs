namespace Day20PulsePropagation;

using System.Linq;

public class CommunicationSystem
{
    private readonly List<CommunicationModule> modules = [];

    private CommunicationModule? broadcasterModule;

    public void AddModule(CommunicationModule module) => this.modules.Add(module);

    public CommunicationModule? GetModuleByName(string name) => this.modules.FirstOrDefault(m => m.Name == name);

    public (int LowPulsesSent, int HighPulsesSent) HandleButtonPush()
    {
        this.broadcasterModule ??= this.modules.Single(m => m.Type == CommunicationModuleType.Broadcaster);

        var pulsesToSend = new Queue<PulseToSend>();
        pulsesToSend.Enqueue(new PulseToSend(PulseType.Low, "button", this.broadcasterModule));

        var lowPulsesSent = 0;
        var highPulsesSent = 0;

        while (pulsesToSend.Count > 0)
        {
            var (pulseType, sourceModuleName, destinationModule) = pulsesToSend.Dequeue();
            if (pulseType == PulseType.Low)
            {
                lowPulsesSent++;
            }
            else // if (pulseType == PulseType.High)
            {
                highPulsesSent++;
            }

            var newPulsesToSend = destinationModule.ReceivePulse(pulseType, sourceModuleName);
            foreach (var newPulseToSend in newPulsesToSend)
            {
                pulsesToSend.Enqueue(newPulseToSend);
            }
        }

        return (lowPulsesSent, highPulsesSent);
    }

    public List<int> HandleButtonPushesUntilPulseReceivedForAllModules(PulseType targetPulseType, List<CommunicationModule> modules)
    {
        var result = new List<int>();

        this.broadcasterModule ??= this.modules.Single(m => m.Type == CommunicationModuleType.Broadcaster);

        for (var i = 1; ; i++)
        {
            var pulsesToSend = new Queue<PulseToSend>();
            pulsesToSend.Enqueue(new PulseToSend(PulseType.Low, "button", this.broadcasterModule));

            while (pulsesToSend.Count > 0)
            {
                var (pulseType, sourceModuleName, destinationModule) = pulsesToSend.Dequeue();

                var newPulsesToSend = destinationModule.ReceivePulse(pulseType, sourceModuleName);
                foreach (var newPulseToSend in newPulsesToSend)
                {
                    pulsesToSend.Enqueue(newPulseToSend);

                    if (newPulseToSend.PulseType == targetPulseType && modules.Contains(newPulseToSend.DestinationModule))
                    {
                        result.Add(i);

                        if (result.Count == modules.Count)
                        {
                            return result;
                        }
                    }
                }
            }
        }
    }
}
