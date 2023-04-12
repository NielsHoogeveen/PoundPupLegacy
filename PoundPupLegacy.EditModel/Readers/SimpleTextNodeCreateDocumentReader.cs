﻿namespace PoundPupLegacy.EditModel.Readers;

public abstract class SimpleTextNodeCreateDocumentReaderFactory<T> : NodeCreateDocumentReaderFactory<T>
    where T : class, IDatabaseReader
{
    public override string Sql => SQL;  
    private const string SQL = $"""
            {CTE_CREATE}
            select
                jsonb_build_object(
                    'NodeId', 
                    null,
                    'UrlId', 
                    null,
                    'PublisherId',
                    @user_id,
                    'OwnerId',
                    @tenant_id,
                    'Title', 
                    '',
                    'Text', 
                    '',
            		'Tags', null,
                    'TenantNodes',
                    null,
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null
                ) document
        """;

}

public class SimpleTextNodeCreateDocumentReader<T> : NodeCreateDocumentReader<T>
    where T : class, SimpleTextNode
{
    protected SimpleTextNodeCreateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command, nodeTypeId)
    {
    }

}


