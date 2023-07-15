using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = CaseType;

internal sealed class CaseTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };

    public override string TableName => "case_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Text, request.Text)
        };
    }
}
