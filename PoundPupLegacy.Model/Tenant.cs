namespace PoundPupLegacy.Model;

public record Tenant: Owner
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string DomainName { get; init; }
    public required int? VocabularyIdTagging { get; init; }
}
