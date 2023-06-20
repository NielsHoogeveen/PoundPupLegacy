namespace PoundPupLegacy.EditModel.Readers;

internal static class SharedSql
{
    internal const string NODE_UPDATE_CTE = $"""
        WITH
        {TAGGING_VOCABULARY},
        {TAGS_DOCUMENT_UPDATE},
        {TENANT_NODES_DOCUMENT},
        {TENANTS_DOCUMENT},
        {ATTACHMENTS_DOCUMENT},
        {IDENTIFICATION_FOR_UPDATE_DOCUMENT},
        {NODE_DETAILS_FOR_UPDATE_DOCUMENT}
        """;

    internal const string NODE_CREATE_CTE = $"""
        WITH
        {TAGGING_VOCABULARY},
        {TENANTS_DOCUMENT},
        {TAGS_DOCUMENT_CREATE},
        {NODE_DETAILS_FOR_CREATE_DOCUMENT}
        """;


    internal const string SIMPLE_TEXT_NODE_UPDATE_CTE = $"""
        {NODE_UPDATE_CTE},
        {SIMPLE_TEXT_NODE_DOCUMENT}
        """;

    internal const string SIMPLE_TEXT_NODE_CREATE_CTE = $"""
        {NODE_CREATE_CTE}
        """;
    internal const string LOCATABLE_UPDATE_CTE = $"""
        {NODE_UPDATE_CTE},
        {NAMEABLE_DETAILS_DOCUMENT},
        {SUBDIVISIONS_DOCUMENT},
        {LOCATIONS_DOCUMENT},
        {LOCATABLE_DETAILS_DOCUMENT}
    """;
    internal const string CASE_UPDATE_CTE = $"""
        {LOCATABLE_UPDATE_CTE},
        {CASE_CASE_PARTY_DOCUMENT},
        {CASE_DETAILS_DOCUMENT}
        """;

    internal const string CASE_CREATE_CTE = $"""
        {NODE_CREATE_CTE},
        {CASE_TYPE_CASE_PARTY_TYPE_DOCUMENT}
        """;

    internal const string PARTY_UPDATE_CTE = $"""
        {LOCATABLE_UPDATE_CTE},
        {CASE_CASE_PARTY_DOCUMENT},
        {CASE_DETAILS_DOCUMENT}
        """;

    internal const string PARTY_CREATE_CTE = $"""
        {NODE_CREATE_CTE}
        """;

    internal const string INTER_ORGANIZATIONAL_RELATION_TYPES_DOCUMENT = """
        inter_organizational_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title,
                        'IsSymmetric',
                        t.is_symmetric
                    )
                    order by n.title
                ) "document"
            from inter_organizational_relation_type t
            join node n on n.id = t.id
        )
        """;
    internal const string PERSON_ORGANIZATION_RELATION_TYPES_DOCUMENT = """
        person_organization_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from person_organization_relation_type t
            join node n on n.id = t.id
        )
        """;

    internal const string PERSON_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT = """
        person_political_entity_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from person_political_entity_relation_type t
            join node n on n.id = t.id
        )
        """;

    internal const string ORGANIZATION_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT = """
        organization_political_entity_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from organization_political_entity_relation_type t
            join node n on n.id = t.id
        )
        """;

    internal const string FAMILY_SIZES_DOCUMENT = """
        family_sizes_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from family_size t
            join node n on n.id = t.id
        )
        """;

    internal const string CHILD_PLACEMENT_TYPES_DOCUMENT = """
        child_placement_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from child_placement_type t
            join node n on n.id = t.id
        )
        """;
    internal const string TYPES_OF_ABUSE_DOCUMENT = """
        types_of_abuse_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from type_of_abuse t
            join node n on n.id = t.id
        )
        """;

    internal const string TYPES_OF_ABUSER_DOCUMENT = """
        types_of_abuser_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from type_of_abuser t
            join node n on n.id = t.id
        )
        """;

