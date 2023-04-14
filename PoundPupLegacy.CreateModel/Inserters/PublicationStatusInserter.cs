namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PublicationStatusInserterFactory : DatabaseInserterFactory<PublicationStatus, PublicationStatusInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "publication_status";

}
internal sealed class PublicationStatusInserter : DatabaseInserter<PublicationStatus>
{

    public PublicationStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(PublicationStatus item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PublicationStatusInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PublicationStatusInserterFactory.Name, item.Name),
        };
    }
}
