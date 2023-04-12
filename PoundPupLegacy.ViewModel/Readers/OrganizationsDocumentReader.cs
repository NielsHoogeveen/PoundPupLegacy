using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel.Readers;
public class OrganizationsDocumentReaderFactory : DatabaseReaderFactory<OrganizationsDocumentReader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    internal static NonNullableIntegerDatabaseParameter Limit = new() { Name = "limit" };
    internal static NonNullableIntegerDatabaseParameter Offset = new() { Name = "offset" };
    internal static NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NullableStringDatabaseParameter Pattern = new() { Name = "pattern" };

    public override string Sql => SQL;

    protected const string SQL = """
        select
            jsonb_build_object(
                'Countries', (select
                    jsonb_agg(
                        jsonb_build_object(
            	            'Name', "name",
            	            'Id', "url_id"
                        )
                    ) "document"
                    from(
                        select
                        distinct
                        n.title "name",
                        tn.url_id
                        from organization o
                        join location_locatable ll on ll.locatable_id = o.id
                        join location l on l.id = ll.location_id
                        join node n on n.id = l.country_id
                        join tenant_node tn on tn.node_id = n.id and tn.tenant_id = 1
                        ORDER BY n.title
                    ) x
                ),
                'OrganizationTypes', (select
                    jsonb_agg(
                        jsonb_build_object(
            	            'Name', "name",
            	            'Id', "id"
                        )
                    ) "document"
                    from(
                        select
                        distinct
                        n.title "name",
                        tn.node_id "id"
                        from organization_type ot
                        join node n on n.id = ot.id
                        join tenant_node tn on tn.node_id = n.id and tn.tenant_id = 1
                        ORDER BY n.title
                    ) x
                ),
                'Organizations', (select
            	    jsonb_build_object(
            	        'NumberOfEntries', number_of_entries,
            	        'Entries', jsonb_agg(
            	            jsonb_build_object(
            	                'Path', url_path,
            	                'Title', title,
            	                'HasBeenPublished', case 
            		                when status = 0 then false
            		                else true
            	                end
            	            )
                        )
            	    )
                    from(
            	        select
            	        tn.id,
            	        n.title,
            	        n.node_type_id,
            	        tn.tenant_id,
            	        tn.node_id,
            	        n.publisher_id,
            	        n.created_date_time,
            	        n.changed_date_time,
            	        tn.url_id,
            	        count(tn.id) over() number_of_entries,
            	        case 
            		        when tn.url_path is null then '/node/' || tn.url_id
            		        else '/' || url_path
            	        end url_path,
            	        tn.subgroup_id,
            	        tn.publication_status_id,
            	        case
            		        when tn.publication_status_id = 0 then (
            			        select
            				        case 
            					        when count(*) > 0 then 0
            					        else -1
            				        end status
            			        from user_group_user_role_user ugu
                                join user_group ug on ug.id = ugu.user_group_id
            			        WHERE ugu.user_group_id = 
            			        case
            				        when tn.subgroup_id is null then tn.tenant_id 
            				        else tn.subgroup_id 
            			        end 
            			        AND ugu.user_role_id = ug.administrator_role_id
            			        AND ugu.user_id = @user_id
            		        )
            		        when tn.publication_status_id = 1 then 1
            		        when tn.publication_status_id = 2 then (
            			        select
            				        case 
            					        when count(*) > 0 then 1
            					        else -1
            				        end status
            			        from user_group_user_role_user ugu
            			        WHERE ugu.user_group_id = 
            				        case
            					        when tn.subgroup_id is null then tn.tenant_id 
            					        else tn.subgroup_id 
            				        end
            				        AND ugu.user_id = 2
            			        )
            		        end status	
            	        from tenant_node tn
            	        join node n on n.id = tn.node_id
            	        join organization o on o.id = n.id
                        left join (
                            select 
                            ll.locatable_id,
                            tn.url_id
                            from location_locatable ll 
                            join location l on l.id = ll.location_id
                            join tenant_node tn on tn.node_id = l.country_id
                            where tn.tenant_id = @tenant_id
                        ) ll on ll.locatable_id = o.id
                        left join organization_organization_type oot on oot.organization_id = o.id
            	        WHERE tn.tenant_id = 1 and n.title ilike @pattern 
                        AND (@country_id is null or ll.url_id = @country_id)
                        AND (@organization_type_id is null or oot.organization_type_id = @organization_type_id)
            	        ORDER BY n.title
                        LIMIT @limit OFFSET @offset
                    ) an
                    where an.status <> -1
                    group by number_of_entries
                )
            )
        """;

}
public class OrganizationsDocumentReader : SingleItemDatabaseReader<OrganizationsDocumentReader.OrganizationsDocumentRequest, OrganizationSearch>
{
    public record OrganizationsDocumentRequest
    {
        public required int UserId { get; init; }
        public required int TenantId { get; init; }
        public required int Limit { get; init; }
        public required int Offset { get; init; }
        public required string SearchTerm { get; init; }
        public required SearchOption SearchOption { get; init; }
        public required int? OrganizationTypeId { get; init; }
        public required int? CountryId { get; init; }
    }
    public OrganizationsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task<OrganizationSearch> ReadAsync(OrganizationsDocumentRequest request)
    {
        string GetPattern(string searchTerm, SearchOption searchOption)
        {
            if (string.IsNullOrEmpty(searchTerm)) {
                return "%";
            }
            return searchOption switch {
                SearchOption.IsEqualTo => searchTerm,
                SearchOption.Contains => $"%{searchTerm}%",
                SearchOption.StartsWith => $"{searchTerm}%",
                SearchOption.EndsWith => $"%{searchTerm}",
                _ => throw new Exception("Cannot reach")
            };
        }

        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["limit"].Value = request.Limit;
        _command.Parameters["offset"].Value = request.Offset;
        _command.Parameters["pattern"].Value = GetPattern(request.SearchTerm, request.SearchOption);
        if (request.OrganizationTypeId.HasValue) {
            _command.Parameters["organization_type_id"].Value = request.OrganizationTypeId.Value;
        }
        else {
            _command.Parameters["organization_type_id"].Value = DBNull.Value;
        }
        if (request.CountryId.HasValue) {
            _command.Parameters["country_id"].Value = request.CountryId.Value;
        }
        else {
            _command.Parameters["country_id"].Value = DBNull.Value;
        }
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var organizations = reader.GetFieldValue<OrganizationSearch>(0);
        return organizations;

    }

}
