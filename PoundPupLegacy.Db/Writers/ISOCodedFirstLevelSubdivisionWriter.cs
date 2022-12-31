namespace PoundPupLegacy.Db.Writers;

internal class ISOCodedFirstLevelSubdivisionWriter : IDatabaseWriter<ISOCodedFirstLevelSubdivision>
{
    public static DatabaseWriter<ISOCodedFirstLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<ISOCodedFirstLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("iso_coded_first_level_subdivision", connection));
    }
}
