namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CaseInserterFactory;
using Request = Case;
using Inserter = CaseInserter;

internal sealed class CaseInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableTimeStampRangeDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };

    public override string TableName => "case";
}
internal sealed class CaseInserter : IdentifiableDatabaseInserter<Request>
{
    public CaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Description, request.Description),
            ParameterValue.Create(Factory.FuzzyDate, request.Date),
        };
    }
}
