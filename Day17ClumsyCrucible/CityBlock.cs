namespace Day17ClumsyCrucible;

public record CityBlock(CityBlockLocation Location, int AccumulatedHeatLoss, MoveDirection MoveDirection, int CurrentBlocksInDirection)
{
    public int EstimatedDistanceToDestination { get; private set; }

    public int Score => this.AccumulatedHeatLoss + this.EstimatedDistanceToDestination;

    public CityBlock? Parent { get; set; }

    public void SetEstimatedDistanceToDestination(CityBlockLocation destination)
    {
        this.EstimatedDistanceToDestination =
            Math.Abs(destination.Row - this.Location.Row) + Math.Abs(destination.Col - this.Location.Col);
    }
}
