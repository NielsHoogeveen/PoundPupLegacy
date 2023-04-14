namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = ProfessionalRoleInserterFactory;
using Request = ProfessionalRole;
using Inserter = ProfessionalRoleInserter;

internal sealed class ProfessionalRoleInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter ProfessionId = new() { Name = "profession_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "daterange" };

    public override string TableName => "professional_role";
}
internal sealed class ProfessionalRoleInserter : AutoGenerateIdDatabaseInserter<Request>
{
    public ProfessionalRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PersonId, request.PersonId),
            ParameterValue.Create(Factory.ProfessionId, request.ProfessionId),
            ParameterValue.Create(Factory.DateRange, request.DateTimeRange)
        };
    }
}
