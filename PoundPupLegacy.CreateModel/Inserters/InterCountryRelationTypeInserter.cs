namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = InterCountryRelationTypeInserterFactory;
using Request = InterCountryRelationType;
using Inserter = InterCountryRelationTypeInserter;

internal sealed class InterCountryRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };
    public override string TableName => "inter_country_relation_type";
}
internal sealed class InterCountryRelationTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public InterCountryRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.IsSymmetric, request.IsSymmetric),
        };
    }
}
