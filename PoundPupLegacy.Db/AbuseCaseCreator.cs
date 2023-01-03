using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class AbuseCaseCreator : IEntityCreator<AbuseCase>
{
    public static void Create(IEnumerable<AbuseCase> abuseCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var abuseCaseWriter = AbuseCaseWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var abuseCase in abuseCases)
        {
            nodeWriter.Write(abuseCase);
            documentableWriter.Write(abuseCase);
            locatableWriter.Write(abuseCase);
            caseWriter.Write(abuseCase);
            abuseCaseWriter.Write(abuseCase);
            EntityCreator.WriteTerms(abuseCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
