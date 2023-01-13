﻿using PoundPupLegacy.Model;
using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Xml.Linq;

namespace PoundPupLegacy.Db.Writers;

internal sealed class TenantNodeWriter : DatabaseWriter<TenantNode>, IDatabaseWriter<TenantNode>
{

    private const string TENANT_ID = "tenant_id";
    private const string URL_ID = "url_id";
    private const string URL_PATH = "url_path";
    private const string NODE_ID = "node_id";
    private const string SUBGROUP_ID = "subgroup_id";
    private const string PUBLICATION_STATUS_ID = "publication_status_id";

    public static async Task<DatabaseWriter<TenantNode>> CreateAsync(NpgsqlConnection connection)
    {
        var collumnDefinitions = new ColumnDefinition[]
        {
            new ColumnDefinition{
                Name = TENANT_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = URL_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = URL_PATH,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = NODE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = SUBGROUP_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = PUBLICATION_STATUS_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
        };

        var command = await CreateInsertStatementAsync(
            connection,
            "tenant_node",
            collumnDefinitions
        );
        return new TenantNodeWriter(command);
    }
 
    internal TenantNodeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(TenantNode tenantNode)
    {
        WriteValue(tenantNode.TenantId, TENANT_ID);
        WriteValue(tenantNode.UrlId.HasValue ? tenantNode.UrlId.Value: tenantNode.NodeId, URL_ID);
        WriteNullableValue(tenantNode.UrlPath, URL_PATH);
        WriteValue(tenantNode.NodeId, NODE_ID);
        WriteNullableValue(tenantNode.SubgroupId, SUBGROUP_ID);
        WriteValue(tenantNode.PublicationStatusId, PUBLICATION_STATUS_ID);
        await _command.ExecuteNonQueryAsync();
    }

}
