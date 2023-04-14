namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = InterOrganizationalRelationTypeInserterFactory;
using Request = InterOrganizationalRelationType;
using Inserter = InterOrganizationalRelationTypeInserter;

internal sealed class InterOrganizationalRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };
    public override string TableName => "inter_organizational_relation_type";
}
internal sealed class InterOrganizationalRelationTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public InterOrganizationalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.IsSymmetric, request.IsSymmetric),
        };
    }
}
