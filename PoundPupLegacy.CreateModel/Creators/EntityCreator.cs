namespace PoundPupLegacy.CreateModel.Creators;

public interface IEntityCreator<T>
{
    Task CreateAsync(IAsyncEnumerable<T> elements, IDbConnection connection);
    Task CreateAsync(T element, IDbConnection connection);
}

internal abstract class EntityCreator<T> : IEntityCreator<T>
{
    public abstract Task CreateAsync(IAsyncEnumerable<T> elements, IDbConnection connection);
    public async Task CreateAsync(T element, IDbConnection connection)
    {
        await CreateAsync(new List<T> { element }.ToAsyncEnumerable(), connection);
    }

    internal static async Task WriteTerms(Nameable nameable, IDatabaseInserter<Term> termWriter, IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, Term> termReader, IDatabaseInserter<TermHierarchy> termHierarchyWriter, IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader)
    {
        foreach (var vocabularyName in nameable.VocabularyNames) {
            var vocubularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
                OwnerId = vocabularyName.OwnerId,
                Name = vocabularyName.Name
            });
            var term = new Term {
                Name = vocabularyName.TermName,
                Id = null,
                VocabularyId = vocubularyId,
                NameableId = (int)nameable.Id!
            };
            await termWriter.InsertAsync(term);
            foreach (var parent in vocabularyName.ParentNames) {
                var parentTerm = await termReader.ReadAsync(new TermReaderByNameRequest {
                    Name = parent,
                    VocabularyId = vocubularyId
                });
                await termHierarchyWriter.InsertAsync(new TermHierarchy { TermIdPartent = parentTerm.Id!.Value, TermIdChild = (int)term.Id! });
            }
        }
    }

}