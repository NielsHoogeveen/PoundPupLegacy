namespace PoundPupLegacy.EditModel;

public record Tag
{
    public required int? NodeId { get; set; }

    public required int TermId { get; init; }

    public required string Name { get; init; }

    public bool IsStored { get; set; } = true;

    public bool HasBeenDeleted { get; set; } = false;

    public void SetToDeleted()
    {
        HasBeenDeleted = true;
    }
}
