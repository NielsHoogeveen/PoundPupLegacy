namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonInserterFactory : DatabaseInserterFactory<Person>
{
    public override async Task<IDatabaseInserter<Person>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "person",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PersonInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonInserter.DATE_OF_BIRTH,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = PersonInserter.DATE_OF_DEATH,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = PersonInserter.FILE_ID_PORTRAIT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonInserter.GOVTRACK_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonInserter.FIRST_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PersonInserter.MIDDLE_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PersonInserter.LAST_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PersonInserter.FULL_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PersonInserter.SUFFIX,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PersonInserter.BIOGUIDE,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PersonInserter(command);
    }
}
internal sealed class PersonInserter : DatabaseInserter<Person>
{
    internal const string ID = "id";
    internal const string DATE_OF_BIRTH = "date_of_birth";
    internal const string DATE_OF_DEATH = "date_of_death";
    internal const string FILE_ID_PORTRAIT = "file_id_portrait";
    internal const string FIRST_NAME = "first_name";
    internal const string MIDDLE_NAME = "middle_name";
    internal const string LAST_NAME = "last_name";
    internal const string FULL_NAME = "full_name";
    internal const string SUFFIX = "suffix";
    internal const string GOVTRACK_ID = "govtrack_id";
    internal const string BIOGUIDE = "bioguide";


    internal PersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Person person)
    {
        if (person.Id is null)
            throw new NullReferenceException();
        WriteValue(person.Id, ID);
        WriteNullableValue(person.DateOfBirth, DATE_OF_BIRTH);
        WriteNullableValue(person.DateOfDeath, DATE_OF_DEATH);
        WriteNullableValue(person.FileIdPortrait, FILE_ID_PORTRAIT);
        WriteNullableValue(person.GovtrackId, GOVTRACK_ID);
        WriteNullableValue(person.FirstName, FIRST_NAME);
        WriteNullableValue(person.MiddleName, MIDDLE_NAME);
        WriteNullableValue(person.LastName, LAST_NAME);
        WriteNullableValue(person.Suffix, SUFFIX);
        WriteNullableValue(person.FullName, FULL_NAME);
        WriteNullableValue(person.Bioguide, BIOGUIDE);
        await _command.ExecuteNonQueryAsync();
    }
}
