namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterPersonalRelation;

internal sealed class InterPersonalRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter PersonIdFrom = new() { Name = "person_id_from" };
    internal static NonNullableIntegerDatabaseParameter PersonIdTo = new() { Name = "person_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter InterPersonalRelationTypeId = new() { Name = "inter_personal_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "inter_personal_relation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PersonIdFrom, request.PersonIdFrom),
            ParameterValue.Create(PersonIdTo, request.PersonIdTo),
            ParameterValue.Create(InterPersonalRelationTypeId, request.InterPersonalRelationTypeId),
            ParameterValue.Create(DateRange, request.DateRange),
            ParameterValue.Create(DocumentIdProof, request.DocumentIdProof),
        };
    }
}
