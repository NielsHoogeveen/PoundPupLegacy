namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewPerson;

internal sealed class PersonInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableDateTimeDatabaseParameter DateOfBirth = new() { Name = "date_of_birth" };
    private static readonly NullableDateTimeDatabaseParameter DateOfDeath = new() { Name = "date_of_death" };
    private static readonly NullableIntegerDatabaseParameter FileIdPortrait = new() { Name = "file_id_portrait" };
    private static readonly NullableStringDatabaseParameter FirstName = new() { Name = "first_name" };
    private static readonly NullableStringDatabaseParameter MiddleName = new() { Name = "middle_name" };
    private static readonly NullableStringDatabaseParameter LastName = new() { Name = "last_name" };
    private static readonly NullableStringDatabaseParameter FullName = new() { Name = "full_name" };
    private static readonly NullableStringDatabaseParameter Suffix = new() { Name = "suffix" };
    private static readonly NullableIntegerDatabaseParameter GovtrackId = new() { Name = "govtrack_id" };
    private static readonly NullableStringDatabaseParameter Bioguide = new() { Name = "bioguide" };

    public override string TableName => "person";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DateOfBirth, request.DateOfBirth),
            ParameterValue.Create(DateOfDeath, request.DateOfDeath),
            ParameterValue.Create(FileIdPortrait, request.FileIdPortrait),
            ParameterValue.Create(GovtrackId, request.GovtrackId),
            ParameterValue.Create(FirstName, request.FirstName),
            ParameterValue.Create(MiddleName, request.MiddleName),
            ParameterValue.Create(LastName, request.LastName),
            ParameterValue.Create(Suffix, request.Suffix),
            ParameterValue.Create(FullName, request.FullName),
            ParameterValue.Create(Bioguide, request.Bioguide),
        };
    }
}
