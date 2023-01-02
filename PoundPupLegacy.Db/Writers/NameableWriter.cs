namespace PoundPupLegacy.Db.Writers;

internal class NameableWriter : DatabaseWriter<Nameable>, IDatabaseWriter<Nameable>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    public static DatabaseWriter<Nameable> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "nameable",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new NameableWriter(command);

    }

    internal NameableWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Nameable nameable)
    {
        if (nameable.Id is null)
            throw new NullReferenceException();

        WriteValue(nameable.Id, ID);
        WriteValue(nameable.Description, DESCRIPTION);
        _command.ExecuteNonQuery();
    }
}