    internal const string PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT = """
        party_political_entity_relations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'NodeIdentification',
                        jsonb_build_object(
                            'NodeId', 
                            node_id,
                            'UrlId', 
                            url_id
                        ),
                        'NodeDetailsForUpdate',
                        jsonb_build_object(
                           'NodeTypeName',
                            node_type_name,
                            'NodeTypeId',
                            node_type_id,
                            'PublisherId', 
                            publisher_id,
                            'OwnerId', 
                            owner_id,
                            'Title', 
                            title,
            		        'TagsForUpdate', 
                            null,
                            'TenantNodeDetailsForUpdate',
                            json_build_object(
                                'TenantNodesToUpdate',
                                null
                            ),
                            'Tenants',
                            null,
                            'Files',
                            null
                        ),
                        'Party',
                        jsonb_build_object(
                            'Id',
                            party_id,
                            'Name',
                            party_name
                        ),
                        'PoliticalEntity',
                        jsonb_build_object(
                            'Id',
                            political_entity_id,
                            'Name',
                            political_entity_name
                        ),
                        'PartyPoliticalEntityRelationType',
                        jsonb_build_object(
                            'Id',
                            party_political_entity_relation_type_id,
                            'Name',
                            party_political_entity_relation_type_name
                        ),
                        'RelationDetails',
                        jsonb_build_object(
                            'ProofDocument',
                            case
                                when document_id_proof is null then null
                                else jsonb_build_object(
                                    'Id',
                                    document_id_proof,
                                    'Name',
                                    document_title_proof
                                )
                            end,
                            'DateFrom',
                            date_from,
                            'DateTo',
                            date_to,
                            'Description',
                            description
                        )
                    )
                ) "document"
            from(
                select
                    node_id,
                    title,
                    publisher_id,
                    owner_id,
                    node_type_name,
                    node_type_id,
                    url_id,
                    party_id,
                    party_name,
                    political_entity_id,
                    political_entity_name,
                    party_political_entity_relation_type_id,
                    party_political_entity_relation_type_name,
                    document_id_proof,
                    document_title_proof,
                    date_from,
                    date_to,
                    description,
                    case 
                        when status_relation = 1 then true
                        else false
                    end has_been_published	
                from(
                    select
                        distinct
                        r.id node_id,
                        n.title title,
                        n.publisher_id,
                        n.owner_id,
                        nt.name node_type_name,
                        nt.id node_type_id,
                        tn2.url_id,
                        r.party_id,
                        n1.title party_name,
                        r.political_entity_id,
                        n2.title political_entity_name,
                        r.party_political_entity_relation_type_id,
                        n5.title party_political_entity_relation_type_name,
                        r.document_id_proof,
                        n4.title document_title_proof,
                        lower(r.date_range) date_from,
                        upper(r.date_range) date_to,
                        r.description,
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
                        end status_political_entity,
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
                        end status_relation	
                    from party_political_entity_relation r
                    join node n on n.id = r.id
                    join node_type nt on nt.id = n.node_type_id
                    join node n1 on n1.id = r.party_id
                    join node n2 on n2.id = r.political_entity_id
                    left join node n4 on n4.id = r.document_id_proof
                    join node n5 on n5.id = r.party_political_entity_relation_type_id
                    join tenant_node tn on tn.node_id = n1.id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
        		    join tenant_node tn3 on tn3.node_id = r.id and tn3.tenant_id = tn.tenant_id
                    where tn.tenant_id = @tenant_id
                    and tn.url_id = @url_id
                ) x
                where status_political_entity > -1 and status_relation > -1
            ) x
        )
        """;

