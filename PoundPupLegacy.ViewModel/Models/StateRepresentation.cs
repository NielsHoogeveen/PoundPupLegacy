namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(StateRepresentation))]
public partial class StateRepresentationJsonContext : JsonSerializerContext { }

public record StateRepresentation
{
    public required BasicLink State { get; set; }

    private MemberOfCongress[] members = Array.Empty<MemberOfCongress>();
    public required MemberOfCongress[] Members {
        get => members;
        init {
            if (value is not null) {
                members = value;
            }
        }
    }
}

