namespace PoundPupLegacy.ViewModel.Models;
public enum RowType
{
    Line = 1,
    Sub = 2,
    Total = 3
}
public record AdoptionImport
{
    public required string CountryFrom { get; init; }

    public required RowType RowType { get; init; }

    private AdoptionImportValue[] values = Array.Empty<AdoptionImportValue>();
    public required AdoptionImportValue[] Values
    {
        get => values;
        init
        {
            if (values is not null)
            {
                values = value;
            }
        }
    }
}
