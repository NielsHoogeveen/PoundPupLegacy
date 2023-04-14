namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = ProfessionInserterFactory;
using Request = Profession;
using Inserter = ProfessionInserter;

internal sealed class ProfessionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "profession";

}
internal sealed class ProfessionInserter : IdentifiableDatabaseInserter<Request>
{

    public ProfessionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
