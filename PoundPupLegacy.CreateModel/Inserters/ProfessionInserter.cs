namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ProfessionInserter : DatabaseInserter<Profession>, IDatabaseInserter<Profession>
{
    private const string ID = "id";
    private const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";
    public static async Task<DatabaseInserter<Profession>> CreateAsync(NpgsqlConnection connection)
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
        return new ProfessionInserter(command);

    }

    internal ProfessionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Profession profession)
    {
        if (profession.Id is null)
            throw new NullReferenceException();

        WriteValue(profession.Id, ID);
        WriteValue(profession.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
