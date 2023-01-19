namespace PoundPupLegacy.ViewModel;

public record struct AdoptionImport
{
    public string CountryFrom { get; set; }

    public int RowType { get; set; }
    public AdoptionImportValue[] Values { get; set; }
}
