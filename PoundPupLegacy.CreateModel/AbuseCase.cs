namespace PoundPupLegacy.CreateModel;

public sealed record NewAbuseCase : NewCaseBase, EventuallyIdentifiableAbuseCase
{
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required List<int> TypeOfAbuseIds { get; init; }
    public required List<int> TypeOfAbuserIds { get; init; }
}
public sealed record ExistingAbuseCase : ExistingCaseBase, ImmediatelyIdentifiableAbuseCase
{
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required List<int> TypeOfAbuseIds { get; init; }
    public required List<int> TypeOfAbuserIds { get; init; }
}

public interface ImmediatelyIdentifiableAbuseCase : AbuseCase, ImmediatelyIdentifiableCase
{
}

public interface EventuallyIdentifiableAbuseCase: AbuseCase, EventuallyIdentifiableCase
{
}
public interface AbuseCase: Case
{
    int ChildPlacementTypeId { get; }
    int? FamilySizeId { get; }
    bool? HomeschoolingInvolved { get; }
    bool? FundamentalFaithInvolved { get; }
    bool? DisabilitiesInvolved { get; }
    List<int> TypeOfAbuseIds { get;  }
    List<int> TypeOfAbuserIds { get; }

}

