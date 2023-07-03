namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PublisherToCreate;

public class PublisherInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "publisher";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
        };
    }
}
