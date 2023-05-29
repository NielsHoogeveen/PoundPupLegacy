namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CaseToCreate;

internal sealed class CaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableFuzzyDateDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };

    public override string TableName => "case";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FuzzyDate, request.CaseDetails.Date),
        };
    }
}
