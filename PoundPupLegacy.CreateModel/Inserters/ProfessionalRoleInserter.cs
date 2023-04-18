namespace PoundPupLegacy.CreateModel.Inserters;

using Request = ProfessionalRole;

internal sealed class ProfessionalRoleInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter ProfessionId = new() { Name = "profession_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "daterange" };

    public override string TableName => "professional_role";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PersonId, request.PersonId),
            ParameterValue.Create(ProfessionId, request.ProfessionId),
            ParameterValue.Create(DateRange, request.DateTimeRange)
        };
    }
}
