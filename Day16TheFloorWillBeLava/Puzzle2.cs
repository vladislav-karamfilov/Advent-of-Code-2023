namespace Day16TheFloorWillBeLava;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/16#part2
    public static async Task Solve()
    {
        var contraptionLayout = PuzzleHelper.ReadContraptionLayout();

        var maxEnergizedTiles = int.MinValue;

        var tasks = new Task[4];
        tasks[0] = Task.Run(() =>
        {
            for (var row = 0; row < contraptionLayout.Count; row++)
            {
                var energizedTiles = PuzzleHelper.CalculateEnergizedTiles(
                    contraptionLayout,
                    startLocation: new TileLocation(row, Col: 0),
                    startBeamDirection: BeamDirection.Right);

                InterlockedExchangeIfGreaterThan(ref maxEnergizedTiles, energizedTiles);
            }
        });

        tasks[1] = Task.Run(() =>
        {
            for (var row = 0; row < contraptionLayout.Count; row++)
            {
                var energizedTiles = PuzzleHelper.CalculateEnergizedTiles(
                    contraptionLayout,
                    startLocation: new TileLocation(row, Col: contraptionLayout[0].Length - 1),
                    startBeamDirection: BeamDirection.Left);

                InterlockedExchangeIfGreaterThan(ref maxEnergizedTiles, energizedTiles);
            }
        });

        tasks[2] = Task.Run(() =>
        {
            for (var col = 0; col < contraptionLayout[0].Length; col++)
            {
                var energizedTiles = PuzzleHelper.CalculateEnergizedTiles(
                    contraptionLayout,
                    startLocation: new TileLocation(Row: 0, col),
                    startBeamDirection: BeamDirection.Down);

                InterlockedExchangeIfGreaterThan(ref maxEnergizedTiles, energizedTiles);
            }
        });

        tasks[3] = Task.Run(() =>
        {
            for (var col = 0; col < contraptionLayout[0].Length; col++)
            {
                var energizedTiles = PuzzleHelper.CalculateEnergizedTiles(
                    contraptionLayout,
                    startLocation: new TileLocation(Row: contraptionLayout.Count - 1, col),
                    startBeamDirection: BeamDirection.Up);

                InterlockedExchangeIfGreaterThan(ref maxEnergizedTiles, energizedTiles);
            }
        });

        await Task.WhenAll(tasks);

        Console.WriteLine(maxEnergizedTiles);
    }

    private static bool InterlockedExchangeIfGreaterThan(ref int target, int newValue)
    {
        int snapshot;
        bool stillBigger;
        do
        {
            snapshot = target;
            stillBigger = newValue > snapshot;
        } while (stillBigger && Interlocked.CompareExchange(ref target, newValue, snapshot) != snapshot);

        return stillBigger;
    }
}
