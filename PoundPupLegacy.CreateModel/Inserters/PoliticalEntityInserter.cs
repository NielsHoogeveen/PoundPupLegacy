namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PoliticalEntityToCreate;

internal sealed class PoliticalEntityInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableIntegerDatabaseParameter FileIdFlag = new() { Name = "file_id_flag" };

    public override string TableName => "political_entity";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FileIdFlag, request.FileIdFlag),
        };
    }
}