    internal const string PERSON_ORGANIZATION_RELATIONS_DOCUMENT = """
        person_organization_relations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'NodeIdentification',
                        jsonb_build_object(
                            'NodeId', 
                            node_id,
                            'UrlId', 
                            url_id
                        ),
                        'NodeDetailsForUpdate',
                        jsonb_build_object(
                           'NodeTypeName',
                            node_type_name,
                            'NodeTypeId',
                            node_type_id,
                            'PublisherId', 
                            publisher_id,
                            'OwnerId', 
                            owner_id,
                            'Title', 
                            title,
            		        'TagsForUpdate', 
                            null,
                            'TenantNodeDetailsForUpdate',
                            json_build_object(
                                'TenantNodesToUpdate',
                                null
                            ),
                            'Tenants',
                            null,
                            'Files',
                            null
                        ),
                        'Person',
                        jsonb_build_object(
                            'Id',
                            person_id,
                            'Name',
                		    person_name
                        ),
                	    'Organization',
                        jsonb_build_object(
                            'Id',
                            organization_id,
                            'Name',
                            organization_name
                        ),
                	    'PersonOrganizationRelationType',
                        jsonb_build_object(
                            'Id',
                            person_organization_relation_type_id,
                            'Name',
                            person_organization_relation_type_name
                        ),
                        'RelationDetails',
                        jsonb_build_object(
                	        'ProofDocument',
                            case
                                when document_id_proof is null then null
                                else jsonb_build_object(
                                    'Id',
                                    document_id_proof,
                                    'Name',
                                    document_title_proof
                                )
                            end,
                	        'GeographicalEntity',
                            case 
                                when geographical_entity_id is null then  null
                                else jsonb_build_object(
                                    'Id',
                                    geographical_entity_id,
                                    'Name',
                                    geographical_entity_name
                                )
                            end,
                	        'DateFrom',
                	        date_from,
                	        'DateTo',
                	        date_to,
                	        'Description',
                	        description
                        )
                    )
                ) "document"
            from(
                select
                    node_id,
                    title,
                    publisher_id,
                    owner_id,
                    node_type_name,
                    node_type_id,
                    url_id,
                    person_id,
                    person_name,
                    organization_id,
                    organization_name,
                    person_organization_relation_type_id,
                    person_organization_relation_type_name,
                    document_id_proof,
                    document_title_proof,
                    geographical_entity_id,
                    geographical_entity_name,
                    date_from,
                    date_to,
                    description,
                    case 
                	    when status_relation = 1 then true
                	    else false
                    end has_been_published	
                from(
                    select
                        distinct
                        r.id node_id,
                        n.title title,
                        n.publisher_id,
                        n.owner_id,
                        nt.name node_type_name,
                        nt.id node_type_id,
                        tn2.url_id,
                        r.person_id,
                        n1.title person_name,
                        r.organization_id,
                        n2.title organization_name,
                        r.person_organization_relation_type_id,
                        n5.title person_organization_relation_type_name,
                        r.document_id_proof,
                        n4.title document_title_proof,
                        r.geographical_entity_id,
                        n3.title geographical_entity_name,
                        lower(r.date_range) date_from,
                        upper(r.date_range) date_to,
                        r.description,
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
                        end status_other_organization,
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
                        end status_relation	
                    from person_organization_relation r
                    join node n on n.id = r.id
                    join node_type nt on nt.id = n.node_type_id
                    join node n1 on n1.id = r.person_id
                    join node n2 on n2.id = r.organization_id
                    left join node n3 on n3.id = r.geographical_entity_id
                    left join node n4 on n4.id = r.document_id_proof
                    join node n5 on n5.id = r.person_organization_relation_type_id
                    join tenant_node tn on tn.node_id = n1.id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                    join tenant_node tn3 on tn3.node_id = r.id and tn3.tenant_id = tn.tenant_id
                    where tn.tenant_id = @tenant_id
        		    and {0}.url_id = @url_id
                ) x
                where status_other_organization > -1 and status_relation > -1
            ) x
        )
        """;

    internal const string PERSON_PERSONAL_RELATION_TYPES_DOCUMENT = """
        person_organization_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                    order by n.title
                ) "document"
            from person_organization_relation_type t
            join node n on n.id = t.id
        )
        """;

    internal const string INTER_PERSONAL_RELATION_TYPES_DOCUMENT = """
        inter_personal_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title,
                        'IsSymmetric',
                        t.is_symmetric
                    )
                    order by n.title
                ) "document"
            from inter_personal_relation_type t
            join node n on n.id = t.id
        )
        """;

    internal const string ORGANIZATION_TYPES_DOCUMENT = """
        organization_types_document as(
            select
            jsonb_agg(
                jsonb_build_object(
                    'Id',
                    ot.id,
                    'Name',
                    t.name,
                    'HasConcreteSubtype',
                    ot.has_concrete_subtype
                )
                order by t.name
            ) "document"
            from organization_type ot
            join term t on t.nameable_id = ot.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 12622
        )
        """;

