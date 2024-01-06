namespace Day15LensLibrary;

public class Box
{
    public List<Lens> Lenses { get; } = [];

    public void AddOrUpdateLens(Lens lens)
    {
        var lensIndex = this.Lenses.FindIndex(l => l.Label == lens.Label);
        if (lensIndex >= 0)
        {
            this.Lenses[lensIndex] = lens;
        }
        else
        {
            this.Lenses.Add(lens);
        }
    }

    public void RemoveLens(string lensLabel)
    {
        var lensIndex = this.Lenses.FindIndex(l => l.Label == lensLabel);
        if (lensIndex >= 0)
        {
            this.Lenses.RemoveAt(lensIndex);
        }
    }
}