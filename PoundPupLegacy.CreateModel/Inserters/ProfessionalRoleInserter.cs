namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ProfessionalRoleInserterFactory : DatabaseInserterFactory<ProfessionalRole> 
{
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter ProfessionId = new() { Name = "profession_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "daterange" };

    public override async Task<IDatabaseInserter<ProfessionalRole>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "professional_role",
            new DatabaseParameter[] {
                PersonId,
                ProfessionId,
                DateRange
            }
        );
        return new ProfessionalRoleInserter(command);
    }
}
internal sealed class ProfessionalRoleInserter : DatabaseInserter<ProfessionalRole>
{
    internal ProfessionalRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ProfessionalRole professionalRole)
    {
        if (professionalRole.PersonId is null)
            throw new NullReferenceException(nameof(professionalRole.Id));
        Set(ProfessionalRoleInserterFactory.PersonId, professionalRole.PersonId.Value);
        Set(ProfessionalRoleInserterFactory.ProfessionId, professionalRole.ProfessionId);
        Set(ProfessionalRoleInserterFactory.DateRange, professionalRole.DateTimeRange);
        professionalRole.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of professional role does not return an id.")
        };
    }
}
