namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableGeographicalEntity : GeographicalEntity, ImmediatelyIdentifiableDocumentable, ImmediatelyIdentifiableNameable
{
}

public interface EventuallyIdentifiableGeographicalEntity: GeographicalEntity, EventuallyIdentifiableDocumentable, EventuallyIdentifiableNameable
{
}

public interface GeographicalEntity : Documentable, Nameable
{
}
