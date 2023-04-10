namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonInserterFactory : DatabaseInserterFactory<Person>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableDateTimeDatabaseParameter DateOfBirth = new() { Name = "date_of_birth" };
    internal static NullableDateTimeDatabaseParameter DateOfDeath = new() { Name = "date_of_death" };
    internal static NullableIntegerDatabaseParameter FileIdPortrait = new() { Name = "file_id_portrait" };
    internal static NullableStringDatabaseParameter FirstName = new() { Name = "first_name" };
    internal static NullableStringDatabaseParameter MiddleName = new() { Name = "middle_name" };
    internal static NullableStringDatabaseParameter LastName = new() { Name = "last_name" };
    internal static NullableStringDatabaseParameter FullName = new() { Name = "full_name" };
    internal static NullableStringDatabaseParameter Suffix = new() { Name = "suffix" };
    internal static NullableIntegerDatabaseParameter GovtrackId = new() { Name = "govtrack_id" };
    internal static NullableStringDatabaseParameter Bioguide = new() { Name = "bioguide" };

    public override async Task<IDatabaseInserter<Person>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "person",
            new DatabaseParameter[] {
                Id,
                DateOfBirth,
                DateOfDeath,
                FileIdPortrait,
                FirstName,
                MiddleName,
                LastName,
                FullName,
                Suffix,
                GovtrackId,
                Bioguide
            }
        );
        return new PersonInserter(command);
    }
}
internal sealed class PersonInserter : DatabaseInserter<Person>
{
    internal PersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Person person)
    {
        if (person.Id is null)
            throw new NullReferenceException();
        Set(PersonInserterFactory.Id, person.Id.Value);
        Set(PersonInserterFactory.DateOfBirth, person.DateOfBirth);
        Set(PersonInserterFactory.DateOfDeath, person.DateOfDeath);
        Set(PersonInserterFactory.FileIdPortrait, person.FileIdPortrait);
        Set(PersonInserterFactory.GovtrackId, person.GovtrackId);
        Set(PersonInserterFactory.FirstName, person.FirstName);
        Set(PersonInserterFactory.MiddleName, person.MiddleName);
        Set(PersonInserterFactory.LastName, person.LastName);
        Set(PersonInserterFactory.Suffix, person.Suffix);
        Set(PersonInserterFactory.FullName, person.FullName);
        Set(PersonInserterFactory.Bioguide, person.Bioguide) ;
        await _command.ExecuteNonQueryAsync();
    }
}
