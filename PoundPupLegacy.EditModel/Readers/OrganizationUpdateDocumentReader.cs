namespace PoundPupLegacy.EditModel.Readers;

internal sealed class OrganizationUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Organization>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ORGANIZATION;

        const string SQL = $"""
            {CTE_EDIT},
            {INTER_ORGANIZATIONAL_RELATION_TYPES_DOCUMENT},
            {PERSON_ORGANIZATION_RELATION_TYPES_DOCUMENT},
            {PARTY_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT},
            {INTER_ORGANIZATIONAL_RELATIONS_DOCUMENT},
            {PERSON_ORGANIZATION_RELATIONS_DOCUMENT},
            {PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT}
            select
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'UrlId', 
                    tn.url_id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title' , 
                    n.title,
                    'NodeTypeName',
                    nt.name,
                    'Description', 
                    nm.description,
                    'WebSiteUrl',
                    o.website_url,
                    'EmailAddress',
                    o.email_address,
                    'EstablishmentDateFrom',
                    lower(o.established),
                    'EstablishmentDateTo',
                    upper(o.established),
                    'TerminationDateFrom',
                    lower(o.terminated),
                    'TerminationDateTo',
                    upper(o.terminated),
                    'OrganizationOrganizationTypes',
                    (select document from organization_organization_types_document),
                    'OrganizationTypes',
                    (select document from organization_types_document),
            		'Tags', 
                    (select document from tags_document),
                    'TenantNodes',
                    (select document from tenant_nodes_document),
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    (select document from attachments_document),
                    'Locations',
                    (select document from locations_document),
                    'Countries',
                    (select document from countries_document),
                    'Countries',
                    (select document from countries_document),
                    'InterOrganizationalRelations',
                    (select document from inter_organizational_relations_document),
                    'InterOrganizationalRelationTypes',
                    (select document from inter_organizational_relation_types_document),
                    'PersonOrganizationRelationTypes',
                    (select document from person_organization_relation_types_document),
                    'PersonOrganizationRelations',
                    (select document from person_organization_relations_document),
                    'PartyPoliticalEntityRelationTypes',
                    (select document from party_political_entity_relation_types_document),
                    'PartyPoliticalEntityRelations',
                    (select document from party_political_entity_relations_document)
                ) document
            from node n
            join node_type nt on nt.id = n.node_type_id
            join organization o on o.id = n.id
            join nameable nm on nm.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;
    const string INTER_ORGANIZATIONAL_RELATIONS_DOCUMENT = """
        inter_organizational_relations_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'NodeId',
        		        node_id,
                        'Title',
                        title,
                        'PublisherId',
                        publisher_id,
                        'OwnerId',
                        owner_id,
                        'NodeTypeName',
                        node_type_name,
                        'UrlId',
                        url_id,
                        'OrganizationFrom',
                        jsonb_build_object(
                            'Id',
                	        organization_id_from,
                            'Name',
        		            organization_name_from
                        ),
        		        'OrganizationTo',
                        jsonb_build_object(
                            'Id',
                            organization_id_to,
                            'Name',
                            organization_name_to
                        ),
        		        'InterOrganizationalRelationType',
                        jsonb_build_object(
                            'Id',
                            inter_organizational_relation_type_id,
                            'Name',
                            inter_organizational_relation_type_name
                        ),
        		        'DocumentProof',
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
        		        'MoneyInvolved',
        		        money_involved,
        		        'NumberOfChildrenInvolved',
        		        number_of_children_involved,
        		        'DateFrom',
        		        date_from,
        		        'DateTo',
        		        date_to,
        		        'Description',
        		        description,
                        'Tags',
                        null,
                        'Files',
                        null
        	        )
                ) "document"
            from(
                select
                    node_id,
                    title,
                    publisher_id,
                    owner_id,
                    node_type_name,
                    url_id,
                    organization_id_from,
                    organization_name_from,
                    organization_id_to,
                    organization_name_to,
                    inter_organizational_relation_type_id,
                    inter_organizational_relation_type_name,
                    document_id_proof,
                    document_title_proof,
                    geographical_entity_id,
                    geographical_entity_name,
                    money_involved,
                    number_of_children_involved,
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
                        tn2.url_id,
                        r.organization_id_from,
                        n1.title organization_name_from,
                        r.organization_id_to,
                        n2.title organization_name_to,
                        r.inter_organizational_relation_type_id,
                        n5.title inter_organizational_relation_type_name,
                        r.document_id_proof,
                        n4.title document_title_proof,
                        r.geographical_entity_id,
                        n3.title geographical_entity_name,
                        r.money_involved,
                        r.number_of_children_involved,
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
                    from inter_organizational_relation r
                    join node n on n.id = r.id
                    join node_type nt on nt.id = n.node_type_id
                    join node n1 on n1.id = r.organization_id_from
                    join node n2 on n2.id = r.organization_id_to
                    left join node n3 on n3.id = r.geographical_entity_id
                    left join node n4 on n4.id = r.document_id_proof
                    join node n5 on n5.id = r.inter_organizational_relation_type_id
                    join tenant_node tn on tn.node_id = n1.id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                    join tenant_node tn3 on tn3.node_id = r.id and tn3.tenant_id = tn.tenant_id
                    where tn.tenant_id = @tenant_id and tn.url_id = @url_id
                    union
                    select
                        distinct
                        r.id node_id,
                        n.title title,
                        n.publisher_id,
                        n.owner_id,
                        nt.name node_type_name,
                        tn2.url_id,
                        r.organization_id_from,
                        n1.title organization_from_name,
                        r.organization_id_to,
                        n2.title organization_to_name,
                        r.inter_organizational_relation_type_id,
                        n5.title inter_organizational_relation_type_name,
                        r.document_id_proof,
                        n4.title document_title_proof,
                        r.geographical_entity_id,
                        n3.title geographical_entity_name,
                        r.money_involved,
                        r.number_of_children_involved,
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
                    from inter_organizational_relation r
                    join node n on n.id = r.id
                    join node_type nt on nt.id = n.node_type_id
                    join node n1 on n1.id = r.organization_id_from
                    join node n2 on n2.id = r.organization_id_to
                    left join node n3 on n3.id = r.geographical_entity_id
                    left join node n4 on n4.id = r.document_id_proof
                    join node n5 on n5.id = r.inter_organizational_relation_type_id
                    join tenant_node tn on tn.node_id = n2.id
                    join tenant_node tn2 on tn2.node_id = n1.id and tn2.tenant_id = tn.tenant_id
                    join tenant_node tn3 on tn3.node_id = r.id and tn3.tenant_id = tn.tenant_id
                    where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        	    ) x
                where status_other_organization > -1 and status_relation > -1
        	) x
        )
        """;

    const string INTER_ORGANIZATIONAL_RELATION_TYPES_DOCUMENT = """
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
                ) "document"
            from inter_organizational_relation_type t
            join node n on n.id = t.id
        )
        """;
    const string PERSON_ORGANIZATION_RELATION_TYPES_DOCUMENT = """
        person_organization_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                ) "document"
            from person_organization_relation_type t
            join node n on n.id = t.id
        )
        """;

    const string PARTY_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT = """
        party_political_entity_relation_types_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id', 
                        n.id,
                        'Name', 
                        n.title
                    )
                ) "document"
            from party_political_entity_relation_type t
            join node n on n.id = t.id
        )
        """;

    const string PERSON_ORGANIZATION_RELATIONS_DOCUMENT = """
        person_organization_relations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                	    'NodeId',
                	    node_id,
                        'Title',
                        title,
                        'PublisherId',
                        publisher_id,
                        'OwnerId',
                        owner_id,
                        'NodeTypeName',
                        node_type_name,
                        'UrlId',
                        url_id,
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
                	    'DocumentProof',
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
                	    description,
                        'Tags',
                        null,
                        'Files',
                        null
                    )
                ) "document"
            from(
                select
                    node_id,
                    title,
                    publisher_id,
                    owner_id,
                    node_type_name,
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
        		    and tn2.url_id = @url_id
                ) x
                where status_other_organization > -1 and status_relation > -1
            ) x
        )
        """;
    const string PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT = """
        party_political_entity_relations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'NodeId',
                        node_id,
                        'Title',
                        title,
                        'PublisherId',
                        publisher_id,
                        'OwnerId',
                        owner_id,
                        'NodeTypeName',
                        node_type_name,
                        'UrlId',
                        url_id,
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
                        'DocumentProof',
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
                        description,
                        'Tags',
                        null,
                        'Files',
                        null
                    )
                ) "document"
            from(
                select
                    node_id,
                    title,
                    publisher_id,
                    owner_id,
                    node_type_name,
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
}
