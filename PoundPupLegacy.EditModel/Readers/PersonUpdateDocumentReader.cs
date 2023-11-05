namespace PoundPupLegacy.EditModel.Readers;

internal sealed class PersonUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Person.ToUpdate>
{
    public override string Sql => string.Format(SQL, "tn");

    protected override int NodeTypeId => Constants.PERSON;

    const string SQL = $"""
            {SharedSql.PARTY_UPDATE_CTE},
            {SharedSql.COUNTRIES_DOCUMENT},
            {SharedSql.INTER_PERSONAL_RELATION_TYPES_DOCUMENT},
            {SharedSql.PERSON_PERSONAL_RELATION_TYPES_DOCUMENT},
            {SharedSql.PERSON_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT},
            {INTER_PERSONAL_RELATIONS_FROM_DOCUMENT},
            {INTER_PERSONAL_RELATIONS_TO_DOCUMENT},
            {SharedSql.PERSON_ORGANIZATION_RELATIONS_DOCUMENT},
            {SharedSql.PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT}
                select
                jsonb_build_object(
                    'NodeIdentification',
                    (select document from identification_for_update_document where id = n.id),
                    'NodeDetailsForUpdate',
                    (select document from node_details_for_update_document where id = n.id),
                    'NameableDetails',
                    (select document from nameable_details_document where id = n.id),
                    'LocatableDetailsForUpdate',
                    (select document from locatable_details_document where id = n.id),
                    'PersonDetailsForUpdate',
                    jsonb_build_object(
                        'Name',
                        n.title,
                        'Countries',
                        (select document from countries_document),
                        'InterPersonalRelationTypes',
                        (select document from inter_personal_relation_types_document),
                        'PersonOrganizationRelationTypes',
                        (select document from person_organization_relation_types_document),
                        'PersonPoliticalEntityRelationTypes',
                        (select document from person_political_entity_relation_types_document),
                        'InterPersonalRelationsFromToUpdate',
                        (select document from inter_personal_relations_from_document),
                        'InterPersonalRelationsToToUpdate',
                        (select document from inter_personal_relations_to_document),
                        'PersonOrganizationRelationsToUpdate',
                        (select document from person_organization_relations_document),
                        'PartyPoliticalEntityRelationsToUpdate',
                        (select document from party_political_entity_relations_document)
                    )
                ) document
            from node n
            join node_type nt on nt.id = n.node_type_id
            join person p on p.id = n.id
            join nameable nm on nm.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id and n.node_type_id = @node_type_id
        """;
    const string INTER_PERSONAL_RELATIONS_FROM_DOCUMENT = """
        inter_personal_relations_from_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
                        'NodeIdentification',
                        jsonb_build_object(
                            'NodeId', 
                            node_id
                        ),
                        'NodeDetailsForUpdate',
                        jsonb_build_object(
                            'Id',
                            node_id,
                            'NodeTypeName',
                            node_type_name,
                            'NodeTypeId',
                            node_type_id,
                            'PublisherId', 
                            publisher_id,
                            'PublisherName', 
                            publisher_name,
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
                        'PersonFrom',
                        jsonb_build_object(
                            'Id',
                	        person_id_from,
                            'Name',
        		            person_name_from
                        ),
        		        'PersonTo',
                        jsonb_build_object(
                            'Id',
                            person_id_to,
                            'Name',
                            person_name_to
                        ),
        		        'InterPersonalRelationType',
                        jsonb_build_object(
                            'Id',
                            inter_personal_relation_type_id,
                            'Name',
                            inter_personal_relation_type_name
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
                    publisher_name,
                    owner_id,
                    node_type_name,
                    node_type_id,
                    person_id_from,
                    person_name_from,
                    person_id_to,
                    person_name_to,
                    inter_personal_relation_type_id,
                    inter_personal_relation_type_name,
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
                        p.name publisher_name,
                        n.owner_id,
                        nt.name node_type_name,
                        nt.id node_type_id,
                        r.person_id_from,
                        n1.title person_name_from,
                        r.person_id_to,
                        n2.title person_name_to,
                        r.inter_personal_relation_type_id,
                        n5.title inter_personal_relation_type_name,
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
                        end status_other_person,
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
                    from inter_personal_relation r
                    join node n on n.id = r.id
                    join publisher p on p.id = n.publisher_id
                    join node_type nt on nt.id = n.node_type_id
                    join node n1 on n1.id = r.person_id_from
                    join node n2 on n2.id = r.person_id_to
                    left join node n4 on n4.id = r.document_id_proof
                    join node n5 on n5.id = r.inter_personal_relation_type_id
                    join tenant_node tn on tn.node_id = n1.id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                    join tenant_node tn3 on tn3.node_id = r.id and tn3.tenant_id = tn.tenant_id
                    where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        	    ) x
                where status_other_person > -1 and status_relation > -1
        	) x
        )
        """;

    const string INTER_PERSONAL_RELATIONS_TO_DOCUMENT = """
        inter_personal_relations_to_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
                        'NodeIdentification',
                        jsonb_build_object(
                            'NodeId', 
                            node_id
                        ),
                        'NodeDetailsForUpdate',
                        jsonb_build_object(
                            'Id',
                            node_id,
                           'NodeTypeName',
                            node_type_name,
                            'NodeTypeId',
                            node_type_id,
                            'PublisherId', 
                            publisher_id,
                            'PublisherName',
                            publisher_name,
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
                        'PersonFrom',
                        jsonb_build_object(
                            'Id',
                	        person_id_from,
                            'Name',
        		            person_name_from
                        ),
        		        'PersonTo',
                        jsonb_build_object(
                            'Id',
                            person_id_to,
                            'Name',
                            person_name_to
                        ),
        		        'InterPersonalRelationType',
                        jsonb_build_object(
                            'Id',
                            inter_personal_relation_type_id,
                            'Name',
                            inter_personal_relation_type_name
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
                    publisher_name,
                    owner_id,
                    node_type_name,
                    node_type_id,
                    person_id_from,
                    person_name_from,
                    person_id_to,
                    person_name_to,
                    inter_personal_relation_type_id,
                    inter_personal_relation_type_name,
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
                        p.name publisher_name,
                        n.owner_id,
                        nt.name node_type_name,
                        nt.id node_type_id,
                        r.person_id_from,
                        n1.title person_name_from,
                        r.person_id_to,
                        n2.title person_name_to,
                        r.inter_personal_relation_type_id,
                        n5.title inter_personal_relation_type_name,
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
                        end status_other_person,
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
                    from inter_personal_relation r
                    join node n on n.id = r.id
                    join publisher p on p.id = n.publisher_id
                    join node_type nt on nt.id = n.node_type_id
                    join node n1 on n1.id = r.person_id_from
                    join node n2 on n2.id = r.person_id_to
                    left join node n4 on n4.id = r.document_id_proof
                    join node n5 on n5.id = r.inter_personal_relation_type_id
                    join tenant_node tn on tn.node_id = n2.id
                    join tenant_node tn2 on tn2.node_id = n1.id and tn2.tenant_id = tn.tenant_id
                    join tenant_node tn3 on tn3.node_id = r.id and tn3.tenant_id = tn.tenant_id
                    where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        	    ) x
                where status_other_person > -1 and status_relation > -1
        	) x
        )
        """;

}
