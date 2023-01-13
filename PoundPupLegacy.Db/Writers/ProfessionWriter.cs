namespace PoundPupLegacy.Db.Writers;

internal sealed class ProfessionWriter : DatabaseWriter<Profession>, IDatabaseWriter<Profession>
{
    private const string ID = "id";
    private const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";
    public static async Task<DatabaseWriter<Profession>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "profession",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new ProfessionWriter(command);

    }

    internal ProfessionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Profession profession)
    {
        if (profession.Id is null)
            throw new NullReferenceException();

        WriteValue(profession.Id, ID);
        WriteValue(profession.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
