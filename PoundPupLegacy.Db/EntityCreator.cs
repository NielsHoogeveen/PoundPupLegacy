using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db
{

    internal interface IEntityCreator<T>
    {
        public abstract static Task CreateAsync(IEnumerable<T> elements, NpgsqlConnection connection);
    }

    internal static class EntityCreator
    {
        internal static async Task WriteTerms(Nameable nameable, DatabaseWriter<Term> termWriter, TermReader termReader, DatabaseWriter<TermHierarchy> termHierarchyWriter)
        {
            foreach (var vocabularyName in nameable.VocabularyNames)
            {
                var term = new Term
                {
                    Name = vocabularyName.Name,
                    Id = null,
                    VocabularyId = vocabularyName.VocabularyId,
                    NameableId = (int)nameable.Id!
                };
                await termWriter.WriteAsync(term);
                foreach (var parent in vocabularyName.ParentNames)
                {
                    var parentTerm = await termReader.ReadAsync(vocabularyName.VocabularyId, parent);
                    await termHierarchyWriter.WriteAsync(new TermHierarchy { TermIdPartent = (int)parentTerm.Id!, TermIdChild = (int)term.Id! });
                }
            }
        }

    }
}