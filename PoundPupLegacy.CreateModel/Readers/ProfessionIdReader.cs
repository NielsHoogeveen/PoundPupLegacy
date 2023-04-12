namespace PoundPupLegacy.CreateModel.Readers;

using Factory = ProfessionIdReaderFactory;
using Reader = ProfessionIdReader;
public sealed class ProfessionIdReaderFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static NonNullableStringDatabaseParameter ProfessionName = new() { Name = "profession_name" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        select
        pr.id
        from person p
        join tenant_node tn on tn.node_id = p.id
        join professional_role pr on pr.person_id = p.id
        left join profession prt on prt.id = pr.profession_id
        left join node n on n.id = prt.id
        where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.title = @profession_name
        """;
}
public sealed class ProfessionIdReader : IntDatabaseReader<Reader.Request>
{
    public record Request
    {
        public int TenantId { get; init; }
        public int UrlId { get; init; }
        public ProfessionType ProfessionType { get; init; }
    }
    public enum ProfessionType
    {
        Senator,
        Representative,
        Lawyer,
        Therapist
    }
    internal ProfessionIdReader(NpgsqlCommand command) : base(command) { }

    private string GetProfessionName(ProfessionType type)
    {
        return type switch {
            ProfessionType.Senator => "Senator",
            ProfessionType.Lawyer => "Lawyer",
            ProfessionType.Representative => "Representative",
            ProfessionType.Therapist => "Therapist",
            _ => throw new Exception("Cannot reach"),
        };
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.UrlId, request.UrlId),
            ParameterValue.Create(Factory.ProfessionName, GetProfessionName(request.ProfessionType))
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"profession role {request.ProfessionType} cannot be found";
    }
}
