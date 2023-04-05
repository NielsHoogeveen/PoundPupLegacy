namespace PoundPupLegacy.CreateModel.Creators;

public interface IEntityCreator<T>
{
    Task CreateAsync(IAsyncEnumerable<T> elements, IDbConnection connection);
}

internal abstract class EntityCreator<T>: IEntityCreator<T>
{
    public abstract Task CreateAsync(IAsyncEnumerable<T> elements, IDbConnection connection);
    public async Task CreateAsync(T element, IDbConnection connection)
    {
        await CreateAsync(new List<T> { element }.ToAsyncEnumerable(), connection);
    }

    internal static async Task WriteTerms(Nameable nameable, IDatabaseInserter<Term> termWriter, TermReaderByName termReader, IDatabaseInserter<TermHierarchy> termHierarchyWriter, VocabularyIdReaderByOwnerAndName vocabularyIdReader)
    {
        foreach (var vocabularyName in nameable.VocabularyNames) {
            var vocubularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
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
                var parentTerm = await termReader.ReadAsync(new TermReaderByName.Request {
                    Name = parent,
                    VocabularyId = vocubularyId
                });
                await termHierarchyWriter.InsertAsync(new TermHierarchy { TermIdPartent = (int)parentTerm.Id!, TermIdChild = (int)term.Id! });
            }
        }
    }

}