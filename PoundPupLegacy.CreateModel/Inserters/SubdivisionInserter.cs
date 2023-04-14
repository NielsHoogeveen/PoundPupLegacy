namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = SubdivisionInserterFactory;
using Request = Subdivision;
using Inserter = SubdivisionInserter;

internal sealed class SubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "subdivision";
}
internal sealed class SubdivisionInserter : IdentifiableDatabaseInserter<Request>
{
    public SubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.CountryId, request.CountryId),
            ParameterValue.Create(Factory.SubdivisionTypeId, request.SubdivisionTypeId),
        };
    }
}
