namespace PoundPupLegacy.CreateModel.Inserters;

public class PrincipalInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Principal, PrincipalInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };

    public override string TableName => "principal";
}
public class PrincipalInserter : ConditionalAutoGenerateIdDatabaseInserter<Principal>
{
    public PrincipalInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Principal item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PrincipalInserterFactory.Id, item.Id)
        };
    }
}
