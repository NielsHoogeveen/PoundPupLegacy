namespace PoundPupLegacy.ViewModel;

public record AdoptionImport
{
    public string CountryFrom { get; set; }

    public int RowType { get; set; }
    public AdoptionImportValue[] Values { get; set; }
}
