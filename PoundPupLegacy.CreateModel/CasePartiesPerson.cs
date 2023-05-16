namespace PoundPupLegacy.CreateModel;

public sealed record CasePartiesPerson : IRequest
{
    public required int CasePartiesId { get; init; }
    public required int PersonId { get; init; }
}
