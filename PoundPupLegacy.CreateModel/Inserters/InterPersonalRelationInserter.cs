namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = InterPersonalRelationInserterFactory;
using Request = InterPersonalRelation;
using Inserter = InterPersonalRelationInserter;

internal sealed class InterPersonalRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter PersonIdFrom = new() { Name = "person_id_from" };
    internal static NonNullableIntegerDatabaseParameter PersonIdTo = new() { Name = "person_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter InterPersonalRelationTypeId = new() { Name = "inter_personal_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "inter_personal_relation";

}
internal sealed class InterPersonalRelationInserter : IdentifiableDatabaseInserter<Request>
{
    public InterPersonalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PersonIdFrom, request.PersonIdFrom),
            ParameterValue.Create(Factory.PersonIdTo, request.PersonIdTo),
            ParameterValue.Create(Factory.InterPersonalRelationTypeId, request.InterPersonalRelationTypeId),
            ParameterValue.Create(Factory.DateRange, request.DateRange),
            ParameterValue.Create(Factory.DocumentIdProof, request.DocumentIdProof),
        };
    }
}
