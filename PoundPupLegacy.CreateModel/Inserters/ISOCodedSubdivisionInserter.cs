namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = ISOCodedSubdivisionInserterFactory;
using Request = ISOCodedSubdivision;
using Inserter = ISOCodedSubdivisionInserter;

internal sealed class ISOCodedSubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableFixedStringDatabaseParameter ISO31661_2_Code = new() { Name = "iso_3166_2_code" };

    public override string TableName => "iso_coded_subdivision";
}
internal sealed class ISOCodedSubdivisionInserter : IdentifiableDatabaseInserter<Request>
{
    public ISOCodedSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.ISO31661_2_Code, request.ISO3166_2_Code),
        };
    }
}
