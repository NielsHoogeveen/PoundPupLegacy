﻿using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;

public class OrganizationsDocumentReader : DatabaseReader, IDatabaseReader<OrganizationsDocumentReader>
{
    public OrganizationsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public static async Task<OrganizationsDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var readCommand = connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = string.Format(SQL, "AND oot.organization_type_id=@organization_type_id AND ll.url_id=@country_id");
        readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("pattern", NpgsqlTypes.NpgsqlDbType.Varchar);
        readCommand.Parameters.Add("organization_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("country_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        return new OrganizationsDocumentReader(readCommand);
    }

    public async Task<OrganizationSearch> ReadAsync(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId)
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

        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["user_id"].Value = userId;
        _command.Parameters["limit"].Value = limit;
        _command.Parameters["offset"].Value = offset;
        _command.Parameters["pattern"].Value = GetPattern(searchTerm, searchOption);
        if (organizationTypeId.HasValue) {
            _command.Parameters["organization_type_id"].Value = organizationTypeId.Value;
        }
        else {
            _command.Parameters["organization_type_id"].Value = DBNull.Value;
        }
        if (countryId.HasValue) {
            _command.Parameters["country_id"].Value = countryId.Value;
        }
        else {
            _command.Parameters["country_id"].Value = DBNull.Value;
        }
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var organizations = reader.GetFieldValue<OrganizationSearch>(0);
        return organizations;

    }
    protected const string SQL = """
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
