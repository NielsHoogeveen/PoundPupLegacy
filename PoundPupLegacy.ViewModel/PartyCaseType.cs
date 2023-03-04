namespace PoundPupLegacy.ViewModel;

public record PartyCaseType
{
    public required string CaseTypeName { get; init; }

    private PartyCase[] partyCases = Array.Empty<PartyCase>();
    public required PartyCase[] PartyCases {
        get => partyCases;
        init {
            if (value is not null) {
                partyCases = value;
            }
        }
    }
}
