namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableDocumentable : Documentable, ImmediatelyIdentifiableSearchable
{
}

public interface EventuallyIdentifiableDocumentable : Documentable, EventuallyIdentifiableSearchable
{
}

public interface Documentable : Searchable
{
}
