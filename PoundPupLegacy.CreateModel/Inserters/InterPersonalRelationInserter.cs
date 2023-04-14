﻿namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterPersonalRelationInserterFactory : DatabaseInserterFactory<InterPersonalRelation, InterPersonalRelationInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PersonIdFrom = new() { Name = "person_id_from" };
    internal static NonNullableIntegerDatabaseParameter PersonIdTo = new() { Name = "person_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter InterPersonalRelationTypeId = new() { Name = "inter_personal_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "inter_personal_relation";

}
internal sealed class InterPersonalRelationInserter : DatabaseInserter<InterPersonalRelation>
{
    public InterPersonalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(InterPersonalRelation item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(InterPersonalRelationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(InterPersonalRelationInserterFactory.PersonIdFrom, item.PersonIdFrom),
            ParameterValue.Create(InterPersonalRelationInserterFactory.PersonIdTo, item.PersonIdTo),
            ParameterValue.Create(InterPersonalRelationInserterFactory.InterPersonalRelationTypeId, item.InterPersonalRelationTypeId),
            ParameterValue.Create(InterPersonalRelationInserterFactory.DateRange, item.DateRange),
            ParameterValue.Create(InterPersonalRelationInserterFactory.DocumentIdProof, item.DocumentIdProof),
        };
    }
}
