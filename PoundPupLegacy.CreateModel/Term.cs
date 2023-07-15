namespace PoundPupLegacy.DomainModel;

public abstract record Term
{
    private Term() { }
    public sealed record ToCreateForNewNameable : Term, PossiblyIdentifiable
    {
        public required Identification.Possible Identification { get; init; }
        public required int VocabularyId { get; init; }
        public required string Name { get; init; }
        public required List<int> ParentTermIds { get; init; }
        public ToCreateForExistingNameable ResolveNameable(int nameableId)
        {
            return new ToCreateForExistingNameable {
                Identification = Identification,
                Name = Name,
                VocabularyId = VocabularyId,
                NameableId = nameableId,
                ParentTermIds = ParentTermIds
            };
        }
    }
    public sealed record ToCreateForExistingNameable : Term, PossiblyIdentifiable
    {
        public required Identification.Possible Identification { get; init; }
        public required int VocabularyId { get; init; }
        public required string Name { get; init; }
        public required int NameableId { get; init; }
        public required List<int> ParentTermIds { get; init; }
    }

    public sealed record ToUpdate : Term, CertainlyIdentifiable
    {
        public required Identification.Certain Identification { get; init; }
        public required int VocabularyId { get; init; }
        public required string Name { get; init; }
        public required int NameableId { get; init; }
        public required List<int> ParentTermIds { get; init; }
    }

}