namespace Day5IfYouGiveASeedAFertilizer;

public record Range(long Start, long End)
{
    public bool IsInRange(long location) => this.Start <= location && location <= this.End;
}
