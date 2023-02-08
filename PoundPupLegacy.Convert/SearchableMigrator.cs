using System.Data;

namespace PoundPupLegacy.Convert;

internal class SearchableMigrator : PPLMigrator
{
    public SearchableMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "searchables";

    protected override async Task MigrateImpl()
    {
        var sql = $"""
                UPDATE searchable
                set tsvector = subquery.tsvector
                from (
                    select
                    n.id,
                    to_tsvector('english', regexp_replace(concat(a.text,' ' ,n.title), E'<[^>]+>', '', 'gi')) tsvector
                    from simple_text_node a
                    join node n  on n.id = a.id
                ) subquery
                where searchable.id = subquery.id;
                UPDATE searchable
                set tsvector = subquery.tsvector
                from (
                    select
                    n.id,
                    to_tsvector('english', regexp_replace(concat(a.text,' ' ,n.title, ' ', a.source_url), E'<[^>]+>', '', 'gi')) tsvector
                    from document a
                    join node n  on n.id = a.id
                ) subquery
                where searchable.id = subquery.id;
                """;
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }
}
