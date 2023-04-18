namespace PoundPupLegacy.CreateModel.Inserters;

using Request =  Person;

internal sealed class PersonInserterFactory : IdentifiableDatabaseInserterFactory<Request>
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
