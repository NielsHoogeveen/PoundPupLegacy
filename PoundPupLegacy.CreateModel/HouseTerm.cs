namespace PoundPupLegacy.CreateModel;

public sealed record NewHouseTerm : NewCongressionalTermBase, EventuallyIdentifiableHouseTerm
{
    public required int? RepresentativeId { get; set; }
    public required int SubdivisionId { get; init; }
    public required int? District { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
public sealed record ExistingHouseTerm : ExistingCongressionalTermBase, ImmediatelyIdentifiableHouseTerm
{
    public required int? RepresentativeId { get; init; }
    public required int SubdivisionId { get; init; }
    public required int? District { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
public interface ImmediatelyIdentifiableHouseTerm : HouseTerm, ImmediatelyIdentifiableCongressionalTerm
{
}
public interface EventuallyIdentifiableHouseTerm : HouseTerm,  EventuallyIdentifiableCongressionalTerm
{
}
public interface HouseTerm :CongressionalTerm
{
    int? RepresentativeId { get;  }
    int SubdivisionId { get;  }
    int? District { get; }
    DateTimeRange DateTimeRange { get; }
}
