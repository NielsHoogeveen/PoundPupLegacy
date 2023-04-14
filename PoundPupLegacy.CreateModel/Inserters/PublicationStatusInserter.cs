namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PublicationStatusInserterFactory;
using Request = PublicationStatus;
using Inserter = PublicationStatusInserter;

internal sealed class PublicationStatusInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "publication_status";

}
internal sealed class PublicationStatusInserter : IdentifiableDatabaseInserter<Request>
{

    public PublicationStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
        };
    }
}
