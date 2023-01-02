namespace PoundPupLegacy.Model;

public interface Case : Locatable, Documentable, Nameable
{
    public DateTimeRange? Date { get; }
}
