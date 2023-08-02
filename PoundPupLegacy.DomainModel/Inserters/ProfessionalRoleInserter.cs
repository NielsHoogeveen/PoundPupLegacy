﻿namespace PoundPupLegacy.DomainModel.Inserters;

using Request = ProfessionalRoleToCreateForExistingPerson;

internal sealed class ProfessionalRoleInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    private static readonly NonNullableIntegerDatabaseParameter ProfessionId = new() { Name = "profession_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "daterange" };

    public override string TableName => "professional_role";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PersonId, request.ProfessionalRoleDetails.PersonId),
            ParameterValue.Create(ProfessionId, request.ProfessionalRoleDetails.ProfessionId),
            ParameterValue.Create(DateRange, request.ProfessionalRoleDetails.DateTimeRange)
        };
    }
}