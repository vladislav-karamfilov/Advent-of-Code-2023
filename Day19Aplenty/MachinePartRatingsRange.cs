namespace Day19Aplenty;

public record MachinePartRatingsRange(Range ExtremelyCoolLookingRange, Range MusicalRange, Range AerodynamicRange, Range ShinyRange)
{
    public long UniqueCombinations
        => (long)this.ExtremelyCoolLookingRange.Length * this.MusicalRange.Length * this.AerodynamicRange.Length * this.ShinyRange.Length;
}
