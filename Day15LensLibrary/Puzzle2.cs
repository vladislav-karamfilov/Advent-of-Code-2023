namespace Day15LensLibrary;

using System;
using System.Buffers;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/15#part2
    public static void Solve()
    {
        var initializationSequence = Console.ReadLine(); // File.ReadLines("input.txt").FirstOrDefault();

        var focusingPower = 0;
        if (string.IsNullOrWhiteSpace(initializationSequence))
        {
            Console.WriteLine(focusingPower);
            return;
        }

        var boxes = new Box[256];

        PerformOperationsOnBoxes(initializationSequence, boxes);

        focusingPower = CalculateFocusingPower(boxes);

        Console.WriteLine(focusingPower);
    }

    private static int CalculateFocusingPower(Box[] boxes)
    {
        var focusingPower = 0;
        for (var boxNumber = 0; boxNumber < boxes.Length; boxNumber++)
        {
            var box = boxes[boxNumber];
            for (var i = 0; i < box?.Lenses.Count; i++)
            {
                var lens = box.Lenses[i];
                var lensFocusingPower = 1 + boxNumber;
                lensFocusingPower *= i + 1;
                lensFocusingPower *= lens.FocalLength;

                focusingPower += lensFocusingPower;
            }
        }

        return focusingPower;
    }

    private static void PerformOperationsOnBoxes(string initializationSequence, Box[] boxes)
    {
        var operationCharacterSearchValues = SearchValues.Create("=-");

        var steps = initializationSequence.Split(',');
        foreach (var step in steps)
        {
            var operationIndex = step.AsSpan().IndexOfAny(operationCharacterSearchValues);
            var lensLabel = step[..operationIndex];
            var operation = step[operationIndex];

            var boxNumber = PuzzleHelper.CalculateHashValue(lensLabel);
            boxes[boxNumber] ??= new Box();
            var box = boxes[boxNumber];

            if (operation == '-')
            {
                box.RemoveLens(lensLabel);
            }
            else
            {
                var lensFocalLength = int.Parse(step.AsSpan()[(operationIndex + 1)..]);
                box.AddOrUpdateLens(new Lens { Label = lensLabel, FocalLength = lensFocalLength });
            }
        }
    }
}
