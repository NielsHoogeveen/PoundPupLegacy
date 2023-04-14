namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PrincipalInserterFactory;
using Request = Principal;
using Inserter = PrincipalInserter;

public class PrincipalInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    public override string TableName => "principal";
}
public class PrincipalInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{
    public PrincipalInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
        };
    }
}
