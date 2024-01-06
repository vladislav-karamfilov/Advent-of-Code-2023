namespace Day16TheFloorWillBeLava;

using System;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/16
    public static void Solve()
    {
        var contraptionLayout = PuzzleHelper.ReadContraptionLayout();

        var energizedTiles = PuzzleHelper.CalculateEnergizedTiles(
            contraptionLayout,
            startLocation: new TileLocation(0, 0),
            startBeamDirection: BeamDirection.Right);

        Console.WriteLine(energizedTiles);
    }
}
