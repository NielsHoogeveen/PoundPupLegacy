using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = Person.ToCreate;

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

    public override string TableName => "person";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DateOfBirth, request.PersonDetails.DateOfBirth),
            ParameterValue.Create(DateOfDeath, request.PersonDetails.DateOfDeath),
            ParameterValue.Create(FileIdPortrait, request.PersonDetails.FileIdPortrait),
            ParameterValue.Create(FirstName, request.PersonDetails.FirstName),
            ParameterValue.Create(MiddleName, request.PersonDetails.MiddleName),
            ParameterValue.Create(LastName, request.PersonDetails.LastName),
            ParameterValue.Create(Suffix, request.PersonDetails.Suffix),
            ParameterValue.Create(FullName, request.PersonDetails.FullName),
        };
    }
}
