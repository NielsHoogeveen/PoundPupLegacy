namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = UnitedStatesMeetingChamberDocumentReaderRequest;

public sealed record UnitedStatesMeetingChamberDocumentReaderRequest : IRequest
{
    public required int Number { get; init; }
    public required int Type { get; init; }

    public required int TenantId { get; init; }
}
internal sealed class UnitedStatesMeetingChamberDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, CongressionalMeetingChamber>
{
    private static readonly NonNullableIntegerDatabaseParameter MeetingNumberParameter = new() { Name = "meeting_number" };
    private static readonly NonNullableIntegerDatabaseParameter ChamberTypeParameter = new() { Name = "chamber_type" };
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly FieldValueReader<CongressionalMeetingChamber> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
        	jsonb_build_object(
        		'MeetingName',	
        		meeting_name,
        		'DateFrom',
        		date_from,
        		'DateTo',
        		date_to,
        		'States',
        		jsonb_agg(
        			jsonb_build_object(
        				'State',
        				jsonb_build_object(
        					'Title',
        					political_entity_name,
        					'Path',
        					political_entity_path
        				),
        				'Members',
        				members
        			)
        		) 
        	) document
        from(
        	select
        		meeting_name,
        		date_from,
        		date_to,
        		political_entity_path, 
        		political_entity_name,
        		jsonb_agg(
        			jsonb_build_object(
        				'Title',
        				"person_name",
        				'Path',
        				person_path,
                        'FilePathPortrait',
                        file_path_portrait,
        				'Bills',
        				bills,
        				'Letters',
        				documents,
        				'Parties',
        				parties,
        				'Terms',
        				terms
        			)
        		) members
        	from(
        		select
        		    case 
                        when @chamber_type = 12660 then 'United States House of Representatives - ' || n4.title
                        when @chamber_type = 12662 then 'United States Senate - ' || n4.title
        			end meeting_name,
        			lower(m.date_range) date_from, 
        			upper(m.date_range) date_to,
        			'/' || nt3.viewer_path || '/' || tn3.node_id political_entity_path,
        			s.name political_entity_name,
        			'/' || nt.viewer_path || '/' || tn.node_id person_path,
        			n.title person_name,
                    '/attachment/' || f.id file_path_portrait,
        			(
        				select
        					jsonb_agg(
        						jsonb_build_object(
        							'Title',
        							"name",
        							'Path',
        							path
        						) 
        					) document

        				from(
        					select
        						n1.title "name",
        						'/' || nt1.viewer_path || '/' || tn1.node_id path
        						from document d
        						join node n1 on n1.id = d.id
                                join node_type nt1 on nt1.id = n1.node_type_id
        						join tenant_node tn1 on tn1.node_id = n1.id and tn1.tenant_id = @tenant_id
        						join document_type dt on dt.id = d.document_type_id
        						join node n2 on n2.id = dt.id
        						join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
        						join node_term ntm on ntm.node_id = d.id
                                join term t on t.id = ntm.term_id
        						join tenant_node tn3 on tn3.node_id = t.nameable_id and tn3.tenant_id = @tenant_id
        						where tn2.node_id = 101489 and tn3.node_id = tn.node_id
        						order by n2.title
        				) x
        			) documents,
        			(
        				select
        				jsonb_agg(
        					jsonb_build_object(
        						'Title',
        						"name",
        						'Path',
        						path
        					)
        				) document
        				from(
        				select
        				n2.title "name",
        				'/' || nt2.viewer_path || '/' || tn2.node_id path
        				from person p
        				join professional_role pr on pr.person_id = p.id
        				join senator_senate_bill_action sba on sba.senator_id = pr.id
        				join senate_bill sb on sb.id = sba.senate_bill_id
        				join node n2 on n2.id = sb.id
                        join node_type nt2 on nt2.id = n2.node_type_id
        				join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
        				join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = @tenant_id
        				where tn1.node_id = tn.node_id
        				union
        				select
        				n2.title "name",
        				'/' || nt2.viewer_path || '/' || tn2.node_id path
        				from person p
        				join professional_role pr on pr.person_id = p.id
        				join representative_house_bill_action sba on sba.representative_id = pr.id
        				join house_bill sb on sb.id = sba.house_bill_id
        				join node n2 on n2.id = sb.id
                        join node_type nt2 on nt2.id = n2.node_type_id
        				join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        				join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = 1
        				where tn1.node_id = tn.node_id
        					) x
        			) bills,
        			(
        				select
        				jsonb_agg(
        					jsonb_build_object(
        						'Title',
        						"name",
        						'Path',
        						path,
        						'DateFrom',
        						date_from,
        						'DateTo',
        						date_to
        					)
        				) parties
        				from(
        					select
        					n2.title "name",
        					'/' || nt2.viewer_path || '/' || tn2.node_id path,
        					lower(por.date_range) date_from,
        					upper(por.date_range) date_to
        					from person p
        					join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = 1
        					join person_organization_relation por on por.person_id = p.id
                            JOIN united_states_political_party pp on pp.id = por.organization_id
        					join node n2 on n2.id = por.organization_id
                            join node_type nt2 on nt2.id = n2.node_type_id
        					join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        					where tn1.node_id = tn.node_id
        				) x			
        			) parties,
        			(
        				select
        					jsonb_agg(
        						jsonb_build_object(
        							'MemberType',
        							member_type,
        							'State',
        							jsonb_build_object(
        								'Title',
        								state_name,
        								'Path',
        								state_path
        							),
        							'DateFrom',
        							date_from,
        							'DateTo',
        							date_to
        						)
        					) terms
        					from(
        						select
        						n2.title member_type,
        						s.name state_name,
        						'/' || nt3.viewer_path || '/' || tn3.node_id state_path,
        						lower(pper.date_range) date_from,
        						upper(pper.date_range) date_to
        						from person p
        						join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = @tenant_id 
        						join party_political_entity_relation pper on pper.party_id = p.id
        						join node n2 on n2.id = pper.party_political_entity_relation_type_id
        						join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
        						join subdivision s on s.id = pper.political_entity_id
                                join node n3 on n3.id = s.id
                                join node_type nt3 on nt3.id = n3.node_type_id
        						join tenant_node tn3 on tn3.node_id = s.id and tn3.tenant_id = @tenant_id
        						where tn2.node_id in (101366, 101368)
        						and tn1.node_id = tn.node_id
        						order by lower(pper.date_range)
        					) x
        			) terms
        		from person p
                left join file f on f.id = p.file_id_portrait
        		join node n on n.id = p.id
                join node_type nt on nt.id = n.node_type_id
        		join party_political_entity_relation pper on pper.party_id = p.id
        		join node n1 on n1.id = pper.id
        		join node n2 on n2.id = pper.party_political_entity_relation_type_id
        		join tenant_node tn on tn.node_id = n.id AND tn.tenant_id = @tenant_id
        		join iso_coded_subdivision ics on ics.id = pper.political_entity_id
        		join subdivision s on s.id = ics.id
                join node n3 on n3.id = s.id
                join node_type nt3 on nt3.id = n3.node_type_id
        		join tenant_node tn2 on tn2.node_id = n2.id AND tn2.tenant_id = @tenant_id
        		join tenant_node tn3 on tn3.node_id = s.id AND tn3.tenant_id = @tenant_id
        		join united_states_congressional_meeting m on m.date_range && pper.date_range
        		join node n4 on n4.id = m.id
        		join tenant_node tn4 on tn4.node_id = n4.id AND tn4.tenant_id = @tenant_id
        		where 
        		tn2.node_id = @chamber_type and m.number = @meeting_number
        	) x
        	group by meeting_name, date_from, date_to,political_entity_path, political_entity_name
        ) x
        group by meeting_name, date_from, date_to
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(MeetingNumberParameter, request.Number),
            ParameterValue.Create(ChamberTypeParameter, request.Type),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }
    protected override CongressionalMeetingChamber Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
