namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PublisherInserterFactory;
using Request = Publisher;
using Inserter = PublisherInserter;

public class PublisherInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "publisher";
}
public class PublisherInserter : IdentifiableDatabaseInserter<Request>
{
    public PublisherInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
        };
    }
}
