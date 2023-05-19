namespace PoundPupLegacy.ViewModel.Models;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(AbuseCase))]
public partial class AbuseCaseJsonContext : JsonSerializerContext { }
public sealed record AbuseCase : CaseBase
{
    public required BasicLink? ChildPlacementType { get; init; }

    public required BasicLink? FamilySize { get; init; }

    public required bool? HomeSchoolingInvolved { get; init; }

    public required bool? FundamentalFaithInvolved { get; init; }

    public required bool? DisabilitiesInvolved { get; init; }

    private BasicLink[] typesOfAbuse = Array.Empty<BasicLink>();
    public required BasicLink[] TypesOfAbuse {
        get => typesOfAbuse;
        init {
            if (value is not null) {
                typesOfAbuse = value;
            }
        }
    }

    private BasicLink[] typesOfAbuser = Array.Empty<BasicLink>();
    public required BasicLink[] TypesOfAbuser {
        get => typesOfAbuser;
        init {
            if (value is not null) {
                typesOfAbuser = value;
            }
        }
    }
}
