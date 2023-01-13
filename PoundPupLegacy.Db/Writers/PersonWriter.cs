namespace PoundPupLegacy.Db.Writers;

internal sealed class PersonWriter : DatabaseWriter<Person>, IDatabaseWriter<Person>
{
    private const string ID = "id";
    private const string DATE_OF_BIRTH = "date_of_birth";
    private const string DATE_OF_DEATH = "date_of_death";
    private const string FILE_ID_PORTRAIT = "file_id_portrait";
    public static async Task<DatabaseWriter<Person>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "person",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_OF_BIRTH,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = DATE_OF_DEATH,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = FILE_ID_PORTRAIT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );

        return new PersonWriter(command);

    }

    internal PersonWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Person person)
    {
        if (person.Id is null)
            throw new NullReferenceException();
        WriteValue(person.Id, ID);
        WriteNullableValue(person.DateOfBirth, DATE_OF_BIRTH);
        WriteNullableValue(person.DateOfDeath, DATE_OF_DEATH);
        WriteNullableValue(person.FileIdPortrait, FILE_ID_PORTRAIT);
        await _command.ExecuteNonQueryAsync();
    }
}
