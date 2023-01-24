namespace PoundPupLegacy.ViewModel;

public record AdoptionImports
{
    public int StartYear { get; set; }

    public int EndYear { get; set; }

    public AdoptionImport[] Imports { get; set; }
}
