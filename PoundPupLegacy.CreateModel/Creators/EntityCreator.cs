namespace PoundPupLegacy.CreateModel.Creators;

internal interface IEntityCreator<T>
{
    public abstract static Task CreateAsync(IAsyncEnumerable<T> elements, NpgsqlConnection connection);
}

internal static class EntityCreator
{
    internal static async Task WriteTerms(Nameable nameable, DatabaseWriter<Term> termWriter, TermReaderByName termReader, DatabaseWriter<TermHierarchy> termHierarchyWriter, VocabularyIdReaderByOwnerAndName vocabularyIdReader)
    {
        foreach (var vocabularyName in nameable.VocabularyNames) {
            var vocubularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.VocabularyIdReaderByOwnerAndNameRequest {
                OwnerId = vocabularyName.OwnerId,
                Name = vocabularyName.Name
            });
            var term = new Term {
                Name = vocabularyName.TermName,
                Id = null,
                VocabularyId = vocubularyId,
                NameableId = (int)nameable.Id!
            };
            await termWriter.WriteAsync(term);
            foreach (var parent in vocabularyName.ParentNames) {
                var parentTerm = await termReader.ReadAsync(new TermReaderByName.TermReaderByNameRequest {
                    Name = parent,
                    VocabularyId = vocubularyId
                });
                await termHierarchyWriter.WriteAsync(new TermHierarchy { TermIdPartent = (int)parentTerm.Id!, TermIdChild = (int)term.Id! });
            }
        }
    }

}