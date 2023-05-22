namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableLocatable : Locatable, ImmediatelyIdentifiableSearchable
{
}

public interface EventuallyIdentifiableLocatable: Locatable, EventuallyIdentifiableSearchable
{
}

public interface Locatable : Searchable
{
}
