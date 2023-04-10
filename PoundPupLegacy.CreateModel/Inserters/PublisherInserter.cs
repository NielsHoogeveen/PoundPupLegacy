namespace PoundPupLegacy.CreateModel.Inserters;

public class PublisherInserterFactory : DatabaseInserterFactory<Publisher, PublisherInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "publisher";
}
public class PublisherInserter : DatabaseInserter<Publisher>
{
    public PublisherInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Publisher item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PublisherInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PublisherInserterFactory.Name, item.Name),
        };
    }
}
