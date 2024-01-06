namespace Day16TheFloorWillBeLava;

internal static class PuzzleHelper
{
    public static List<string> ReadContraptionLayout()
    {
        var result = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            result.Add(line);
        }

        return result;
    }

    public static int CalculateEnergizedTiles(List<string> contraptionLayout, TileLocation startLocation, BeamDirection startBeamDirection)
    {
        var visitedTilesDuringBeamMovement = new HashSet<(TileLocation TileLocation, BeamDirection BeamDirection)>();

        EnergizeTiles(
            contraptionLayout,
            startLocation,
            startBeamDirection,
            visitedTilesDuringBeamMovement);

        return visitedTilesDuringBeamMovement.Select(x => x.TileLocation).Distinct().Count();
    }

    private static void EnergizeTiles(
        List<string> contraptionLayout,
        TileLocation startLocation,
        BeamDirection startBeamDirection,
        HashSet<(TileLocation TileLocation, BeamDirection BeamDirection)> visitedTilesDuringBeamMovement)
    {
        var currentLocation = startLocation;
        var currentBeamDirection = startBeamDirection;
        while (IsTileInContraptionLayout(contraptionLayout, currentLocation) &&
            visitedTilesDuringBeamMovement.Add((currentLocation, currentBeamDirection))) // Loop detected
        {
            var currentTile = contraptionLayout[currentLocation.Row][currentLocation.Col];
            if (currentTile == '.' ||
                (currentTile == '-' && (currentBeamDirection == BeamDirection.Right || currentBeamDirection == BeamDirection.Left)) ||
                (currentTile == '|' && (currentBeamDirection == BeamDirection.Up || currentBeamDirection == BeamDirection.Down)))
            {
                if (currentBeamDirection == BeamDirection.Right)
                {
                    currentLocation = new TileLocation(currentLocation.Row, currentLocation.Col + 1);
                }
                else if (currentBeamDirection == BeamDirection.Left)
                {
                    currentLocation = new TileLocation(currentLocation.Row, currentLocation.Col - 1);
                }
                else if (currentBeamDirection == BeamDirection.Up)
                {
                    currentLocation = new TileLocation(currentLocation.Row - 1, currentLocation.Col);
                }
                else // if (currentBeamDirection == BeamDirection.Down)
                {
                    currentLocation = new TileLocation(currentLocation.Row + 1, currentLocation.Col);
                }
            }
            else if (currentTile == '/')
            {
                if (currentBeamDirection == BeamDirection.Right)
                {
                    currentLocation = new TileLocation(currentLocation.Row - 1, currentLocation.Col);
                    currentBeamDirection = BeamDirection.Up;
                }
                else if (currentBeamDirection == BeamDirection.Left)
                {
                    currentLocation = new TileLocation(currentLocation.Row + 1, currentLocation.Col);
                    currentBeamDirection = BeamDirection.Down;
                }
                else if (currentBeamDirection == BeamDirection.Up)
                {
                    currentLocation = new TileLocation(currentLocation.Row, currentLocation.Col + 1);
                    currentBeamDirection = BeamDirection.Right;
                }
                else // if (currentBeamDirection == BeamDirection.Down)
                {
                    currentLocation = new TileLocation(currentLocation.Row, currentLocation.Col - 1);
                    currentBeamDirection = BeamDirection.Left;
                }
            }
            else if (currentTile == '\\')
            {
                if (currentBeamDirection == BeamDirection.Right)
                {
                    currentLocation = new TileLocation(currentLocation.Row + 1, currentLocation.Col);
                    currentBeamDirection = BeamDirection.Down;
                }
                else if (currentBeamDirection == BeamDirection.Left)
                {
                    currentLocation = new TileLocation(currentLocation.Row - 1, currentLocation.Col);
                    currentBeamDirection = BeamDirection.Up;
                }
                else if (currentBeamDirection == BeamDirection.Up)
                {
                    currentLocation = new TileLocation(currentLocation.Row, currentLocation.Col - 1);
                    currentBeamDirection = BeamDirection.Left;
                }
                else // if (currentBeamDirection == BeamDirection.Down)
                {
                    currentLocation = new TileLocation(currentLocation.Row, currentLocation.Col + 1);
                    currentBeamDirection = BeamDirection.Right;
                }
            }
            else if (currentTile == '-')
            {
                EnergizeTiles(
                    contraptionLayout,
                    new TileLocation(currentLocation.Row, currentLocation.Col - 1),
                    BeamDirection.Left,
                    visitedTilesDuringBeamMovement);

                EnergizeTiles(
                    contraptionLayout,
                    new TileLocation(currentLocation.Row, currentLocation.Col + 1),
                    BeamDirection.Right,
                    visitedTilesDuringBeamMovement);
            }
            else // if (currentTile == '|')
            {
                EnergizeTiles(
                    contraptionLayout,
                    new TileLocation(currentLocation.Row - 1, currentLocation.Col),
                    BeamDirection.Up,
                    visitedTilesDuringBeamMovement);

                EnergizeTiles(
                    contraptionLayout,
                    new TileLocation(currentLocation.Row + 1, currentLocation.Col),
                    BeamDirection.Down,
                    visitedTilesDuringBeamMovement);
            }
        }
    }

    private static bool IsTileInContraptionLayout(List<string> contraptionLayout, TileLocation tileLocation)
        => tileLocation.Row >= 0 &&
            tileLocation.Col >= 0 &&
            tileLocation.Row < contraptionLayout.Count &&
            tileLocation.Col < contraptionLayout[0].Length;
}
