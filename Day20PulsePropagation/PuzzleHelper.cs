namespace Day20PulsePropagation;

internal static class PuzzleHelper
{
    public static CommunicationSystem ReadCommunicationSystem(TextReader textReader)
    {
        var destinationModulesSeparators = new char[] { ',', ' ' };

        var modulesToAdd = new Dictionary<string, CommunicationModule>();
        var destinationModulesToAdd = new Dictionary<string, string[]>();

        while (true)
        {
            var line = textReader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var parts = line.Split(" -> ");

            var moduleType = parts[0];
            var destinationModules = parts[1].Split(destinationModulesSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (moduleType == "broadcaster")
            {
                destinationModulesToAdd[moduleType] = destinationModules;
                modulesToAdd[moduleType] = new BroadcasterCommunicationModule { Name = moduleType };
                continue;
            }

            var name = moduleType[1..];
            destinationModulesToAdd[name] = destinationModules;
            modulesToAdd[name] = moduleType[0] == '%'
                ? new FlipFlopCommunicationModule { Name = name }
                : new ConjunctionCommunicationModule { Name = name };
        }

        foreach (var (moduleName, destinationModuleNames) in destinationModulesToAdd)
        {
            var module = modulesToAdd[moduleName];
            foreach (var destinationModuleName in destinationModuleNames)
            {
                if (modulesToAdd.TryGetValue(destinationModuleName, out var destinationModule))
                {
                    module.AddOutputModule(destinationModule);
                    destinationModule.AddInputModule(module);
                }
                else
                {
                    var untypedModule = new UntypedCommunicationModule { Name = destinationModuleName };
                    module.AddOutputModule(untypedModule);
                    untypedModule.AddInputModule(module);

                    modulesToAdd.Add(destinationModuleName, untypedModule);
                }
            }
        }

        var result = new CommunicationSystem();
        foreach (var (_, module) in modulesToAdd)
        {
            result.AddModule(module);
        }

        return result;
    }
}