    internal const string CASE_TYPE_CASE_PARTY_TYPE_DOCUMENT = """
        case_type_case_party_type_document as(
            select
            jsonb_agg(
                jsonb_build_object(
                    'CaseId',
                    null,
                    'CasePartyTypeId',
                    n.id,
                    'CasePartyTypeName',
                    n.title,
                    'OrganizationsText',
                    null,
                    'PersonsText',
                    null,
                    'Organizations',
                    null,
                    'Persons',
                    null
                )
            ) document
            from case_type_case_party_type ctpt 
            join case_party_type cpt on cpt.id = ctpt.case_party_type_id
            join node n on n.id = cpt.id
            where ctpt.case_type_id = @node_type_id
        )
        """;

    internal const string CASE_CASE_PARTY_DOCUMENT = """
        case_case_party_document as(
            select
            jsonb_agg(
        	    jsonb_build_object(
                    'Id',
                    case_party_id,
                    'CaseId',
                    c.id,
        		    'CasePartyTypeId',
        		    n.id,
        		    'CasePartyTypeName',
        		    n.title,
        		    'PersonsText',
        		    persons_text,
        		    'OrganizationsText',
        		    organizations_text,
        		    'Organizations',
        		    organizations,
        		    'Persons',
        		    persons
        	    )
            ) document
            from case_type_case_party_type ctpt 
            join case_party_type cpt on cpt.id = ctpt.case_party_type_id
            join node n on n.id = cpt.id
            join tenant_node tn on tn.tenant_id = @tenant_id and tn.url_id = @url_id
            join "case" c on c.id = tn.node_id
            join node n2 on n2.id = c.id
            left join (
        	    select
                cp.id case_party_id,
        	    ccp.case_id,
        	    ccp.case_party_type_id,
        	    cp.organizations organizations_text,
        	    cp.persons persons_text,
        		CASE 
        			WHEN COUNT(DISTINCT cpo.organization_id) = 0 THEN NULL
        			else jsonb_agg(
        				distinct
                        jsonb_build_object(
                            'Organization',
        				    jsonb_build_object(
        					    'Id',
        					    cpo.organization_id,
        					    'Name',
        					    cpo.organization_name
                            ),
                            'HasBeenDeleted',
                            false
                		)
        			) 
        		end organizations,
                CASE 
        			WHEN COUNT(DISTINCT cpp.person_id) = 0 THEN NULL
        			else jsonb_agg(
        				distinct
                        jsonb_build_object(
                            'Person',
        				    jsonb_build_object(
        					    'Id',
        					    cpp.person_id,
        					    'Name',
        					    cpp.person_name
        				    ),
                            'HasBeenDeleted',
                            false
                        )
        			) 
        		END persons
                from case_case_parties ccp
        	    join case_parties cp on cp.id = ccp.case_parties_id
        	    left join (
        		    select
        		    cpo.case_parties_id,
        		    n.id organization_id,
        		    n.title organization_name
        		    from
        		    case_parties_organization cpo 
        		    join organization o on o.id = cpo.organization_id
        		    join node n on n.id = o.id
        	    ) cpo on cpo.case_parties_id = cp.id
        	    left join (
        		    select
        		    cpp.case_parties_id,
        		    n.id person_id,
        		    n.title person_name
        		    from
        		    case_parties_person cpp
        		    join person p on p.id = cpp.person_id
        		    join node n on n.id = p.id
        	    ) cpp on cpp.case_parties_id = cp.id
        	    group by 
        	    ccp.case_id,
        	    ccp.case_party_type_id,
        	    cp.organizations,
        	    cp.persons,
                cp.id
            ) ccp on ccp.case_id = c.id and ccp.case_party_type_id = cpt.id
            where ctpt.case_type_id = n2.node_type_id
        )
        """;

    internal const string CASE_DETAILS_DOCUMENT = """
        case_details_document as(
            select
                jsonb_build_object(
                    'Date',
                    c.fuzzy_date,
                    'CasePartyTypesCaseParties',
                    (select document from case_case_party_document)
                ) document,
                id
            from "case" c
        )
        """;

