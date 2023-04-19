namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PublicationStatus;

internal sealed class PublicationStatusInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "publication_status";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
        };
    }
}
