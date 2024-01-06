namespace Day3GearRatios;

using System;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/3#part2
    public static void Solve()
    {
        var engineSchematic = ReadEngineSchematic();

        var sum = CalculateSumOfGearRatios(engineSchematic);

        Console.WriteLine(sum);
    }

    private static long CalculateSumOfGearRatios(List<string> engineSchematic)
    {
        var sum = 0L;
        for (var y = 0; y < engineSchematic.Count; y++)
        {
            var engineSchematicLine = engineSchematic[y];
            for (var x = 0; x < engineSchematicLine.Length; x++)
            {
                if (engineSchematicLine[x] == '*')
                {
                    var adjacentPartNumbers = GetAdjacentPartNumberCoords(x, y, engineSchematic);
                    if (adjacentPartNumbers.Count == 2)
                    {
                        var (line, startIndex, endIndex) = adjacentPartNumbers.First();
                        var firstPartNumber = int.Parse(engineSchematic[line].AsSpan()[startIndex..(endIndex + 1)]);

                        (line, startIndex, endIndex) = adjacentPartNumbers.Last();
                        var secondPartNumber = int.Parse(engineSchematic[line].AsSpan()[startIndex..(endIndex + 1)]);

                        sum += firstPartNumber * secondPartNumber;
                    }
                }
            }
        }

        return sum;
    }

    private static HashSet<(int Line, int StartIndex, int EndIndex)> GetAdjacentPartNumberCoords(
        int potentialGearX,
        int potentialGearY,
        List<string> engineSchematic)
    {
        var result = new HashSet<(int Line, int StartIndex, int EndIndex)>();

        // Horizontal checks
        var engineSchematicLine = engineSchematic[potentialGearY];
        if (potentialGearX > 0 && char.IsDigit(engineSchematicLine[potentialGearX - 1]))
        {
            var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX - 1, engineSchematicLine);

            if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY))
            {
                result.Add((potentialGearY, numberStartIndex, numberEndIndex));
            }
        }

        if (potentialGearX < engineSchematicLine.Length - 1 && char.IsDigit(engineSchematicLine[potentialGearX + 1]))
        {
            var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX + 1, engineSchematicLine);

            if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY))
            {
                result.Add((potentialGearY, numberStartIndex, numberEndIndex));
            }
        }

        // Vertical checks
        var engineSchematicLineAbove = potentialGearY > 0 ? engineSchematic[potentialGearY - 1] : null;
        var engineSchematicLineBelow = potentialGearY < engineSchematic.Count - 1 ? engineSchematic[potentialGearY + 1] : null;

        if (engineSchematicLineAbove is not null && char.IsDigit(engineSchematicLineAbove[potentialGearX]))
        {
            var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX, engineSchematicLineAbove);

            if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY - 1))
            {
                result.Add((potentialGearY - 1, numberStartIndex, numberEndIndex));
            }
        }

        if (engineSchematicLineBelow is not null && char.IsDigit(engineSchematicLineBelow[potentialGearX]))
        {
            var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX, engineSchematicLineBelow);

            if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY + 1))
            {
                result.Add((potentialGearY + 1, numberStartIndex, numberEndIndex));
            }
        }

        // Diagonal checks
        if (engineSchematicLineAbove is not null)
        {
            if (potentialGearX > 0 && char.IsDigit(engineSchematicLineAbove[potentialGearX - 1]))
            {
                var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX - 1, engineSchematicLineAbove);

                if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY - 1))
                {
                    result.Add((potentialGearY - 1, numberStartIndex, numberEndIndex));
                }
            }

            if (potentialGearX < engineSchematicLineAbove.Length - 1 && char.IsDigit(engineSchematicLineAbove[potentialGearX + 1]))
            {
                var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX + 1, engineSchematicLineAbove);

                if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY - 1))
                {
                    result.Add((potentialGearY - 1, numberStartIndex, numberEndIndex));
                }
            }
        }

        if (engineSchematicLineBelow is not null)
        {
            if (potentialGearX > 0 && char.IsDigit(engineSchematicLineBelow[potentialGearX - 1]))
            {
                var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX - 1, engineSchematicLineBelow);

                if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY + 1))
                {
                    result.Add((potentialGearY + 1, numberStartIndex, numberEndIndex));
                }
            }

            if (potentialGearX < engineSchematicLineBelow.Length - 1 && char.IsDigit(engineSchematicLineBelow[potentialGearX + 1]))
            {
                var (numberStartIndex, numberEndIndex) = GetNumberStartAndEndIndices(potentialGearX + 1, engineSchematicLineBelow);

                if (IsPartNumber(numberStartIndex, numberEndIndex, engineSchematic, potentialGearY + 1))
                {
                    result.Add((potentialGearY + 1, numberStartIndex, numberEndIndex));
                }
            }
        }

        return result;
    }

    private static (int NumberStartIndex, int NumberEndIndex) GetNumberStartAndEndIndices(int digitIndex, string line)
    {
        var potentialNumberStartIndex = digitIndex;
        var potentialNumberEndIndex = digitIndex;

        while (potentialNumberStartIndex > 0 && char.IsDigit(line[potentialNumberStartIndex - 1]))
        {
            potentialNumberStartIndex--;
        }

        while (potentialNumberEndIndex < line.Length - 1 && char.IsDigit(line[potentialNumberEndIndex + 1]))
        {
            potentialNumberEndIndex++;
        }

        return (potentialNumberStartIndex, potentialNumberEndIndex);
    }

    private static List<string> ReadEngineSchematic()
    {
        var engineSchematic = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            engineSchematic.Add(line);
        }

        return engineSchematic;
    }

    private static bool IsPartNumber(int numberStartIndex, int numberEndIndex, List<string> engineSchematic, int y)
    {
        const char NonSymbol = '.';

        if (numberStartIndex < 0 || numberEndIndex < 0)
        {
            return false;
        }

        var engineSchematicLine = engineSchematic[y];

        // Horizontal checks
        if (numberStartIndex > 0 &&
            engineSchematicLine[numberStartIndex - 1] != NonSymbol)
        {
            return true;
        }

        if (numberEndIndex < engineSchematicLine.Length - 1 &&
            engineSchematicLine[numberEndIndex + 1] != NonSymbol)
        {
            return true;
        }

        var engineSchematicLineAbove = y > 0 ? engineSchematic[y - 1] : null;
        var engineSchematicLineBelow = y < engineSchematic.Count - 1 ? engineSchematic[y + 1] : null;

        // Vertical checks
        for (var x = numberStartIndex; x <= numberEndIndex; x++)
        {
            if (engineSchematicLineAbove is not null && engineSchematicLineAbove[x] != NonSymbol)
            {
                return true;
            }

            if (engineSchematicLineBelow is not null && engineSchematicLineBelow[x] != NonSymbol)
            {
                return true;
            }
        }

        // Diagonal checks
        if (engineSchematicLineAbove is not null)
        {
            if (numberStartIndex > 0 &&
                engineSchematicLineAbove[numberStartIndex - 1] != NonSymbol)
            {
                return true;
            }

            if (numberEndIndex < engineSchematicLine.Length - 1 &&
                engineSchematicLineAbove[numberEndIndex + 1] != NonSymbol)
            {
                return true;
            }
        }

        if (engineSchematicLineBelow is not null)
        {
            if (numberStartIndex > 0 &&
                engineSchematicLineBelow[numberStartIndex - 1] != NonSymbol)
            {
                return true;
            }

            if (numberEndIndex < engineSchematicLine.Length - 1 &&
                engineSchematicLineBelow[numberEndIndex + 1] != NonSymbol)
            {
                return true;
            }
        }

        return false;
    }
}
