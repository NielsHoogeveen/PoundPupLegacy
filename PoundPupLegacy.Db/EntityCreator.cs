using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db
{

    internal interface IEntityCreator<T>
    {
        public abstract static void Create(IEnumerable<T> elements, NpgsqlConnection connection);
    }

    internal static class EntityCreator
    {
        internal static void WriteTerms(Nameable nameable, DatabaseWriter<Term> termWriter, TermReader termReader, DatabaseWriter<TermHierarchy> termHierarchyWriter)
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
                termWriter.Write(term);
                foreach (var parent in vocabularyName.ParentNames)
                {
                    var parentTerm = termReader.Read(vocabularyName.VocabularyId, parent);
                    termHierarchyWriter.Write(new TermHierarchy { TermIdPartent = (int)parentTerm.Id!, TermIdChild = (int)term.Id! });
                }
            }
        }

    }
}