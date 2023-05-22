namespace PoundPupLegacy.CreateModel;

public sealed record NewDocumentType : NewNameableBase, EventuallyIdentifiableDocumentType
{
}
public sealed record ExistingDocumentType : ExistingNameableBase, ImmediatelyIdentifiableDocumentType
{
}
public interface ImmediatelyIdentifiableDocumentType : DocumentType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableDocumentType : DocumentType, EventuallyIdentifiableNameable
{
}
public interface DocumentType : Nameable
{
}
