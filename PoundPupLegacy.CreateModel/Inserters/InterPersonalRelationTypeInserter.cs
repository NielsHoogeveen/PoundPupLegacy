namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = InterPersonalRelationTypeInserterFactory;
using Request = InterPersonalRelationType;
using Inserter = InterPersonalRelationTypeInserter;

internal sealed class InterPersonalRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override string TableName => "inter_personal_relation_type";
}
internal sealed class InterPersonalRelationTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public InterPersonalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.IsSymmetric, request.IsSymmetric),
        };
    }
}
