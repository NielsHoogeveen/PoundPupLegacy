namespace PoundPupLegacy.CreateModel;

public interface ActToUpdate : Act, NameableToUpdate, DocumentableToUpdate
{
}

public interface ActToCreate: Act, NameableToCreate, DocumentableToCreate
{
}

public interface Act
{
    ActDetails ActDetails { get;}
}

public sealed record ActDetails
{
    public required DateTime? EnactmentDate { get; init; }
}
