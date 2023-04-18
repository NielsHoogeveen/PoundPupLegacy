namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PollStatus;

internal sealed class PollStatusInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "poll_status";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
        };
    }
}
