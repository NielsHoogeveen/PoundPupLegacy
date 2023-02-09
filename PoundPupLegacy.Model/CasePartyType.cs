namespace PoundPupLegacy.Model;

public record CasePartyType: Identifiable
{
    public int? Id { get; set; }
    public string Name { get; init; }

    public CasePartyType(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
