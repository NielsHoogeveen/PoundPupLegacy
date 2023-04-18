namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PoliticalEntity;

internal sealed class PoliticalEntityInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableIntegerDatabaseParameter FileIdFlag = new() { Name = "file_id_flag" };

    public override string TableName => "political_entity";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FileIdFlag, request.FileIdFlag),
        };
    }
}
