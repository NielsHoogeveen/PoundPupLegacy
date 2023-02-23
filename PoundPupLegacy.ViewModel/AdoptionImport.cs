namespace PoundPupLegacy.ViewModel;

public record AdoptionImport
{
    public required string CountryFrom { get; init; }

    public required int RowType { get; init; }

    private AdoptionImportValue[] values = Array.Empty<AdoptionImportValue>();
    public required AdoptionImportValue[] Values {
        get => values;
        init {
            if (values is not null) {
                values = value;
            }
        }
    }
}
