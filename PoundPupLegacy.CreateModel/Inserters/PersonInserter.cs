namespace PoundPupLegacy.CreateModel.Inserters;

using Factory =  PersonInserterFactory;
using Request =  Person;
using Inserter = PersonInserter;

internal sealed class PersonInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
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
internal sealed class PersonInserter : IdentifiableDatabaseInserter<Request>
{
    public PersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.DateOfBirth, request.DateOfBirth),
            ParameterValue.Create(Factory.DateOfDeath, request.DateOfDeath),
            ParameterValue.Create(Factory.FileIdPortrait, request.FileIdPortrait),
            ParameterValue.Create(Factory.GovtrackId, request.GovtrackId),
            ParameterValue.Create(Factory.FirstName, request.FirstName),
            ParameterValue.Create(Factory.MiddleName, request.MiddleName),
            ParameterValue.Create(Factory.LastName, request.LastName),
            ParameterValue.Create(Factory.Suffix, request.Suffix),
            ParameterValue.Create(Factory.FullName, request.FullName),
            ParameterValue.Create(Factory.Bioguide, request.Bioguide),
        };
    }
}
