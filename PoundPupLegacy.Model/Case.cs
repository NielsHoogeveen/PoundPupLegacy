namespace PoundPupLegacy.Model;

public interface Case : Locatable, Documentable
{
    public DateTimeRange? Date { get; }

    public string Description { get; }
}
