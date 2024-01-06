namespace Day5IfYouGiveASeedAFertilizer;

using System.Collections.Concurrent;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/5#part2
    public static void Solve()
    {
        List<Range> seedRangesToPlant = null!;
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

            if (seedRangesToPlant is null)
            {
                seedRangesToPlant = ParseSeedRangesToPlant(line);
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

        var minLocationsForRanges = new ConcurrentBag<long>();

        Parallel.ForEach(
            seedRangesToPlant,
            new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 },
            seedRange =>
            {
                var minLocation = long.MaxValue;
                Range? lastSeedSourceRange = null;
                Range? lastSoilSourceRange = null;
                Range? lastFertilizerSourceRange = null;
                Range? lastWaterSourceRange = null;
                Range? lastLightSourceRange = null;
                Range? lastTemperatureSourceRange = null;
                Range? lastHumiditySourceRange = null;
                for (var i = seedRange.Start; i <= seedRange.End; i++)
                {
                    (var currentLocation, lastSeedSourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        seedToSoilMap,
                        i,
                        lastSeedSourceRange);

                    (currentLocation, lastSoilSourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        soilToFertilizerMap,
                        currentLocation,
                        lastSoilSourceRange);

                    (currentLocation, lastFertilizerSourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        fertilizerToWaterMap,
                        currentLocation,
                        lastFertilizerSourceRange);

                    (currentLocation, lastWaterSourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        waterToLightMap,
                        currentLocation,
                        lastWaterSourceRange);

                    (currentLocation, lastLightSourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        lightToTemperatureMap,
                        currentLocation,
                        lastLightSourceRange);

                    (currentLocation, lastTemperatureSourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        temperatureToHumidityMap,
                        currentLocation,
                        lastTemperatureSourceRange);

                    (currentLocation, lastHumiditySourceRange) = CalculateNewLocationAndMatchedSourceRange(
                        humidityToLocationMap,
                        currentLocation,
                        lastHumiditySourceRange);

                    if (currentLocation < minLocation)
                    {
                        minLocation = currentLocation;
                    }
                }

                minLocationsForRanges.Add(minLocation);
            });

        Console.WriteLine(minLocationsForRanges.Min());
    }

    private static (long NewLocation, Range? SourceRange) CalculateNewLocationAndMatchedSourceRange(
        Dictionary<Range, Range> currentRangesMap,
        long currentLocation,
        Range? lastSourceRange)
    {
        Range? sourceRange = null;
        Range? destinationRange = null;
        if (lastSourceRange?.IsInRange(currentLocation) == true)
        {
            sourceRange = lastSourceRange;
            destinationRange = currentRangesMap[sourceRange];
        }
        else
        {
            (sourceRange, destinationRange) = currentRangesMap.FirstOrDefault(kv => kv.Key.IsInRange(currentLocation));
            if (sourceRange is null)
            {
                return (currentLocation, null);
            }
        }

        var offset = currentLocation - sourceRange.Start;
        var destinationLocation = destinationRange.Start + offset;
        return (destinationLocation, sourceRange);
    }

    private static List<Range> ParseSeedRangesToPlant(string seedsLine)
    {
        var seedsToPlantStartIndex = seedsLine.IndexOf(':') + 1;
        var seedRangesParts = seedsLine[seedsToPlantStartIndex..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        var result = new List<Range>(seedRangesParts.Count);
        for (var i = 0; i < seedRangesParts.Count; i += 2)
        {
            var start = seedRangesParts[i];
            var length = seedRangesParts[i + 1];
            result.Add(new Range(start, start + length - 1));
        }

        return result;
    }
}