    internal const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                jsonb_agg(jsonb_build_object(
        			'LocationId', "location_id",
                    'LocatableId', locatable_id,
        			'Street', street,
        			'Additional', additional,
        			'City', city,
        			'PostalCode', postal_code,
        			'SubdivisionId', subdivision_id,
                    'SubdivisionName', subdivision_name,
        			'CountryId', country_id,
                    'CountryName', country_name,
                    'Latitude', latitude,
                    'Longitude', longitude,
                    'Subdivisions', subdivisions
        		)) document,
                locatable_id id
            from(
                select 
                ll.location_id,
                ll.locatable_id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                l.subdivision_id,
                s.name subdivision_name,
                l.country_id,
                nc.title country_name,
                l.latitude,
                l.longitude,
                (select document from subdivisions_document where country_id = l.country_id) subdivisions
                from "location" l
                join location_locatable ll on ll.location_id = l.id
                join node nc on nc.id = l.country_id
                left join subdivision s on s.id = l.subdivision_id
            )x
            group by locatable_id
        )
        """;

    internal const string COUNTRIES_DOCUMENT = $"""
        countries_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        c.id,
        		        'Name',
        		        t.name
        	        )
                ) document
            from country c
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
        )
        """;

    internal const string LOCATABLE_DETAILS_DOCUMENT = """
        locatable_details_document as(
            select
                jsonb_build_object(
                    'LocationsToUpdate',
                    (select document from locations_document ld where ld.id = l.id)
                ) document,
                l.id
            from locatable l
        )
        """;
    const string TAGS_DOCUMENT_CREATE = """
        tags_for_create_document as (
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'TagNodeType',
        		    jsonb_build_object(
        			    'NodeTypeIds',
        			    node_type_ids,
        			    'TagLabelName',
        			    tag_label_name
        		    ),
        		    'Entries',
        		    null
        	    )
            ) "document"
            from(
        	    select
        	    jsonb_agg(
        		    nt.id
        	    ) node_type_ids,
        	    nt.tag_label_name
        	    from nameable_type nt
        	    join node_type nt3 on nt3.id = nt.id
        	    group by nt.tag_label_name
            ) x        
        )
        """;

    const string TAGS_DOCUMENT_UPDATE = """
        tags_for_update_document as (
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'TagNodeType',
        		    jsonb_build_object(
        			    'NodeTypeIds',
        			    node_type_ids,
        			    'TagLabelName',
        			    tag_label_name
        		    ),
        		    'EntriesToUpdate',
        		    tags
        	    )
            ) "document"
            from(
        	    select
        	    jsonb_agg(
        		    nt.id
        	    ) node_type_ids,
        	    nt.tag_label_name,
        	    (
        		    select 
        		    jsonb_agg(
        			    jsonb_build_object(
        				    'NodeId',
        				    nt2.node_id,
        				    'TermId',
        				    t.id,
        				    'Name',
        				    t.name,
        				    'NodeTypeId',
        				    n.node_type_id
        			    )
        		    ) 
        		    from node_term nt2
        		    join tenant_node tn on tn.node_id = nt2.node_id
        		    join term t on t.id = nt2.term_id
        		    join node n on n.id = t.nameable_id
        		    where tn.tenant_id = @tenant_id
        		    AND tn.url_id = @url_id
        		    AND n.node_type_id = ANY(ARRAY_AGG(nt.id))
        	    ) tags 
        	    from nameable_type nt
        	    join node_type nt3 on nt3.id = nt.id
        	    group by nt.tag_label_name
            ) x        
        )
        """;

    internal const string DOCUMENT_TYPES = """
        document_types as (
            select
        	    n.id,
        	    n.title,
                case when n.title = 'News paper article' then true else false end is_selected
            from document_type dt
            join term t on t.nameable_id = dt.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            join node n on n.id = dt.id 
            where tn.url_id = 42416 and tn.tenant_id = 1
        )
        """;

    internal const string DOCUMENT_TYPES_DOCUMENT_CREATE = """
        document_types_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        id,
        		        'Name',
        		        title,
                        'IsSelected',
                        is_selected
        	        )
                ) document
            from document_types
        )
        """;

    const string DOCUMENT_TYPES_DOCUMENT_EDIT = """
        document_types_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        n.id,
        		        'Name',
        		        n.title,
                        'IsSelected',
                        d.document_type_id is not null
        	        )
                ) document
            from document_type dt
            join term t on t.nameable_id = dt.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            join node n on n.id = dt.id 
            left join (
                select 
                d.document_type_id
                from document d
                join tenant_node tn2 on tn2.node_id = d.id
                where tn2.url_id = @url_id and tn2.tenant_id = @tenant_id
            ) d on d.document_type_id = dt.id
            where tn.url_id = 42416 and tn.tenant_id = 1
        )
        """;

    const string ATTACHMENTS_DOCUMENT = """
        attachments_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        f.id,
                        'Name',
                        f.name,
        		        'Path',
                        f.path,
                        'Size',
                        f.size,
                        'MimeType',
                        f.mime_type,
                        'NodeId',
                        nf.node_id,
                        'HasBeenStored',
                        true
        	        )
                ) document
            from file f
            join node_file nf on nf.file_id = f.id
            join tenant_node tn on tn.node_id = nf.node_id
            where tn.url_id = @url_id and tn.tenant_id = @tenant_id
        )
        """;

    const string SUBDIVISIONS_DOCUMENT = """
        subdivisions_document as(
            select
            c.country_id,
            jsonb_agg(
        	    jsonb_build_object(
        		    'Id',
        		    c.id,
        		    'Name',
        		    t.name
        	    )
            ) document
            from subdivision c
            join bottom_level_subdivision b on b.id = c.id
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
            group by c.country_id
        )
        """;

    const string TENANT_NODES_DOCUMENT = """
        tenant_nodes_document as(
        select
            jsonb_agg(
                jsonb_build_object(
                    'Id',
                    id,
                    'TenantId',
                    tenant_id,
                    'UrlId',
                    url_id,
                    'UrlPath',
                    url_path,
                    'NodeId',
                    node_id,
                    'SubgroupId',
                    subgroup_id,
                    'PublicationStatusId',
                    publication_status_id,
                    'HasBeenStored',
                    true,
                    'CanBeUnchecked',
                    false
                )
            ) document
        from(
            select
        		tn2.id,
        		tn2.tenant_id,
        		tn2.url_id,
        		tn2.url_path,
        		tn2.node_id,
        		tn2.subgroup_id,
        		tn2.publication_status_id,
        		(
                    select
                        case 
                            when max(status) = 1 then true
                            else false
                        end
                    from(
                        select
                            distinct
                            1 status
                        from user_group_user_role_user uguru
                        join user_group ug on ug.id = uguru.user_group_id
                        where uguru.user_id = @user_id
                        and user_group_id = tn2.tenant_id
                        and ug.administrator_role_id = uguru.user_role_id
                        union
                        select
                            distinct
                            1 status
                        from user_group_user_role_user uguru
                        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                        join create_node_action cna on cna.id = arp.action_id
                        where uguru.user_id = @user_id
                        and user_group_id = tn2.tenant_id
                        and cna.node_type_id = @node_type_id
                    ) x
                ) allow_access
                from tenant_node tn
                join tenant_node tn2 on tn2.node_id = tn.node_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
        ) x 
        where allow_access = true
        )
        """;

    const string TENANTS_DOCUMENT = """
        tenants_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        id,
        		        'DomainName',
        		        domain_name,
        		        'AllowAccess',
        		        allow_access,
                        'PublicationStatusIdDefault',
                        tenant_publication_status_id_default,
        		        'Subgroups',
        		        case 
        			        when subgroups = '[null]' then null
        			        else subgroups
        		        end
        	        )
                ) document
            from(
                select
                    id,
                    domain_name,
                    allow_access,
                    tenant_publication_status_id_default,
                    jsonb_agg(
        	            case when subgroup_id is null then null
        	            else 
        	            jsonb_build_object(
        		            'Id',
        		            subgroup_id,
        		            'Name',
        		            subgroup_name,
                            'PublicationStatusIdDefault',
                            subgroup_publication_status_id_default
        	            )
        	            end
                    ) subgroups
                from(
        		    select
        		        distinct
                        t.id,
                        t.domain_name,
                        pug.publication_status_id_default tenant_publication_status_id_default,
        		        s.name subgroup_name,
                        s.publication_status_id_default subgroup_publication_status_id_default,
        		        s.id subgroup_id,
                        (
                	        select
                		        case 
                			        when max(status) = 1 then true
                			        else false
                		        end
                	        from(
                		        select
                			        distinct
                			        1 status
                		        from user_group_user_role_user uguru
                		        join user_group ug on ug.id = uguru.user_group_id
                		        where uguru.user_id = @user_id
                		        and user_group_id = t.id
                		        and ug.administrator_role_id = uguru.user_role_id
                		        union
                		        select
                			        distinct
                			        1 status
                		        from user_group_user_role_user uguru
                		        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                		        join create_node_action cna on cna.id = arp.action_id
                		        where uguru.user_id = @user_id
                		        and user_group_id = t.id
                		        and cna.node_type_id = @node_type_id
                	        ) x
                        ) allow_access
                    from tenant t
                    join publishing_user_group pug on pug.id = t.id
                    left join(
                        select
                	        name,
                	        id,
                	        tenant_id,
                            publication_status_id_default
                        from(
                	        select
                		        distinct
                		        ug.name,
                		        s.id,
                                pug.publication_status_id_default,
                		        s.tenant_id,
                		        (
                			        select
                				        case 
                					        when max(status) = 1 then true
                					        else false
                				        end
                			        from(
                				        select
                					        distinct
                					        1 status
                				        from user_group_user_role_user uguru
                				        join user_group ug on ug.id = uguru.user_group_id
                				        where uguru.user_id = @user_id
                				        and user_group_id = s.id
                				        and ug.administrator_role_id = uguru.user_role_id
                				        union
                				        select
                					        distinct
                					        1 status
                				        from user_group_user_role_user uguru
                				        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                				        join create_node_action cna on cna.id = arp.action_id
                				        where uguru.user_id = @user_id
                				        and user_group_id = s.id
                				        and cna.node_type_id = @node_type_id
                			        ) x
                		        ) allow_access
                	        from subgroup s 
                            join publishing_user_group pug on pug.id = s.id
                	        join user_group ug on ug.id = s.id
                        ) x
                        where allow_access =  true
                    ) s on s.tenant_id = t.id
                    join user_group ug on ug.id = t.id
                    join user_group_user_role_user uguru on uguru.user_group_id = ug.id
                    where uguru.user_id = @user_id
                ) x
                group by
                id, domain_name, allow_access,tenant_publication_status_id_default
        	) x
        )
        """;


    const string TAGGING_VOCABULARY = """
        tagging_vocabulary as (
            select
            max(vocabulary_id_tagging) id
            from system_group
        )
        """;
    const string IDENTIFICATION_FOR_UPDATE_DOCUMENT = """
        identification_for_update_document as (
            select 
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'UrlId', 
                    @url_id
                ) document,
                n.id
            from node n
        )
        """;

    const string NODE_DETAILS_FOR_CREATE_DOCUMENT = """
        node_details_for_create_document as (
            select 
                jsonb_build_object(
                   'NodeTypeName',
                    nt.name,
                    'NodeTypeId',
                    nt.id,
                    'PublisherId', 
                    @user_id,
                    'OwnerId', 
                    @tenant_id,
                    'Title', 
                    '',
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null,
                    'TagsToCreate',
                    (select document from tags_for_create_document),
                    'TenantNodeDetailsForCreate',
                    jsonb_build_object(
                        'TenantNodesToAdd',
                        null
                    )
                ) document
            from node_type nt 
            where nt.id = @node_type_id
        )
        """;

    const string NODE_DETAILS_FOR_UPDATE_DOCUMENT = """
        node_details_for_update_document as (
            select 
                jsonb_build_object(
                   'NodeTypeName',
                    nt.name,
                    'NodeTypeId',
                    nt.id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title', 
                    n.title,
            		'TagsForUpdate', 
                    (select document from tags_for_update_document),
                    'TenantNodeDetailsForUpdate',
                    json_build_object(
                        'TenantNodesToUpdate',
                        (select document from tenant_nodes_document)
                    ),
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    (select document from attachments_document)
                ) document,
                n.id
            from node n
            join node_type nt on nt.id = n.node_type_id
        )
        """;

    const string NAMEABLE_DETAILS_DOCUMENT = """
        nameable_details_document as(
            select
                jsonb_build_object(
                    'Description', 
                    description,
            		'VocabularyIdTagging',
                    (select id from tagging_vocabulary)
                ) document,
                id
            from nameable
        )
        """;

    const string SIMPLE_TEXT_NODE_DOCUMENT = """
        select
            json_build_object(
                'Text', 
                stn.text
            ) document,
            id
        from simple_text_node
        """;

}
