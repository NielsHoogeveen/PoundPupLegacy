namespace PoundPupLegacy.CreateModel;
public interface BillToUpdate : Bill, NameableToUpdate, DocumentableToUpdate
{
}
public interface BillToCreate: Bill, NameableToCreate, DocumentableToCreate
{
}

public interface Bill : Nameable, Documentable
{
    BillDetails BillDetails { get; }

}

public sealed record BillDetails
{
    public required DateTime? IntroductionDate { get; init; }

    public required int? ActId { get; init; }

}
