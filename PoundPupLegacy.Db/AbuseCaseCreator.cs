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

        foreach (var abuseCase in abuseCases)
        {
            nodeWriter.Write(abuseCase);
            documentableWriter.Write(abuseCase);
            locatableWriter.Write(abuseCase);
            caseWriter.Write(abuseCase);
            abuseCaseWriter.Write(abuseCase);
        }
    }
}
