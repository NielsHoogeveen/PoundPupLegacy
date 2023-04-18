namespace PoundPupLegacy.CreateModel.Inserters;

using Request = BasicSecondLevelSubdivision;

internal sealed class BasicSecondLevelSubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter IntermediateLevelSubdivisionId = new() { Name = "intermediate_level_subdivision_id" };
    public override string TableName => "basic_second_level_subdivision";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IntermediateLevelSubdivisionId, request.IntermediateLevelSubdivisionId)
        };
    }
}
