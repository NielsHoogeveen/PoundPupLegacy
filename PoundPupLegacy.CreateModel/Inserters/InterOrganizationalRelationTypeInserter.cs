namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterOrganizationalRelationTypeInserterFactory : DatabaseInserterFactory<InterOrganizationalRelationType>
{
    public override async Task<IDatabaseInserter<InterOrganizationalRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_organizational_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = InterOrganizationalRelationTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationTypeInserter.IS_SYMMETRIC,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new InterOrganizationalRelationTypeInserter(command);
    }

}
internal sealed class InterOrganizationalRelationTypeInserter : DatabaseInserter<InterOrganizationalRelationType>
{
    internal const string ID = "id";
    internal const string IS_SYMMETRIC = "is_symmetric";


    internal InterOrganizationalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterOrganizationalRelationType interOrganizationalRelationType)
    {
        if (interOrganizationalRelationType.Id is null)
            throw new NullReferenceException();
        WriteValue(interOrganizationalRelationType.Id, ID);
        WriteValue(interOrganizationalRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
