namespace PoundPupLegacy.DomainModel.Updaters;

using Request = Term.ToUpdate;

internal sealed class TermUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string Sql => $"""
        update term 
        set 
        name = @name
        where id = @id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Id, request.Identification.Id),
            ParameterValue.Create(Name, request.Name),
        };
    }
}

