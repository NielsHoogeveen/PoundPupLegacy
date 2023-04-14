namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = BasicSecondLevelSubdivisionInserterFactory;
using Request = BasicSecondLevelSubdivision;
using Inserter = BasicSecondLevelSubdivisionInserter;

internal sealed class BasicSecondLevelSubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter IntermediateLevelSubdivisionId = new() { Name = "intermediate_level_subdivision_id" };
    public override string TableName => "basic_second_level_subdivision";
}
internal sealed class BasicSecondLevelSubdivisionInserter : IdentifiableDatabaseInserter<Request>
{
    public BasicSecondLevelSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.IntermediateLevelSubdivisionId, request.IntermediateLevelSubdivisionId)
        };
    }
}
