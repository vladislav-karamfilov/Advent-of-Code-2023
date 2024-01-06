namespace Day3GearRatios;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/3
    public static void Solve()
    {
        var engineSchematic = ReadEngineSchematic();

        var sum = CalculateSumOfEnginePartNumbers(engineSchematic);

        Console.WriteLine(sum);
    }

    private static long CalculateSumOfEnginePartNumbers(List<string> engineSchematic)
    {
        var sum = 0L;
        for (var y = 0; y < engineSchematic.Count; y++)
        {
            var isInNumber = false;
            var potentialPartNumberStartIndex = -1;
            var potentialPartNumberEndIndex = -1;

            var engineSchematicLine = engineSchematic[y];
            for (var x = 0; x < engineSchematicLine.Length; x++)
            {
                var isDigit = char.IsDigit(engineSchematicLine[x]);
                if (isDigit)
                {
                    if (!isInNumber)
                    {
                        potentialPartNumberStartIndex = x;
                    }

                    isInNumber = true;
                }
                else
                {
                    if (isInNumber)
                    {
                        potentialPartNumberEndIndex = x;

                        if (IsPartNumber(potentialPartNumberStartIndex, potentialPartNumberEndIndex - 1, engineSchematic, y))
                        {
                            sum += int.Parse(engineSchematicLine.AsSpan()[potentialPartNumberStartIndex..potentialPartNumberEndIndex]);
                        }
                    }

                    isInNumber = false;
                    potentialPartNumberStartIndex = -1;
                    potentialPartNumberEndIndex = -1;
                }
            }

            if (isInNumber)
            {
                potentialPartNumberEndIndex = engineSchematicLine.Length;
            }

            if (IsPartNumber(potentialPartNumberStartIndex, potentialPartNumberEndIndex - 1, engineSchematic, y))
            {
                sum += int.Parse(engineSchematicLine.AsSpan()[potentialPartNumberStartIndex..potentialPartNumberEndIndex]);
            }
        }

        return sum;
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

        // Vertical checks
        var engineSchematicLineAbove = y > 0 ? engineSchematic[y - 1] : null;
        var engineSchematicLineBelow = y < engineSchematic.Count - 1 ? engineSchematic[y + 1] : null;

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
