﻿namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class FormalSubdivisionDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, FormalSubdivision>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.SUBDIVISIONS_VIEWER},
            {DOCUMENT}
            SELECT document from formal_subdivision_document
            """
            ;

    const string DOCUMENT = """
        formal_subdivision_document AS (
            SELECT 
                jsonb_build_object(
                'NodeId', n.node_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'PublicationStatusId', publication_status_id,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'Country', jsonb_build_object(
                    'Path', n.country_path,
                    'Title', n.country_name
                ),
                'ISO3166_2_Code', iso_3166_2_code,
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM subdivision_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_subdivision_document),
                'SubdivisionTypes', (SELECT document FROM subdivision_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    ics.iso_3166_2_code,
                    '/' || nt.viewer_path || '/' || tn.node_id country_path,
                    n.title country_name,
                    n2.title subdivision_type_name
                FROM node n
                join node_type nt on nt.id = n.node_type_id
                join subdivision sd on sd.id = n.id 
                join iso_coded_subdivision ics on ics.id = sd.id
                join tenant_node tn on tn.node_id = n.id 
                join node n2 on n2.id = sd.subdivision_type_id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.node_id = @node_id
                and tn.publication_status_id in 
                (
                    select 
                    publication_status_id  
                    from user_publication_status 
                    where tenant_id = tn.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
            ) n
        ) 
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override FormalSubdivision? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<FormalSubdivision>(0);
    }
}