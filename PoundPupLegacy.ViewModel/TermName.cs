namespace PoundPupLegacy.ViewModel;

public record TermName
{
    public int Id { get; set; }

    public string Path { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }
}
