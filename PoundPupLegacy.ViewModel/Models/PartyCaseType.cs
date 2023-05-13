namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PartyCaseType))]
public partial class PartyCaseTypeJsonContext : JsonSerializerContext { }

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
