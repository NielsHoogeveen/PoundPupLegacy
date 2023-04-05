namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ProfessionalRoleInserterFactory : DatabaseInserterFactory<ProfessionalRole> 
{
    public override async Task<IDatabaseInserter<ProfessionalRole>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "professional_role",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ProfessionalRoleInserter.PERSON_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ProfessionalRoleInserter.PROFESSION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ProfessionalRoleInserter.DATERANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new ProfessionalRoleInserter(command);
    }
}
internal sealed class ProfessionalRoleInserter : DatabaseInserter<ProfessionalRole>
{
    internal const string PERSON_ID = "person_id";
    internal const string PROFESSION_ID = "profession_id";
    internal const string DATERANGE = "daterange";

    internal ProfessionalRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ProfessionalRole professionalRole)
    {
        if (professionalRole.Id is null)
            throw new NullReferenceException(nameof(professionalRole.Id));
        WriteValue(professionalRole.PersonId, PERSON_ID);
        WriteValue(professionalRole.ProfessionId, PROFESSION_ID);
        WriteDateTimeRange(professionalRole.DateTimeRange, DATERANGE);
        professionalRole.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of professional role does not return an id.")
        };
    }
}
