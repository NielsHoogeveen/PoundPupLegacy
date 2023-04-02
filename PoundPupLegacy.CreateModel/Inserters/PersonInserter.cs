namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PersonInserter : DatabaseInserter<Person>, IDatabaseInserter<Person>
{
    private const string ID = "id";
    private const string DATE_OF_BIRTH = "date_of_birth";
    private const string DATE_OF_DEATH = "date_of_death";
    private const string FILE_ID_PORTRAIT = "file_id_portrait";
    private const string FIRST_NAME = "first_name";
    private const string MIDDLE_NAME = "middle_name";
    private const string LAST_NAME = "last_name";
    private const string FULL_NAME = "full_name";
    private const string SUFFIX = "suffix";
    private const string GOVTRACK_ID = "govtrack_id";
    private const string BIOGUIDE = "bioguide";

    public static async Task<DatabaseInserter<Person>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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
                new ColumnDefinition{
                    Name = GOVTRACK_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FIRST_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = MIDDLE_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = LAST_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = FULL_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = SUFFIX,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = BIOGUIDE,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },

            }
        );

        return new PersonInserter(command);

    }

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
