namespace PoundPupLegacy.Db.Writers;

internal class CoercedAdoptionCaseWriter : IDatabaseWriter<CoercedAdoptionCase>
{
    public static DatabaseWriter<CoercedAdoptionCase> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CoercedAdoptionCase>(SingleIdWriter.CreateSingleIdCommand("coerced_adoption_case", connection));
    }
}
