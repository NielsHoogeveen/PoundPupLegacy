namespace PoundPupLegacy.CreateModel;

public sealed record NewTermForNewNameable: TermToCreateForNewNameable
{
    public required Identification.Possible Identification { get; init; }
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
    public required List<int> ParentTermIds { get; init; }
    public TermToCreateForExistingNameable ResolveNameable(int nameableId)
    {
        return new NewTermForExistingNameable {
            Identification = Identification,
            Name = Name,
            VocabularyId = VocabularyId,
            NameableId = nameableId,
            ParentTermIds = ParentTermIds
        };
    }
}
public sealed record NewTermForExistingNameable : TermToCreateForExistingNameable
{
    public required Identification.Possible Identification { get; init; }
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
    public required int NameableId { get; init; }
    public required List<int> ParentTermIds { get; init; }
}

public sealed record ExistingTerm : TermToUpdate
{
    public required Identification.Certain Identification { get; init; }
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
    public required int NameableId { get; init; }

    public required List<int> ParentTermIds { get; init; }
}
public interface TermToUpdate : Term, CertainlyIdentifiable
{
    int NameableId { get; }
}
public interface TermToCreateForNewNameable : TermToCreate
{
    public TermToCreateForExistingNameable ResolveNameable(int nameableId);
}

public interface TermToCreateForExistingNameable : TermToCreate
{
    int NameableId { get; }
}

public interface TermToCreate : Term, PossiblyIdentifiable
{
}

public interface Term: IRequest
{
    int VocabularyId { get; }
    string Name { get; }
    List<int> ParentTermIds {get;}
}