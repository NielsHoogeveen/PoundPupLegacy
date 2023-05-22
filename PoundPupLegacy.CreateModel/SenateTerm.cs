namespace PoundPupLegacy.CreateModel;

public sealed record NewSenateTerm : NewCongressionalTermBase, EventuallyIdentifiableSenateTerm
{
    public required int? SenatorId { get; set; }
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
public sealed record ExistingSenateTerm : ExistingCongressionalTermBase, ImmediatelyIdentifiableSenateTerm
{
    public required int? SenatorId { get; set; }
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
public interface ImmediatelyIdentifiableSenateTerm : SenateTerm, ImmediatelyIdentifiableCongressionalTerm
{
}
public interface EventuallyIdentifiableSenateTerm : SenateTerm, EventuallyIdentifiableCongressionalTerm
{
}
public interface SenateTerm : CongressionalTerm
{
    int? SenatorId { get; }
    int SubdivisionId { get; }
    DateTimeRange DateTimeRange { get; }

}
