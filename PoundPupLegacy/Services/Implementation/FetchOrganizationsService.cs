using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchOrganizationsService : IFetchOrganizationsService
{
    private NpgsqlConnection _connection;

    public FetchOrganizationsService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    private string GetPattern(string searchTerm, SearchOption searchOption)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return "%";
        }
        return searchOption switch
        {
            SearchOption.IsEqualTo => searchTerm,
            SearchOption.Contains => $"%{searchTerm}%",
            SearchOption.StartsWith => $"{searchTerm}%",
            SearchOption.EndsWith => $"%{searchTerm}",
            _ => throw new Exception("Cannot reach")
        };
    }

    public async Task<OrganizationSearch> FetchOrganizations(int limit, int offset, string searchTerm, SearchOption searchOption, int tenantId, int userId, int? organizationTypeId, int? countryId)
    {
        await _connection.OpenAsync();
        try
        {
            var filter = (organizationTypeId, countryId) switch
            {
                (null, null) => "",
                (null, int) => "AND ll.url_id=@country_id",
                (int, null) => "AND oot.organization_type_id=@organization_type_id",
                (int, int) => "AND oot.organization_type_id=@organization_type_id AND ll.url_id=@country_id",
            };
            var sql = $"""
            select
                json_build_object(
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
            	            'Entries', json_agg(
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
            	            WHERE tn.tenant_id = 1 and n.title ilike @pattern {filter}
            	            ORDER BY n.title
                            LIMIT @limit OFFSET @offset
                        ) an
                        where an.status <> -1
                        group by number_of_entries
                    )
                )
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("pattern", NpgsqlTypes.NpgsqlDbType.Varchar);
            if (organizationTypeId.HasValue)
            {
                readCommand.Parameters.Add("organization_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
            }
            if (countryId.HasValue)
            {
                readCommand.Parameters.Add("country_id", NpgsqlTypes.NpgsqlDbType.Integer);
            }
            await readCommand.PrepareAsync();
            readCommand.Parameters["tenant_id"].Value = tenantId;
            readCommand.Parameters["user_id"].Value = userId;
            readCommand.Parameters["limit"].Value = limit;
            readCommand.Parameters["offset"].Value = offset;
            readCommand.Parameters["pattern"].Value = GetPattern(searchTerm, searchOption);
            if (organizationTypeId.HasValue)
            {
                readCommand.Parameters["organization_type_id"].Value = organizationTypeId.Value;
            }
            if (countryId.HasValue)
            {
                readCommand.Parameters["country_id"].Value = countryId.Value;
            }
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var organizations = reader.GetFieldValue<OrganizationSearch>(0);
            return organizations;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

}
