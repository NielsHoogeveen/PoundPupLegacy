namespace PoundPupLegacy.Model;

public record Senator : MemberOfCongress
{
    public required int? Id { get; set; }

    public required int? PersonId { get; set; }

    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }


}