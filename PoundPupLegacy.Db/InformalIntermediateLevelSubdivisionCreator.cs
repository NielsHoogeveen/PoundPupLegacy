namespace PoundPupLegacy.Db;

public class InformalIntermediateLevelSubdivisionCreator : IEntityCreator<InformalIntermediateLevelSubdivision>
{
    public static void Create(IEnumerable<InformalIntermediateLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var subdivisionWriter = SubdivisionWriter.Create(connection);
        using var firstLevelSubdivisionWriter = FirstLevelSubdivisionWriter.Create(connection);
        using var intermediateLevelSubdivisionWriter = IntermediateLevelSubdivisionWriter.Create(connection);
        using var formalIntermediateLevelSubdivisionWriter = InformalIntermediateLevelSubdivisionWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var subdivision in subdivisions)
        {
            nodeWriter.Write(subdivision);
            documentableWriter.Write(subdivision);
            geographicalEntityWriter.Write(subdivision);
            subdivisionWriter.Write(subdivision);
            firstLevelSubdivisionWriter.Write(subdivision);
            intermediateLevelSubdivisionWriter.Write(subdivision);
            formalIntermediateLevelSubdivisionWriter.Write(subdivision);
            termHierarchyWriter.Write(new TermHierarchy
            {
                TermIdPartent = subdivision.CountryId,
                TermIdChild = (int)subdivision.Id!
            });
        }
    }
}
