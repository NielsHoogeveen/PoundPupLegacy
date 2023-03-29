namespace PoundPupLegacy.CreateModel;

public interface Case : Locatable, Documentable, Nameable
{
    public DateTimeRange? Date { get; }

}
