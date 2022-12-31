namespace PoundPupLegacy.Db;

public class FormalIntermediateLevelSubdivisionCreator : IEntityCreator<FormalIntermediateLevelSubdivision>
{
    public static void Create(IEnumerable<FormalIntermediateLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEntityWriter.Create(connection);
        using var subdivisionWriter = SubdivisionWriter.Create(connection);
        using var isoCodedSubdivisionWriter = ISOCodedSubdivisionWriter.Create(connection);
        using var firstLevelSubdivisionWriter = FirstLevelSubdivisionWriter.Create(connection);
        using var isoCodedFirstLevelSubdivisionWriter = ISOCodedFirstLevelSubdivisionWriter.Create(connection);
        using var intermediateLevelSubdivisionWriter = IntermediateLevelSubdivisionWriter.Create(connection);
        using var formalIntermediateLevelSubdivisionWriter = FormalIntermediateLevelSubdivisionWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var subdivision in subdivisions)
        {
            nodeWriter.Write(subdivision);
            documentableWriter.Write(subdivision);
            termWriter.Write(subdivision);
            geographicalEntityWriter.Write(subdivision);
            politicalEntityWriter.Write(subdivision);
            subdivisionWriter.Write(subdivision);
            isoCodedSubdivisionWriter.Write(subdivision);
            firstLevelSubdivisionWriter.Write(subdivision);
            intermediateLevelSubdivisionWriter.Write(subdivision);
            isoCodedFirstLevelSubdivisionWriter.Write(subdivision);
            formalIntermediateLevelSubdivisionWriter.Write(subdivision);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = subdivision.CountryId,
                ChildId = subdivision.Id
            });
        }
    }
}
