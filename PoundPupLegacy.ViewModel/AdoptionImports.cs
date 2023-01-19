namespace PoundPupLegacy.ViewModel;

public record struct AdoptionImports
{
    public int StartYear { get; set; }

    public int EndYear { get; set; }

    public AdoptionImport[] Imports { get; set; }
}
