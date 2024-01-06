namespace Day19Aplenty;

public readonly record struct Range(int Start, int End)
{
    public int Length => this.End - this.Start + 1;
}
