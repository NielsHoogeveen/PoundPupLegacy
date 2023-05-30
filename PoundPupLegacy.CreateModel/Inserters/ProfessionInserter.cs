namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Profession.ToCreate;

internal sealed class ProfessionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "profession";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HasConcreteSubtype, request.ProfessionDetails.HasConcreteSubtype),
        };
    }
}
