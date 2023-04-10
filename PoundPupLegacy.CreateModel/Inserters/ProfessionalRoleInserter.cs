namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ProfessionalRoleInserterFactory : AutoGenerateIdDatabaseInserterFactory<ProfessionalRole, ProfessionalRoleInserter> 
{
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter ProfessionId = new() { Name = "profession_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "daterange" };

    public override string TableName => "professional_role";
}
internal sealed class ProfessionalRoleInserter : AutoGenerateIdDatabaseInserter<ProfessionalRole>
{
    public ProfessionalRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(ProfessionalRole professionalRole)
    {
        if (professionalRole.Id.HasValue) {
            throw new Exception($"professional role id should be null upon creation");
        }
        if (professionalRole.PersonId is null)
            throw new NullReferenceException(nameof(professionalRole.Id));
        return new ParameterValue[] {
            ParameterValue.Create(ProfessionalRoleInserterFactory.PersonId, professionalRole.PersonId.Value),
            ParameterValue.Create(ProfessionalRoleInserterFactory.ProfessionId, professionalRole.ProfessionId),
            ParameterValue.Create(ProfessionalRoleInserterFactory.DateRange, professionalRole.DateTimeRange)
        };
    }
}
