namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PartyCase))]
public partial class PartyCaseJsonContext : JsonSerializerContext { }

public record PartyCase
{
    public required string CasePartyTypeName { get; init; }

    private BasicLink[] _cases = Array.Empty<BasicLink>();
    public required BasicLink[] Cases {
        get => _cases;
        init {
            if (value is not null) {
                _cases = value;
            }
        }
    }
}
