﻿namespace PoundPupLegacy.CreateModel.Readers;
public sealed class ProfessionIdReaderFactory : DatabaseReaderFactory<ProfessionIdReader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static NonNullableStringDatabaseParameter ProfessionName = new() { Name = "profession_name" };

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
public sealed class ProfessionIdReader : SingleItemDatabaseReader<ProfessionIdReader.Request, int>
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

    public override async Task<int> ReadAsync(Request request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["url_id"].Value = request.UrlId;
        _command.Parameters["profession_name"].Value = GetProfessionName(request.ProfessionType);

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"profession role {request.ProfessionType} cannot be found");
    }
}
