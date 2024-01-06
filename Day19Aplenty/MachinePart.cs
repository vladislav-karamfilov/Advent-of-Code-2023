namespace Day19Aplenty;

public class MachinePart
{
    public int ShinyRating { get; set; }

    public int AerodynamicRating { get; set; }

    public int MusicalRating { get; set; }

    public int ExtremelyCoolLookingRating { get; set; }

    public int TotalRating => this.ShinyRating + this.AerodynamicRating + this.MusicalRating + this.ExtremelyCoolLookingRating;

    public MachinePartStatus Status { get; set; }
}
