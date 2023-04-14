namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonInserterFactory : DatabaseInserterFactory<Person, PersonInserter>
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

    public override string TableName => "person";
}
internal sealed class PersonInserter : DatabaseInserter<Person>
{
    public PersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Person item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PersonInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PersonInserterFactory.DateOfBirth, item.DateOfBirth),
            ParameterValue.Create(PersonInserterFactory.DateOfDeath, item.DateOfDeath),
            ParameterValue.Create(PersonInserterFactory.FileIdPortrait, item.FileIdPortrait),
            ParameterValue.Create(PersonInserterFactory.GovtrackId, item.GovtrackId),
            ParameterValue.Create(PersonInserterFactory.FirstName, item.FirstName),
            ParameterValue.Create(PersonInserterFactory.MiddleName, item.MiddleName),
            ParameterValue.Create(PersonInserterFactory.LastName, item.LastName),
            ParameterValue.Create(PersonInserterFactory.Suffix, item.Suffix),
            ParameterValue.Create(PersonInserterFactory.FullName, item.FullName),
            ParameterValue.Create(PersonInserterFactory.Bioguide, item.Bioguide),
        };
    }
}
