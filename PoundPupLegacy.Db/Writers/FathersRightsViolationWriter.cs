namespace PoundPupLegacy.Db.Writers;

internal class FathersRightsViolationCaseWriter : IDatabaseWriter<FathersRightsViolationCase>
{
    public static DatabaseWriter<FathersRightsViolationCase> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FathersRightsViolationCase>(SingleIdWriter.CreateSingleIdCommand("fathers_rights_violation_case", connection));
    }
}
