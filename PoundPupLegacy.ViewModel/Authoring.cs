namespace PoundPupLegacy.ViewModel;

public record Authoring
{
    public int Id { get; set; }
    public string Name { get; set; }

    public DateTime CreatedDateTime { get; set; }
    public DateTime ChangedDateTime { get; set; }
}
