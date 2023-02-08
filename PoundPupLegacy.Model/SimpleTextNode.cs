namespace PoundPupLegacy.Model;

public interface SimpleTextNode: Searchable
{
    string Text { get; }

    string Teaser { get; }
}
