using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchNodeService : IFetchNodeService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISiteDataService _siteDateService;
    public FetchNodeService(
        NpgsqlConnection connection,
        ISiteDataService siteDataService)
    {
        _connection = connection;
        _siteDateService = siteDataService;
    }

    public async Task<Node?> FetchNode(int id)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            WITH 
            {AUTHENTICATED_NODE},
            {FILES_DOCUMENT},
            {SEE_ALSO_DOCUMENT},
            {DOCUMENTABLES_DOCUMENT},
            {LOCATIONS_DOCUMENT},
            {POLL_OPTIONS_DOCUMENT},
            {POLL_QUESTIONS_DOCUMENT},
            {ORGANIZATION_CASES_DOCUMENT},
            {PERSON_CASES_DOCUMENT},
            {CASE_CASE_PARTIES_DOCUMENT},
            {INTER_ORGANIZATIONAL_RELATION_DOCUMENT},
            {INTER_PERSONAL_RELATION_DOCUMENT},
            {PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT},
            {PROFESSIONS_DOCUMENT},
            {ORGANIZATION_TYPES_DOCUMENT},
            {PERSON_ORGANIZATION_RELATIONS_DOCUMENT},
            {ORGANIZATION_PERSON_RELATIONS_DOCUMENT},
            {TAGS_DOCUMENT},
            {SUBTOPICS_DOCUMENT},
            {SUPERTOPICS_DOCUMENT},
            {DOCUMENTS_DOCUMENT},
            {COMMENTS_DOCUMENT},
            {ORGANIZATIONS_OF_COUNTRY_DOCUMENT},
            {COUNTRY_SUBDIVISIONS_DOCUMENT},
            {BLOG_POST_BREADCRUM_DOCUMENT},
            {PAGE_BREADCRUM_DOCUMENT},
            {ORGANIZATION_BREADCRUM_DOCUMENT},
            {DOCUMENT_BREADCRUM_DOCUMENT},
            {ARTICLE_BREADCRUM_DOCUMENT},
            {POLL_BREADCRUM_DOCUMENT},
            {ABUSE_CASE_BREADCRUM_DOCUMENT},
            {CHILD_TRAFFICKING_CASE_BREADCRUM_DOCUMENT},
            {COERCED_ADOPTION_CASE_BREADCRUM_DOCUMENT},
            {DEPORTATION_CASE_BREADCRUM_DOCUMENT},
            {DISRUPTED_PLACEMENT_CASE_BREADCRUM_DOCUMENT},
            {FATHERS_RIGHTS_VIOLATION_CASE_BREADCRUM_DOCUMENT},
            {WRONGFUL_MEDICATION_CASE_BREADCRUM_DOCUMENT},
            {WRONGFUL_REMOVAL_CASE_BREADCRUM_DOCUMENT},
            {COUNTRY_BREADCRUM_DOCUMENT},
            {TOPICS_BREADCRUM_DOCUMENT},
            {ADOPTION_IMPORTS_DOCUMENT},
            {BLOG_POST_DOCUMENT},
            {PAGE_DOCUMENT},
            {ARTICLE_DOCUMENT},
            {DOCUMENT_DOCUMENT},
            {SINGLE_QUESTION_POLL_DOCUMENT},
            {MULTIPLE_QUESTION_POLL_DOCUMENT},
            {ABUSE_CASE_DOCUMENT},
            {CHILD_TRAFFICKING_CASE_DOCUMENT},
            {COERCED_ADOPTION_CASE_DOCUMENT},
            {DEPORTATION_CASE_DOCUMENT},
            {DISRUPTED_PLACEMENT_CASE_DOCUMENT},
            {FATHERS_RIGHTS_VIOLATION_CASE_DOCUMENT},
            {WRONGFUL_MEDICATION_CASE_DOCUMENT},
            {WRONGFUL_REMOVAL_CASE_DOCUMENT},
            {BASIC_NAMEABLE_DOCUMENT},
            {ORGANIZATION_DOCUMENT},
            {PERSON_DOCUMENT},
            {BASIC_COUNTRY_DOCUMENT},
            {NODE_DOCUMENT}
            SELECT node_type_id, document from node_document
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["url_id"].Value = id;
            readCommand.Parameters["tenant_id"].Value = _siteDateService.GetTenantId();
            readCommand.Parameters["user_id"].Value = _siteDateService.GetUserId();
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows)
            {
                return null;
            }
            var node_type_id = reader.GetInt32(0);
            var txt = reader.GetString(1);
            Node node = node_type_id switch
            {
                10 => reader.GetFieldValue<Document>(1),
                13 => reader.GetFieldValue<BasicCountry>(1),
                23 => reader.GetFieldValue<Organization>(1),
                24 => reader.GetFieldValue<Person>(1),
                26 => reader.GetFieldValue<AbuseCase>(1),
                29 => reader.GetFieldValue<ChildTraffickingCase>(1),
                30 => reader.GetFieldValue<CoercedAdoptionCase>(1),
                31 => reader.GetFieldValue<DeportationCase>(1),
                32 => reader.GetFieldValue<FathersRightsViolationCase>(1),
                33 => reader.GetFieldValue<WrongfulMedicationCase>(1),
                34 => reader.GetFieldValue<WrongfulRemovalCase>(1),
                35 => reader.GetFieldValue<BlogPost>(1),
                36 => reader.GetFieldValue<Article>(1),
                37 => reader.GetFieldValue<Discussion>(1),
                41 => reader.GetFieldValue<BasicNameable>(1),
                42 => reader.GetFieldValue<Page>(1),
                44 => reader.GetFieldValue<DisruptedPlacementCase>(1),
                53 => reader.GetFieldValue<SingleQuestionPoll>(1),
                54 => reader.GetFieldValue<MultiQuestionPoll>(1),
                _ => throw new Exception($"Node {id} has Unsupported type {node_type_id}")
            };

            return node!;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    const string FILES_DOCUMENT = """
        files_document as(
            select
            jsonb_agg(
                jsonb_build_object(
                    'Name', f.name,
                    'Path', f.path,
                    'Size', f.size,
                    'MimeType', f.mime_type
                )
            ) document
            from node_file nf
            join tenant_node tn on tn.node_id = nf.node_id
            join "file" f on f.id = nf.file_id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;

    const string AUTHENTICATED_NODE = """
        authenticated_node as (
            select
                id,
                title,
                node_type_id,
                tenant_id,
                node_id,
                publisher_id,
                created_date_time,
                changed_date_time,
                url_id,
                url_path,
                subgroup_id,
                publication_status_id,
                case 
                    when status = 0 then false
                    else true
                end has_been_published
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
                            AND ugu.user_id = @user_id
                        )
                    end status	
                    from
                    tenant_node tn
                    join node n on n.id = tn.node_id
                    WHERE tn.tenant_id = @tenant_id AND tn.url_id = @url_id
                ) an
                where an.status <> -1
        )
        """;
    const string POLL_OPTIONS_DOCUMENT = """
        poll_options_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Text', text,
        		        'NumberOfVotes', number_of_votes,
        		        'Percentage', round(100 * (number_of_votes::numeric  / total), 0),
                        'Delta', delta
        	        )
                ) document
            from(
        	    select 
        	    po.text,
        	    po.number_of_votes,
        	    sum(number_of_votes) over() total,
                po.delta
        	    from poll_option po
                join poll_question pq on po.poll_question_id = pq.id
        	    join tenant_node tn on tn.node_id = pq.id
        	    where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) x
        )
        """;

    const string POLL_QUESTIONS_DOCUMENT = """
        poll_questions_document as(
            select
        	    json_agg(
        		    json_build_object(
        			    'Id', id,
        			    'Text',  question_text,
        			    'Authoring', jsonb_build_object(
        				    'Id', publisher_id, 
        				    'Name', publisher_name,
        				    'CreatedDateTime', created_date_time,
                            'ChangedDateTime', changed_date_time
                        ),
                        'NodeTypeId', node_type_id,
                        'Title', title,
                        'HasBeenPublished', true,
        			    'PollOptions', poll_options
        		    )
        	    ) document
            from(
        	    select
        		    id,
        		    question_text,
        		    node_type_id,
                    title,
        		    created_date_time,
        		    changed_date_time,
        		    publisher_id,
        		    publisher_name,
        		    jsonb_agg(
        			    jsonb_build_object(
        				    'Text', option_text,
        				    'NumberOfVotes', number_of_votes,
        				    'Percentage', round(100 * (number_of_votes::numeric  / total), 0),
        				    'Delta', delta
        			    )
        		    ) poll_options
        	    from(
        		    select 
        		    stn.id,
        		    stn.text question_text,
        		    n.node_type_id,
                    n.title,
        		    n.created_date_time,
        		    n.changed_date_time,
        		    p.id publisher_id,
        		    p.name publisher_name,
        		    po.text option_text,
        		    po.number_of_votes,
        		    sum(number_of_votes) over() total,
        		    po.delta
        		    from poll_option po
        		    join poll_question pq on po.poll_question_id = pq.id
        		    join simple_text_node stn on stn.id = pq.id
        		    join node n on n.id = pq.id
        		    join publisher p on p.id = n.publisher_id
        		    join multi_question_poll_poll_question mqppq on mqppq.poll_question_id = pq.id
        		    join tenant_node tn on tn.node_id = mqppq.multi_question_poll_id
        		    where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        	    ) x
        	    group by 
        		    id,
                    title,
        		    question_text,
        		    node_type_id,
        		    created_date_time,
        		    changed_date_time,
        		    publisher_id,
        		    publisher_name
            ) x
        )
        """;

    const string PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT = """
        party_political_entity_relations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Party', jsonb_build_object(
        	                'Name', party_name,
        	                'Path', party_path
                        ),
                        'PoliticalEntity', jsonb_build_object(
        	                'Name', political_entity_name,
        	                'Path', political_entity_path
                        ),
                        'PartyPoliticalEntityRelationType',
                        jsonb_build_object(
        	                'Name', party_political_entity_relation_type_name,
        	                'Path', party_political_entity_relation_type_path
                        ),
                        'DateFrom', lower(date_range),
                        'DateTo', upper(date_range),
                        'DocumentProof', case
        	                when status4 is null or status4 = -1 then null
        	                else jsonb_build_object(
        		                'Name', document_proof_name,
        		                'Path', document_proof_path
        	                )
                        end
                    )
                ) document
            from(
            select
        		n2.title party_name,
        		case
        			when tn2.url_path is null then '/node/' || tn2.url_id
        			else tn2.url_path
        		end party_path,
        		n3.title political_entity_name,
        		case
        			when tn3.url_path is null then '/node/' || tn3.url_id
        			else tn3.url_path
        		end political_entity_path,
        		n4.title party_political_entity_relation_type_name,
        		case
        			when tn4.url_path is null then '/node/' || tn4.url_id
        			else tn4.url_path
        		end party_political_entity_relation_type_path,
        		pper.date_range,
        		case 
        			when tn.url_path is null then '/node/' || tn.url_id
        			else '/' || tn.url_path
        		end path,
        		n5.title document_proof_name,
        		case 
        			when tn5.id is null then null
        			when tn5.url_path is null then '/node/' || tn.url_id
        			else '/' || tn.url_path
        		end document_proof_path,
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
        					AND ugu.user_id = @user_id
        				)
        		end status,
        		case 
        			when tn3.publication_status_id = 0 then (
        				select
        					case 
        						when count(*) > 0 then 0
        						else -1
        					end status
        				from user_group_user_role_user ugu
        				join user_group ug on ug.id = ugu.user_group_id
        				WHERE ugu.user_group_id = 
        				case
        					when tn3.subgroup_id is null then tn3.tenant_id 
        					else tn3.subgroup_id 
        				end 
        				AND ugu.user_role_id = ug.administrator_role_id
        				AND ugu.user_id = @user_id
        			)
        			when tn3.publication_status_id = 1 then 1
        			when tn3.publication_status_id = 2 then (
        				select
        					case 
        						when count(*) > 0 then 1
        						else -1
        					end status
        				from user_group_user_role_user ugu
        				WHERE ugu.user_group_id = 
        					case
        						when tn3.subgroup_id is null then tn3.tenant_id 
        						else tn3.subgroup_id 
        					end
        					AND ugu.user_id = @user_id
        				)
        		end status2,
        		case 
        			when tn4.publication_status_id = 0 then (
        				select
        					case 
        						when count(*) > 0 then 0
        						else -1
        					end status
        				from user_group_user_role_user ugu
        				join user_group ug on ug.id = ugu.user_group_id
        				WHERE ugu.user_group_id = 
        				case
        					when tn4.subgroup_id is null then tn4.tenant_id 
        					else tn4.subgroup_id 
        				end 
        				AND ugu.user_role_id = ug.administrator_role_id
        				AND ugu.user_id = @user_id
        			)
        			when tn4.publication_status_id = 1 then 1
        			when tn4.publication_status_id = 2 then (
        				select
        					case 
        						when count(*) > 0 then 1
        						else -1
        					end status
        				from user_group_user_role_user ugu
        				WHERE ugu.user_group_id = 
        					case
        						when tn4.subgroup_id is null then tn4.tenant_id 
        						else tn4.subgroup_id 
        					end
        					AND ugu.user_id = @user_id
        				)
        		end status3,
        		case 
        			when tn5.publication_status_id = null then null
        			when tn5.publication_status_id = 0 then (
        				select
        					case 
        						when count(*) > 0 then 0
        						else -1
        					end status
        				from user_group_user_role_user ugu
        				join user_group ug on ug.id = ugu.user_group_id
        				WHERE ugu.user_group_id = 
        				case
        					when tn5.subgroup_id is null then tn5.tenant_id 
        					else tn5.subgroup_id 
        				end 
        				AND ugu.user_role_id = ug.administrator_role_id
        				AND ugu.user_id = @user_id
        			)
        			when tn5.publication_status_id = 1 then 1
        			when tn5.publication_status_id = 2 then (
        				select
        					case 
        						when count(*) > 0 then 1
        						else -1
        					end status
        				from user_group_user_role_user ugu
        				WHERE ugu.user_group_id = 
        					case
        						when tn5.subgroup_id is null then tn5.tenant_id 
        						else tn5.subgroup_id 
        					end
        					AND ugu.user_id = @user_id
        				)
        		end status4
        	from  node n
        	join tenant_node tn on tn.node_id = n.id
        	join party_political_entity_relation pper on pper.id = n.id 

        	join node n2 on n2.id = pper.party_id				
        	join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id

        	join node n3 on n3.id = pper.political_entity_id				
        	join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id

        	join node n4 on n4.id = pper.party_political_entity_relation_type_id
        	join tenant_node tn4 on tn4.node_id = n4.id and tn4.tenant_id = tn.tenant_id

        	left join node n5 on n5.id = pper.document_id_proof
        	left join tenant_node tn5 on tn4.node_id = n5.id and tn5.tenant_id = tn.tenant_id

        	where tn.tenant_id = @tenant_id and tn2.url_id = @url_id
        	) x
        	where status <> -1 and status2 <> -1 and status3 <> -1
        )	
        """;

    const string ORGANIZATION_CASES_DOCUMENT = """
        organization_cases_document as (
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'CaseTypeName', case_type_name,
        		    'PartyCases', party_cases
        	    )
            ) document
            from(
            select
            case_type_name,
            jsonb_agg(
        	    jsonb_build_object(
        		    'CasePartyTypeName', case_party_type_name,
        		    'Cases', cases
        	    )
            ) party_cases
            from(
        	    select
        	    case_type_name,
        	    case_party_type_name,
        	    jsonb_agg(jsonb_build_object(
        		    'Name', title,
        		    'Path', path
        	    )) cases
        	    from(
        		    select
        			    nt.name case_type_name,
        			    cpt.name case_party_type_name,
        			    n.title,
        			    case 
        				    when tn.url_path is null then '/node/' || tn.url_id
        				    else '/' || tn.url_path
        			    end path,
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
        						    AND ugu.user_id = @user_id
        					    )
        			    end status	
        		    from case_parties cp
        		    join case_case_parties ccp on ccp.case_parties_id = cp.id 
        		    join node n on n.id = ccp.case_id
        		    join case_party_type cpt on cpt.id= ccp.case_party_type_id
        		    join case_parties_organization o on o.case_parties_id = cp.id
        		    join tenant_node tn2 on tn2.node_id = o.organization_id
        		    join node_type nt on nt.id = n.node_type_id
        		    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		    where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        	    )x 
        	    where status <> -1
        	    group by case_type_name, case_party_type_name
            ) x
            group by case_type_name
            ) x        
        )
        """;
    const string PERSON_CASES_DOCUMENT = """
        person_cases_document as (
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'CaseTypeName', case_type_name,
        		    'PartyCases', party_cases
        	    )
            ) document
            from(
            select
            case_type_name,
            jsonb_agg(
        	    jsonb_build_object(
        		    'CasePartyTypeName', case_party_type_name,
        		    'Cases', cases
        	    )
            ) party_cases
            from(
        	    select
        	    case_type_name,
        	    case_party_type_name,
        	    jsonb_agg(jsonb_build_object(
        		    'Name', title,
        		    'Path', path
        	    )) cases
        	    from(
        		    select
        			    nt.name case_type_name,
        			    cpt.name case_party_type_name,
        			    n.title,
        			    case 
        				    when tn.url_path is null then '/node/' || tn.url_id
        				    else '/' || tn.url_path
        			    end path,
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
        						    AND ugu.user_id = @user_id
        					    )
        			    end status	
        		    from case_parties cp
        		    join case_case_parties ccp on ccp.case_parties_id = cp.id 
        		    join node n on n.id = ccp.case_id
        		    join case_party_type cpt on cpt.id= ccp.case_party_type_id
        		    join case_parties_person o on o.case_parties_id = cp.id
        		    join tenant_node tn2 on tn2.node_id = o.person_id
        		    join node_type nt on nt.id = n.node_type_id
        		    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		    where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        	    )x 
        	    where status <> -1
        	    group by case_type_name, case_party_type_name
            ) x
            group by case_type_name
            ) x        
        )
        """;

    const string SUBTOPICS_DOCUMENT = """
        subtopics_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Name', "name", 
                        'Path', url_path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end url_path,
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
                                AND ugu.user_id = @user_id
                            )
                    end status	
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = tt.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_child = t1.id
        		join term t2 on t2.id = th.term_id_parent
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.url_id = @url_id
            ) an
            where an.status <> -1
        )
        """;

    const string SUPERTOPICS_DOCUMENT = """
        supertopics_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Name', "name", 
                        'Path', url_path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end url_path,
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
                                AND ugu.user_id = @user_id
                            )
                    end status	
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = tt.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_parent = t1.id
        		join term t2 on t2.id = th.term_id_child
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.url_id = @url_id
            ) an
            where an.status <> -1
        )
        """;

    const string CASE_CASE_PARTIES_DOCUMENT = """
        case_case_parties_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'PartyTypeName', "name",
                        'OrganizationsText', organizations_text,
                        'PersonsText', persons_text,
                        'Organizations', organizations,
                        'Persons', persons
                    )
                ) document
            from(
                select
                    cpt.name,
                    cp.organizations organizations_text,
                    cp.persons persons_text,
                    (
                        select
                            jsonb_agg(
                                jsonb_build_object(
                                    'Name', organization_name,
                                    'Path', organization_path
                                )
                            ) organizations
                        from(
                            select
                                case_parties_id,
                                n2.title organization_name,
                                case
                                    when tn2.publication_status_id = 0 then (
                                        select
                                            case 
                                                when count(*) > 0 then 0
                                                else -1
                                            end status
                                        from user_group_user_role_user ugu
                                        join user_group ug on ug.id = ugu.user_group_id
                                        WHERE ugu.user_group_id = 
                                        case
                                            when tn2.subgroup_id is null then tn2.tenant_id 
                                            else tn2.subgroup_id 
                                        end 
                                        AND ugu.user_role_id = ug.administrator_role_id
                                        AND ugu.user_id = @user_id
                                    )
                                    when tn2.publication_status_id = 1 then 1
                                    when tn2.publication_status_id = 2 then (
                                        select
                                            case 
                                                when count(*) > 0 then 1
                                                else -1
                                            end status
                                        from user_group_user_role_user ugu
                                        WHERE ugu.user_group_id = 
                                        case
                                            when tn2.subgroup_id is null then tn2.tenant_id 
                                            else tn2.subgroup_id 
                                        end
                                        AND ugu.user_id = @user_id
                                    )
                                 end status,
                                case 
                                    when tn2.url_path is null then '/node/' || tn2.url_id
                                    else tn2.url_path
                                end organization_path
                            from case_parties_organization cpo 
                            join node n2 on n2.id = cpo.organization_id
                            join tenant_node tn2 on tn2.node_id = n2.id
                            where tn2.tenant_id = @tenant_id and cpo.case_parties_id = cp.id
                        ) x where status <> -1
                    ) organizations,
                    (
                        select
                            jsonb_agg(jsonb_build_object(
                                'Name', person_name,
                                'Path', person_path
                            )) persons
                        from(
                            select
                                case_parties_id,
                                n3.title person_name,
                                case
                                    when tn3.publication_status_id = 0 then (
                                        select
                                            case 
                                                when count(*) > 0 then 0
                                                else -1
                                            end status
                                        from user_group_user_role_user ugu
                                        join user_group ug on ug.id = ugu.user_group_id
                                        WHERE ugu.user_group_id = 
                                        case
                                            when tn3.subgroup_id is null then tn3.tenant_id 
                                            else tn3.subgroup_id 
                                        end 
                                        AND ugu.user_role_id = ug.administrator_role_id
                                        AND ugu.user_id = @user_id
                                    )
                                    when tn3.publication_status_id = 1 then 1
                                    when tn3.publication_status_id = 2 then (
                                        select
                                            case 
                                                when count(*) > 0 then 1
                                                else -1
                                            end status
                                        from user_group_user_role_user ugu
                                        WHERE ugu.user_group_id = 
                                        case
                                            when tn3.subgroup_id is null then tn3.tenant_id 
                                            else tn3.subgroup_id 
                                        end
                                        AND ugu.user_id = @user_id
                                    )
                                end status,
                                case 
                                    when tn3.url_path is null then '/node/' || tn3.url_id
                                    else tn3.url_path
                                end person_path
                            from case_parties_person cpp 
                            join node n3 on n3.id = cpp.person_id
                            join tenant_node tn3 on tn3.node_id = n3.id
                            where tn3.tenant_id = @tenant_id and cpp.case_parties_id = cp.id
                        ) x where status <> -1
                    ) persons
                from node n
                join case_case_parties ccp on ccp.case_id = n.id
                join case_parties cp on cp.id = ccp.case_parties_id
                join case_party_type cpt on cpt.id = ccp.case_party_type_id
                join tenant_node tn on tn.node_id = n.id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) x
        )
        """;

    const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                jsonb_agg(jsonb_build_object(
        			'Id', "id",
        			'Street', street,
        			'Additional', additional,
        			'City', city,
        			'PostalCode', postal_code,
        			'Subdivision', subdivision,
        			'Country', country,
                    'Latitude', latitude,
                    'Longitude', longitude
        		)) document
            from(
                select 
                l.id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                jsonb_build_object(
                	'Path', case when tn2.url_path is null then '/node/' || tn2.url_id else '/' || tn2.url_path end,
                	'Name', s.name
                ) subdivision,
                jsonb_build_object(
                	'Path', case when tn3.url_path is null then '/node/' || tn3.url_id else '/' || tn3.url_path end,
                	'Name', nc.title
                ) country,
                l.latitude,
                l.longitude
                from "location" l
                join location_locatable ll on ll.location_id = l.id
                join node nc on nc.id = l.country_id
                join subdivision s on s.id = l.subdivision_id
                join tenant_node tn on tn.node_id = ll.locatable_id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
                join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = @tenant_id
                join tenant_node tn3 on tn3.node_id = nc.id and tn3.tenant_id = @tenant_id
            )x
        )
        """;

    const string INTER_ORGANIZATIONAL_RELATION_DOCUMENT = """
        inter_organizational_relation_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'OrganizationFrom', organization_from,
        	        'OrganizationTo', organization_to,
        	        'InterOrganizationalRelationType', inter_organizational_relation_type,
        	        'GeographicEntity', geographic_entity,
        	        'DateFrom', lower(date_range),
                    'DateTo', upper(date_range),
        	        'MoneyInvolved', money_involved,
        	        'NumberOfChildrenInvolved', number_of_children_involved,
        	        'Description', description,
        	        'Direction', direction
                )) document
            from(
        	    select
        	    jsonb_build_object(
        		    'Name', organization_name_from,
        		    'Path', organization_path_from
        	    ) organization_from,
        	    jsonb_build_object(
        		    'Name', organization_name_to,
        		    'Path', organization_path_to
        	    ) organization_to,
        	    jsonb_build_object(
        		    'Name', inter_organizational_relation_type_name,
        		    'Path', inter_organizational_relation_type_path
        	    ) inter_organizational_relation_type,
        	    case
        	    when geographic_entity_name is null then null
        	    else jsonb_build_object(
        			    'Name', geographic_entity_name,
        			    'Path', geographic_entity_path) 
        	    end geographic_entity,
        	    date_range,
        	    money_involved,
        	    number_of_children_involved,
        	    description,
        	    direction
        	    from(
        		    select
        		    organization_name_from,
        		    case 
        			    when organization_path_from is null then '/node/' || organization_id_from
        			    else '/' || organization_path_from
        		    end organization_path_from,
        		    organization_name_to,
        		    case 
        			    when organization_path_to is null then '/node/' || organization_id_to
        			    else '/' || organization_path_to
        		    end organization_path_to,
        		    inter_organizational_relation_type_name,
        		    case 
        			    when inter_organizational_relation_type_path is null then '/node/' || inter_organizational_relation_type_id
        			    else '/' || inter_organizational_relation_type_path
        		    end inter_organizational_relation_type_path,
        		    geographic_entity_name,
        		    case
        			    when geographic_entity_path is null and geographic_entity_id is null then null
        			    when geographic_entity_path is null  then '/node/' || geographic_entity_id
        			    else '/' || geographic_entity_path
        		    end geographic_entity_path,
        		    date_range,
        		    money_involved,
        		    number_of_children_involved,
        		    description,
        		    direction
        		    from(
        			    select
        			    distinct
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    1 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    1 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    2 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id 
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where not rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    3 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id 
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where not rt.is_symmetric
        		    ) x
        	    ) x
            )x
        )
        """;
    const string INTER_PERSONAL_RELATION_DOCUMENT = """
        inter_personal_relation_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'PersonFrom', person_from,
        	        'PersonTo', person_to,
        	        'InterPersonalRelationType', inter_personal_relation_type,
        	        'DateFrom', lower(date_range),
                    'DateTo', upper(date_range),
        	        'Direction', direction
                )) document
            from(
        	    select
        	    jsonb_build_object(
        		    'Name', person_name_from,
        		    'Path', person_path_from
        	    ) person_from,
        	    jsonb_build_object(
        		    'Name', person_name_to,
        		    'Path', person_path_to
        	    ) person_to,
        	    jsonb_build_object(
        		    'Name', inter_personal_relation_type_name,
        		    'Path', inter_personal_relation_type_path
        	    ) inter_personal_relation_type,
        	    date_range,
        	    direction
        	    from(
        		    select
        		    person_name_from,
        		    case 
        			    when person_path_from is null then '/node/' || person_id_from
        			    else '/' || person_path_from
        		    end person_path_from,
        		    person_name_to,
        		    case 
        			    when person_path_to is null then '/node/' || person_id_to
        			    else '/' || person_path_to
        		    end person_path_to,
        		    inter_personal_relation_type_name,
        		    case 
        			    when inter_personal_relation_type_path is null then '/node/' || inter_personal_relation_type_id
        			    else '/' || inter_personal_relation_type_path
        		    end inter_personal_relation_type_path,
        		    date_range,
        		    direction
        		    from(
        			    select
        			    distinct
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn3.url_id inter_personal_relation_type_id,
        			    tn3.url_path inter_personal_relation_type_path,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    1 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn3.url_id inter_personal_relation_type_id,
        			    tn3.url_path inter_personal_relation_type_path,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    1 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn3.url_id inter_person_relational_type_id,
        			    tn3.url_path inter_personal_relation_type_path,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    2 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id 
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    where not rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn3.url_id inter_person_relation_type_id,
        			    tn3.url_path inter_person_relation_type_path,
        			    n3.title inter_person_relation_type_name,
        			    r.date_range,
        			    3 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id 
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    where not rt.is_symmetric
        		    ) x
        	    ) x
            )x
        )
        """;

    const string SEE_ALSO_DOCUMENT = """
        see_also_document AS(
            SELECT
                jsonb_agg(
                    jsonb_build_object(
                        'Path', sa.path,
                        'Name', sa.title
                    )
                ) document
            FROM (
                SELECT 
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end path,
                    n2.title
                FROM authenticated_node an
                JOIN node_term nt1 on nt1.node_id = an.node_id
                JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
                JOIN tenant_node tn on tn.node_id = nt2.node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
                JOIN node n2 on n2.id = tn.node_id
                GROUP BY an.node_id, tn.node_id, tn.url_path, tn.url_id, n2.title
                HAVING COUNT(tn.node_id) > 2 
                ORDER BY count(tn.node_id) desc, n2.title
                LIMIT 10
            ) sa
        )
        """;

    const string TAGS_DOCUMENT = """
        tags_document AS (
            SELECT
                jsonb_agg(
                    jsonb_build_object(
                        'Path',  t.path,
                        'Name', t.name
                    )
                ) as document
            FROM (
                select
                    case 
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path,
                    t.name
                FROM node_term nt 
                JOIN tenant_node tn on tn.node_id = nt.node_id
                JOIN term t on t.id = nt.term_id
                JOIN tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id and tn2.publication_status_id = 1
                WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
            ) t
        )
        """;

    const string ORGANIZATIONS_OF_COUNTRY_DOCUMENT = """
        organizations_of_country_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
        	            'OrganizationTypeName', organization_type,
        	            'Organizations', organizations
                    )
                ) document
            from(
                select
        	        organization_type,
        	        jsonb_agg(
        	            jsonb_build_object(
        		            'Name', organization_name,
        		            'Path', "path"
        	            )
                    ) organizations
                from(
                    select
                        n2.title organization_type,
                        n.title organization_name,
                        case 
        	                when tn2.url_path is null then '/node/' || tn2.url_id
        	                else '/' || tn2.url_path
                        end "path"
                    from node n
                    join tenant_node tn2 on tn2.node_id = n.id and tn2.tenant_id = @tenant_id
                    join organization o on o.id = n.id
                    join organization_organization_type oot on oot.organization_id = o.id
                    join organization_type ot on ot.id = oot.organization_type_id
                    join node n2 on n2.id = ot.id
                    join location_locatable ll on ll.locatable_id = n.id
                    join "location" l on l.id = ll.location_id
                    join tenant_node tn on tn.url_id = @url_id and tn.node_id = l.country_id and tn.tenant_id = @tenant_id
        	        ) x
                group by x.organization_type
            ) x
        )
        """;

    const string DOCUMENTS_DOCUMENT = """
        documents_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Path', path,
                        'Title', title,
                        'PublicationDate', publication_date,
                        'SortOrder', sort_order
                    )
                ) document
            from(
                select
                    path,
                    title,
                    publication_date,
                    row_number() over(order by sort_date desc) sort_order
                from(
                    select
                        case 
        	                when tn2.url_path is null then '/node/' || tn2.url_id
        	                else '/' || tn2.url_path
                        end path,
                        n2.title,
                        case 
        	                when d.publication_date is not null then d.publication_date
        	                else lower(d.publication_date_range)
                        end sort_date,
                        case 
        	                when d.publication_date is not null 
        		                then extract(year from d.publication_date) || ' ' || to_char(d.publication_date, 'Month') || ' ' || extract(DAY FROM d.publication_date)
        	                when extract(month from lower(d.publication_date_range)) = extract(month from upper(d.publication_date_range)) 
        		                then extract(year from lower(d.publication_date_range)) || ' ' || to_char(lower(d.publication_date_range), 'Month') 
        	                when extract(year from lower(d.publication_date_range)) = extract(year from upper(d.publication_date_range)) 
        		                then extract(year from lower(d.publication_date_range))  || ''
        	                else ''
                        end publication_date
                    from documentable_document dd
                    join tenant_node tn on tn.url_id = @url_id and tn.tenant_id = @tenant_id and tn.node_id = dd.documentable_id
                    join tenant_node tn2 on tn2.node_id = dd.document_id and tn2.tenant_id = @tenant_id
                    join node n2 on n2.Id = tn2.node_id
                    join "document" d on d.id = n2.id
                ) x
            ) docs
        )
        """;

    const string BLOG_POST_BREADCRUM_DOCUMENT = """
        blog_post_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/blogs', 
                    'blogs', 
                    1
                UNION
                SELECT 
                    '/blog+/' || p.id, 
                    p.name || '''s blog', 
                    2
                FROM authenticated_node an
                JOIN publisher p on p.id = an.publisher_id
                WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string PAGE_BREADCRUM_DOCUMENT = """
        page_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                ) 
            ) document
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
            ) bces
        )
        """;

    const string ARTICLE_BREADCRUM_DOCUMENT = """
        article_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/articles', 
                    'aricles', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string DOCUMENT_BREADCRUM_DOCUMENT = """
        document_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/documents', 
                    'documents', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string TOPICS_BREADCRUM_DOCUMENT = """
        topics_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/topics', 
                    'topics', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string ORGANIZATION_BREADCRUM_DOCUMENT = """
        organization_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/organizations', 
                    'organizations', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string COUNTRY_SUBDIVISIONS_DOCUMENT = """
        country_subdivisions_document as (
            select
        	    jsonb_agg(jsonb_build_object(
        		    'Name', subdivision_type_name,
        		    'Subdivisions', subdivisions
        	    )) document
        	from(
                select
        	        subdivision_type_name,
        	        jsonb_agg(jsonb_build_object(
        		        'Name', subdivision_name,
        		        'Path', case 
        			        when url_path is null then '/node/' || url_id
        			        else url_path
        		        end
        		        )) "subdivisions"
                FROM(
        	        select
        		        distinct
        		        n.title subdivision_type_name, 
        		        s.name subdivision_name,
        		        tn2.url_path,
        		        tn2.url_id
        	        from country c
        	        join tenant_node tn on tn.node_id = c.id and tn.tenant_id = 1 and tn.url_id = @url_id
        	        join tenant t on t.id = tn.tenant_id
        	        join subdivision s on s.country_id = c.id
        	        join node n on n.id = s.subdivision_type_id
        	        join tenant_node tn2 on tn2.node_id = s.id and tn.tenant_id = 1
        	        join term tp on tp.nameable_id = c.id and tp.vocabulary_id = t.vocabulary_id_tagging
        	        join term tc on tc.nameable_id = s.id and tc.vocabulary_id = t.vocabulary_id_tagging
        	        join term_hierarchy th on th.term_id_parent = tp.id and th.term_id_child = tc.id
                ) x
                GROUP BY subdivision_type_name
            ) x
        )
        """;

    const string COUNTRY_BREADCRUM_DOCUMENT = """
        country_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/countries', 
                    'countries', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string ABUSE_CASE_BREADCRUM_DOCUMENT = """
        abuse_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/abuse_cases', 
                    'Abuse cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string DOCUMENTABLES_DOCUMENT = """
        documentables_document as (
            select
                jsonb_agg(jsonb_build_object('Name', documentable_name, 'Path', documentable_path)) document
                from(
                select
                n.title documentable_name,
                case 
        	        when tn.url_path is null then '/node/' || tn.url_id
        	        else tn.url_path
                end documentable_path,
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
        			        AND ugu.user_id = @user_id
        		        )
                end status	
            from documentable_document dd
            join tenant_node tn2 on tn2.node_id = dd.document_id
            join node n on n.id = dd.documentable_id
            join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
            where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
            ) x 
            where status <> -1
        )
        """;

    const string CHILD_TRAFFICKING_CASE_BREADCRUM_DOCUMENT = """
        child_trafficking_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/child_trafficking_cases', 
                    'Child trafficing cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string COERCED_ADOPTION_CASE_BREADCRUM_DOCUMENT = """
        coerced_adoption_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/coerced_adoption_cases', 
                    'Coerced adoption cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string DEPORTATION_CASE_BREADCRUM_DOCUMENT = """
        deportation_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/deportation_cases', 
                    'Deportation cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string DISRUPTED_PLACEMENT_CASE_BREADCRUM_DOCUMENT = """
        disrupted_placement_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/disrupted_placement_cases', 
                    'Disrupted placememt cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string FATHERS_RIGHTS_VIOLATION_CASE_BREADCRUM_DOCUMENT = """
        fathers_rights_violation_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/fathers_rights_violation_cases', 
                    'Father''s rights violation cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string WRONGFUL_MEDICATION_CASE_BREADCRUM_DOCUMENT = """
        wrongful_medication_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/wrongful_medication_cases', 
                    'Wrongful medicatiion cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string WRONGFUL_REMOVAL_CASE_BREADCRUM_DOCUMENT = """
        wrongful_removal_case_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/wrongful_removal_cases', 
                    'Wrongful removal cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string POLL_BREADCRUM_DOCUMENT = """
        poll_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Name', "name"
                )
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/polls', 
                    'Polls', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string COMMENTS_DOCUMENT = """
        comments_document AS (
            SELECT jsonb_agg(tree) document
            FROM (
                SELECT to_jsonb(sub) AS tree
                FROM (
        	        SELECT 
        		        c.id AS "Id", 
        		        c.node_status_id AS "NodeStatusId",
        		        jsonb_build_object(
        			        'Id', p.id, 
        			        'Name', p.name,
                            'CreatedDateTime', c.created_date_time,
                            'ChangedDateTime', c.created_date_time
                        ) AS "Authoring",
        		        c.title AS "Title", 
        		        c.text AS "Text", 
        		        c.comment_id_parent AS "CommentIdParent"
        	        FROM comment c
        	        JOIN publisher p on p.id = c.publisher_id
                    JOIN authenticated_node an on an.node_id = c.node_id
        	        WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
                ) sub
        	) agg        
        )
        """;

    const string ADOPTION_IMPORTS_DOCUMENT = """
        adoption_imports_document as(
            select
                jsonb_build_object(
                    'StartYear', start_year,
                    'EndYear', end_year,
                    'Imports', jsonb_agg(
                        jsonb_build_object(
                           'CountryFrom', name,
                            'RowType', row_type,
                            'Values', y
                        )
                    )
                ) document
            from(
                select
        	        name,
        	        row_type,
        	        start_year,
        	        end_year,
        	        jsonb_agg(
                        jsonb_build_object(
            		        'Year', "year",
            		        'NumberOfChildren', number_of_children
            	        )
                    ) y
                from(
        	        select
        		        row_number() over () id,
        		        case 
        			        when sub is not null then 1
        			        when origin is not null then 2
        			        else 3
        		        end row_type,
        		        case 
        			        when sub is not null then sub
        			        when origin is not null then origin
        			        else null
        		        end name,
        		        number_of_children,
        		        case when "year" is null then 10000
        		        else "year"
        		        end "year",
        		        min("year") over() start_year,
        		        max("year") over() end_year
        	        from(
        		        select
        		        distinct
        		        t.*
        		        from(
        			        select
        			        * 
        			        from
        			        (
        				        select
        					        *,
        					        SUM(number_of_children_involved) over (partition by country_to, "year") toty,
        					        SUM(number_of_children_involved) over (partition by country_to, region_from, "year") totry,
        					        SUM(number_of_children_involved) over (partition by country_to, country_from, "year") totcy,
        					        SUM(number_of_children_involved) over (partition by country_to) tot,
        					        SUM(number_of_children_involved) over (partition by country_to, region_from) totr,
        					        SUM(number_of_children_involved) over (partition by country_to, country_from) totc
        				        from(
        					        select
        						        nto.title country_to,
        						        rfm.title region_from,
        						        nfm.title country_from,
        						        case when 
        							        icr.number_of_children_involved is null then 0
        							        else icr.number_of_children_involved
        						        end number_of_children_involved,
        						        extract('year' from upper(cr.date_range)) "year"
        					        from country_report cr
                                    join node nto on nto.id = cr.country_id
        					        join tenant_node tn on tn.node_id = nto.id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
        					        join top_level_country cto on cto.id = nto.id
        					        join top_level_country cfm on true 
        					        join node rfm on rfm.id = cfm.global_region_id
        					        join node nfm on nfm.id = cfm.id
                                    join tenant_node tn2 on tn2.tenant_id = @tenant_id and tn2.url_id = 144
        					        LEFT join inter_country_relation icr on icr.country_id_from = cto.id and cfm.id = icr.country_id_to and icr.date_range = cr.date_range and icr.inter_country_relation_type_id = tn2.node_id
        					        WHERE tn.url_id = @url_id 

        				        ) a
        			        ) a
        			        where totc <> 0
        			        ORDER BY country_to, region_from, country_from, "year"
        		        ) c
        		        cross join lateral(
        			        values
        			        (null, null, toty, c."year"),
        			        (region_from, null, totry, c."year"),
        			        (region_from, country_from, totcy, c."year"),
        			        (null, null, tot, null),
        			        (region_from, null, totr, null),
        			        (region_from, country_from, totc, null)
        		        ) as t(origin, sub, number_of_children, "year")
        		        order by t.origin, t.sub, t."year"
        	        ) x
                ) imports
                group by imports.name, row_type, start_year, end_year
                order by min(id)
            ) y
            group by start_year, end_year
        )
        """;

    const string ORGANIZATION_TYPES_DOCUMENT = """
        organization_types_document AS (
            select
                jsonb_agg(jsonb_build_object(
        	        'Name', "name",
        	        'Path', "path"
                )) "document"
            from(
            select
            n.title "name",
            case 
        	    when tn2.url_path is null then '/node/' || tn2.url_id
        	    else tn2.url_path
            end path	
            from organization_type ot
            join node n on n.id = ot.id
            join organization_organization_type oot on oot.organization_type_id = ot.id
            join organization o on o.id = oot.organization_id
            join tenant_node tn1 on tn1.node_id = o.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
            join tenant_node tn2 on tn2.node_id = ot.id and tn2.tenant_id = @tenant_id 
            ) x
        )
        """;
    const string PROFESSIONS_DOCUMENT = """
        professions_document AS (
            select
                jsonb_agg(jsonb_build_object(
        	        'Name', "name",
        	        'Path', "path"
                )) "document"
            from(
            select
            n.title "name",
            case 
        	    when tn2.url_path is null then '/node/' || tn2.url_id
        	    else tn2.url_path
            end path	
            from profession ot
            join node n on n.id = ot.id
            join professional_role oot on oot.profession_id = ot.id
            join person o on o.id = oot.person_id
            join tenant_node tn1 on tn1.node_id = o.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
            join tenant_node tn2 on tn2.node_id = ot.id and tn2.tenant_id = @tenant_id 
            ) x
        )
        """;


    const string BASIC_COUNTRY_DOCUMENT = """
        basic_country_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'AdoptionImports', (SELECT document FROM adoption_imports_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_country_document),
                'SubdivisionTypes', (SELECT document FROM country_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document)
            ) document
            FROM (
                 SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    nm.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join top_level_country tlc on tlc.id = an.node_id 
                join nameable nm on nm.id = an.node_id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;


    const string DOCUMENT_DOCUMENT = """
        document_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title,
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id,
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'DateTime', publication_date,
                'DateTimeFrom', lower(publication_date_range),
                'DateTimeTo', upper(publication_date_range),
                'SourceUrl', source_url,
                'DocumentType', document_type,
                'BreadCrumElements', (SELECT document FROM document_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documentables', (SELECT document FROM documentables_document),
                'Files', (SELECT document FROM files_document)
            ) document
            FROM(
                SELECT
                    an.url_id,
                    an.node_type_id,
                    an.title,
                    an.created_date_time,
                    an.changed_date_time,
                    stn.text,
                    an.publisher_id,
                    p.name publisher_name,
                    an.has_been_published,
                    stn.publication_date,
                    stn.publication_date_range,
                    stn.source_url,
                    case 
                        when dt.id is null then null
                        else jsonb_build_object(
                            'Id', dt.id,
                            'Name', dt.name,
                            'Path', dt.path
                        )
                    end document_type
                FROM authenticated_node an
                join document stn on stn.id = an.node_id
                left join (
                    select 
                    dt.id,
                    n.title name,
                    case when tn.url_path is null then '/node/' || tn.url_id
        	            else '/' || tn.url_path
                    end path
                    from document_type dt
                    join node n on n.id = dt.id
                    join tenant_node tn on tn.node_id = dt.id and tn.tenant_id = @tenant_id
                ) dt on dt.id = stn.document_type_id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        )
        """;

    const string PERSON_ORGANIZATION_RELATIONS_DOCUMENT = """
        person_organization_relations_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'Person', jsonb_build_object(
        		        'Name', person_name,
        		        'Path', path
        	        ),
        	        'RelationTypeName', relation_type_name,
        	        'DateFrom', lower(date_range),
        	        'DateTo', upper(date_range)
                )) document
            from(
                select
        			n.title person_name,
        			n2.title relation_type_name,
        			por.date_range,
        			case 
        				when tn.url_path is null then '/node/' || tn.url_id
        				else '/' || tn.url_path
        			end path,
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
        						AND ugu.user_id = @user_id
        					)
        			end status	
        		from  node n
        		join person pe  on pe.id = n.id
        		join person_organization_relation por on por.person_id = pe.Id
        		join node n2 on n2.id = por.person_organization_relation_type_id
        		join tenant_node tn2 on tn2.node_id = por.organization_id
        		join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        	) x
        	where x.status <> -1
        )
        """;
    const string ORGANIZATION_PERSON_RELATIONS_DOCUMENT = """
        organization_person_relations_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'Organization', jsonb_build_object(
        		        'Name', organization_name,
        		        'Path', path
        	        ),
        	        'RelationTypeName', relation_type_name,
        	        'DateFrom', lower(date_range),
        	        'DateTo', upper(date_range)
                )) document
            from(
                select
        			n.title organization_name,
        			n2.title relation_type_name,
        			por.date_range,
        			case 
        				when tn.url_path is null then '/node/' || tn.url_id
        				else '/' || tn.url_path
        			end path,
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
        						AND ugu.user_id = @user_id
        					)
        			end status	
        		from  node n
        		join organization pe  on pe.id = n.id
        		join person_organization_relation por on por.organization_id = pe.Id
        		join node n2 on n2.id = por.person_organization_relation_type_id
        		join tenant_node tn2 on tn2.node_id = por.person_id
        		join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        	) x
        	where x.status <> -1
        )
        """;

    const string ORGANIZATION_DOCUMENT = """
        organization_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'WebsiteUrl', n.website_url,
                'EmailAddress', n.email_address,
                'Established', n.established,
                'Terminated', n.terminated,
                'BreadCrumElements', (SELECT document FROM organization_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document FROM documents_document),
                'OrganizationTypes', (SELECT document FROM organization_types_document),
                'Locations', (SELECT document FROM locations_document),
                'InterOrganizationalRelations', (SELECT document FROM inter_organizational_relation_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'PartyCaseTypes', (SELECT document from organization_cases_document),
                'PersonOrganizationRelations', (SELECT document from person_organization_relations_document),
                'PartyPoliticalEntityRelations', (SELECT document from party_political_entity_relations_document),
                'Files', (SELECT document FROM files_document)
            ) document
            FROM (
                 SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    nm.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published,
                    o.website_url,
                    o.email_address,
                    o.established,
                    o.terminated
                FROM authenticated_node an
                join organization o on o.id = an.node_id 
                join nameable nm on nm.id = an.node_id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string PERSON_DOCUMENT = """
        person_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'DateOfBirth', date_of_birth,
                'DateOfDeath', date_of_death,
                'FirstName', first_name,
                'LastName', last_name,
                'FullName', full_name,
                'Suffix', suffix,
                'NickName', nick_name,
                'MiddleName', middle_name,
                'PortraitFilePath', portrait_file_path,
                'BreadCrumElements', (SELECT document FROM organization_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document FROM documents_document),
                'Professions', (SELECT document FROM professions_document),
                'Locations', (SELECT document FROM locations_document),
                'InterPersonalRelations', (SELECT document FROM inter_personal_relation_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'PartyCaseTypes', (SELECT document from person_cases_document),
                'OrganizationPersonRelations', (SELECT document from organization_person_relations_document),
                'PartyPoliticalEntityRelations', (SELECT document from party_political_entity_relations_document),
                'Files', (SELECT document FROM files_document)
            ) document
            FROM (
                 SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    nm.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published,
                    o.date_of_birth,
                    o.date_of_death,
                    o.first_name,
                    o.middle_name,
                    o.last_name,
                    o.full_name,
                    o.suffix,
                    o.nick_name,
                    f.path portrait_file_path
                FROM authenticated_node an
                join person o on o.id = an.node_id 
                left join file f on f.id = o.file_id_portrait
                join nameable nm on nm.id = an.node_id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string BLOG_POST_DOCUMENT = """
        blog_post_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM blog_post_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Files', (SELECT document FROM files_document)
            ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string SINGLE_QUESTION_POLL_DOCUMENT = """
        single_question_poll_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'Question', question,
                'DateTimeClosure', date_time_closure,
                'PollStatusId', poll_status_id,
                'BreadCrumElements', (SELECT document FROM poll_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Files', (SELECT document FROM files_document),
                'PollOptions', (SELECT document FROM poll_options_document)
            ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published,
                    pq.question,
                    pl.date_time_closure,
                    pl.poll_status_id
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                join poll pl on pl.id = an.node_id 
                join poll_question pq on pq.id = pl.id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string MULTIPLE_QUESTION_POLL_DOCUMENT = """
        multi_question_poll_document AS (
            SELECT 
                jsonb_build_object(
                'Id', n.url_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'DateTimeClosure', date_time_closure,
                'PollStatusId', poll_status_id,
                'BreadCrumElements', (SELECT document FROM poll_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Files', (SELECT document FROM files_document),
                'PollQuestions', (SELECT document FROM poll_questions_document)
            ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published,
                    pl.date_time_closure,
                    pl.poll_status_id
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                join poll pl on pl.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string ARTICLE_DOCUMENT = """
        article_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Text', n.text,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM article_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string PAGE_DOCUMENT = """
        page_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Text', n.text,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM page_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string BASIC_NAMEABLE_DOCUMENT = """
        basic_nameable_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM topics_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'SubTopListItemics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join basic_nameable bn on bn.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string ABUSE_CASE_DOCUMENT = """
        abuse_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM abuse_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join abuse_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string CHILD_TRAFFICKING_CASE_DOCUMENT = """
        child_trafficking_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM child_trafficking_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join child_trafficking_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string COERCED_ADOPTION_CASE_DOCUMENT = """
        coerced_adoption_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM coerced_adoption_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join coerced_adoption_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string DEPORTATION_CASE_DOCUMENT = """
        deportation_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM deportation_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join deportation_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string DISRUPTED_PLACEMENT_CASE_DOCUMENT = """
        disrupted_placement_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM disrupted_placement_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join disrupted_placement_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string FATHERS_RIGHTS_VIOLATION_CASE_DOCUMENT = """
        fathers_rights_violation_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM fathers_rights_violation_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join fathers_rights_violation_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string WRONGFUL_MEDICATION_CASE_DOCUMENT = """
        wrongful_medication_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM wrongful_medication_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join wrongful_medication_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string WRONGFUL_REMOVAL_CASE_DOCUMENT = """
        wrongful_removal_case_document AS (
            SELECT 
                jsonb_build_object(
                    'Id', n.url_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM wrongful_removal_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    an.url_id, 
                    an.node_type_id,
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join wrongful_removal_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string NODE_DOCUMENT = """
        node_document AS (
            SELECT
                an.node_type_id,
                case
                    when an.node_type_id = 10 then (select document from document_document)
                    when an.node_type_id = 13 then (select document from basic_country_document)
                    when an.node_type_id = 23 then (select document from organization_document)
                    when an.node_type_id = 24 then (select document from person_document)
                    when an.node_type_id = 26 then (select document from abuse_case_document)
                    when an.node_type_id = 29 then (select document from child_trafficking_case_document)
                    when an.node_type_id = 30 then (select document from coerced_adoption_case_document)
                    when an.node_type_id = 31 then (select document from deportation_case_document)
                    when an.node_type_id = 32 then (select document from fathers_rights_violation_case_document)
                    when an.node_type_id = 33 then (select document from wrongful_medication_case_document)
                    when an.node_type_id = 34 then (select document from wrongful_removal_case_document)
                    when an.node_type_id = 35 then (select document from blog_post_document)
                    when an.node_type_id = 36 then (select document from article_document)
                    when an.node_type_id = 41 then (select document from basic_nameable_document)
                    when an.node_type_id = 42 then (select document from page_document)
                    when an.node_type_id = 44 then (select document from disrupted_placement_case_document)
                    when an.node_type_id = 53 then (select document from single_question_poll_document)
                    when an.node_type_id = 54 then (select document from multi_question_poll_document)
                end document
            FROM authenticated_node an 
            WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
        ) 
        """;

}
