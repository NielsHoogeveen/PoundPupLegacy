namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Profession;

internal sealed class ProfessionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "profession";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
