namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiableInterPersonalRelationForExistingParticipants;

internal sealed class InterPersonalRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter PersonIdFrom = new() { Name = "person_id_from" };
    private static readonly NonNullableIntegerDatabaseParameter PersonIdTo = new() { Name = "person_id_to" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter InterPersonalRelationTypeId = new() { Name = "inter_personal_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    private static readonly NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "inter_personal_relation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PersonIdFrom, request.PersonIdFrom),
            ParameterValue.Create(PersonIdTo, request.PersonIdTo),
            ParameterValue.Create(InterPersonalRelationTypeId, request.InterPersonalRelationTypeId),
            ParameterValue.Create(DateRange, request.DateRange),
            ParameterValue.Create(DocumentIdProof, request.DocumentIdProof),
            ParameterValue.Create(Description, request.Description),
        };
    }
}
