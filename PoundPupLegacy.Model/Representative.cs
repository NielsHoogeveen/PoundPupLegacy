namespace PoundPupLegacy.Model;

public record Representative : MemberOfCongress
{
    public required int? Id { get; set; }

    public required int? PersonId { get; set; }

    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }


}