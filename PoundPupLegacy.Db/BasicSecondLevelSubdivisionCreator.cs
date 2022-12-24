using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class BasicSecondLevelSubdivisionCreator : IEntityCreator<BasicSecondLevelSubdivision>
{
    public static void Create(IEnumerable<BasicSecondLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var geographicalEntityWriter = GeographicalEnityWriter.Create(connection);
        using var politicalEntityWriter = PoliticalEnityWriter.Create(connection);
        using var subdivisionWriter = SubdivisionWriter.Create(connection);
        using var isoCodedSubdivisionWriter = ISOCodedSubdivisionWriter.Create(connection);
        using var bottomLevelSubdivisionWriter = BottomLevelSubdivisionWriter.Create(connection);
        using var secondLevelSubdivisionWriter = SecondLevelSubdivisionWriter.Create(connection);
        using var basicSecondLevelSubdivisionWriter = BasicSecondLevelSubdivisionWriter.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var subdivision in subdivisions)
        {
            nodeWriter.Write(subdivision);
            geographicalEntityWriter.Write(subdivision);
            politicalEntityWriter.Write(subdivision);
            subdivisionWriter.Write(subdivision);
            isoCodedSubdivisionWriter.Write(subdivision);
            bottomLevelSubdivisionWriter.Write(subdivision);
            secondLevelSubdivisionWriter.Write(subdivision);
            basicSecondLevelSubdivisionWriter.Write(subdivision);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = subdivision.CountryId,
                ChildId = subdivision.Id
            });
        }
    }
}
