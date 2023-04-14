namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PoliticalEntityInserterFactory;
using Request = PoliticalEntity;
using Inserter = PoliticalEntityInserter;

internal sealed class PoliticalEntityInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableIntegerDatabaseParameter FileIdFlag = new() { Name = "file_id_flag" };

    public override string TableName => "political_entity";
}
internal sealed class PoliticalEntityInserter : IdentifiableDatabaseInserter<Request>
{

    public PoliticalEntityInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.FileIdFlag, request.FileIdFlag),
        };
    }
}
