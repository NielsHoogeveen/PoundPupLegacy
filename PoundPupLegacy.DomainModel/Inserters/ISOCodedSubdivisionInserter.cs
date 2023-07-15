using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = ISOCodedSubdivisionToCreate;

internal sealed class ISOCodedSubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableFixedStringDatabaseParameter ISO31661_2_Code = new() { Name = "iso_3166_2_code" };

    public override string TableName => "iso_coded_subdivision";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(ISO31661_2_Code, request.ISOCodedSubdivisionDetails.ISO3166_2_Code),
        };
    }
}
