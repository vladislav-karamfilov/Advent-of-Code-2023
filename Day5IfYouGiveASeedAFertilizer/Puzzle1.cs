namespace Day5IfYouGiveASeedAFertilizer;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/5
    public static void Solve()
    {
        List<long> seedsToPlant = null!;
        var seedToSoilMap = new Dictionary<Range, Range>();
        var soilToFertilizerMap = new Dictionary<Range, Range>();
        var fertilizerToWaterMap = new Dictionary<Range, Range>();
        var waterToLightMap = new Dictionary<Range, Range>();
        var lightToTemperatureMap = new Dictionary<Range, Range>();
        var temperatureToHumidityMap = new Dictionary<Range, Range>();
        var humidityToLocationMap = new Dictionary<Range, Range>();
        Dictionary<Range, Range> currentMap = null!;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                if (humidityToLocationMap.Count != 0)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            if (seedsToPlant is null)
            {
                seedsToPlant = ParseSeedsToPlant(line);
                continue;
            }

            if (line == "seed-to-soil map:")
            {
                currentMap = seedToSoilMap;
                continue;
            }
            else if (line == "soil-to-fertilizer map:")
            {
                currentMap = soilToFertilizerMap;
                continue;
            }
            else if (line == "fertilizer-to-water map:")
            {
                currentMap = fertilizerToWaterMap;
                continue;
            }
            else if (line == "water-to-light map:")
            {
                currentMap = waterToLightMap;
                continue;
            }
            else if (line == "light-to-temperature map:")
            {
                currentMap = lightToTemperatureMap;
                continue;
            }
            else if (line == "temperature-to-humidity map:")
            {
                currentMap = temperatureToHumidityMap;
                continue;
            }
            else if (line == "humidity-to-location map:")
            {
                currentMap = humidityToLocationMap;
                continue;
            }

            var rangeParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            var destinationRangeStart = rangeParts[0];
            var sourceRangeStart = rangeParts[1];
            var rangeLength = rangeParts[2];

            var sourceRange = new Range(sourceRangeStart, sourceRangeStart + rangeLength - 1);
            var destinationRange = new Range(destinationRangeStart, destinationRangeStart + rangeLength - 1);
            currentMap[sourceRange] = destinationRange;
        }

        var minLocation = long.MaxValue;
        foreach (var seed in seedsToPlant)
        {
            var currentLocation = CalculateNewLocation(seedToSoilMap, seed);
            currentLocation = CalculateNewLocation(soilToFertilizerMap, currentLocation);
            currentLocation = CalculateNewLocation(fertilizerToWaterMap, currentLocation);
            currentLocation = CalculateNewLocation(waterToLightMap, currentLocation);
            currentLocation = CalculateNewLocation(lightToTemperatureMap, currentLocation);
            currentLocation = CalculateNewLocation(temperatureToHumidityMap, currentLocation);
            currentLocation = CalculateNewLocation(humidityToLocationMap, currentLocation);

            if (currentLocation < minLocation)
            {
                minLocation = currentLocation;
            }
        }

        Console.WriteLine(minLocation);
    }

    private static long CalculateNewLocation(Dictionary<Range, Range> currentRangesMap, long currentLocation)
    {
        var (sourceRange, destinationRange) = currentRangesMap.FirstOrDefault(kv => kv.Key.IsInRange(currentLocation));
        if (sourceRange is null)
        {
            return currentLocation;
        }

        var offset = currentLocation - sourceRange.Start;
        return destinationRange.Start + offset;
    }

    private static List<long> ParseSeedsToPlant(string seedsLine)
    {
        var seedsToPlantStartIndex = seedsLine.IndexOf(':') + 1;
        return seedsLine[seedsToPlantStartIndex..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
    }
}
