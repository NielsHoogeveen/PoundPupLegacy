namespace PoundPupLegacy.CreateModel;

public sealed record NewTermForNewNameable: EventuallyIdentifiableTermForNewNameable
{
    public int? Id { get; set; } = null;
    public required int VocabularyId { get; init; }

    public required string Name { get; init; }

    public required List<int> ParentTermIds { get; init; }

    public EventuallyIdentifiableTermForExistingNameable ResolveNameable(int nameableId)
    {
        return new NewTermForExistingNameable {
            Name = Name,
            Id = null,
            VocabularyId = VocabularyId,
            NameableId = nameableId,
            ParentTermIds = ParentTermIds
        };
    }
}
public sealed record NewTermForExistingNameable : EventuallyIdentifiableTermForExistingNameable
{
    public required int? Id { get; set; }
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
    public required int NameableId { get; init; }

    public required List<int> ParentTermIds { get; init; }
}

public sealed record ExistingTerm : ImmediatelyIdentifiableTerm
{
    public required int Id { get; init; }
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
    public required int NameableId { get; init; }

    public required List<int> ParentTermIds { get; init; }
}
public interface ImmediatelyIdentifiableTerm : Term, ImmediatelyIdentifiable
{
    int NameableId { get; }
}
public interface EventuallyIdentifiableTermForNewNameable : EventuallyIdentifiableTerm
{
    public EventuallyIdentifiableTermForExistingNameable ResolveNameable(int nameableId);
}

public interface EventuallyIdentifiableTermForExistingNameable : EventuallyIdentifiableTerm
{
    int NameableId { get; }
}

public interface EventuallyIdentifiableTerm : Term, EventuallyIdentifiable
{
}

public interface Term: IRequest
{
    int VocabularyId { get; }
    string Name { get; }
    List<int> ParentTermIds {get;}
}