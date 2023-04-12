namespace PoundPupLegacy.ViewModel.Readers;

using Factory = UnitedStatesMeetingChamberDocumentReaderFactory;
using Reader = UnitedStatesMeetingChamberDocumentReader;
public class UnitedStatesMeetingChamberDocumentReaderFactory : DatabaseReaderFactory<Reader>
{
    internal readonly static NonNullableIntegerDatabaseParameter MeetingNumberParameter = new() { Name = "meeting_number" };
    internal readonly static NonNullableIntegerDatabaseParameter ChamberTypeParameter = new() { Name = "chamber_type" };

    internal readonly static FieldValueReader<CongressionalMeetingChamber> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = """
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
        					'Name',
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
        				'Name',
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
        			case 
        				when tn3.url_path is null then '/node/' || tn3.url_id
        				else '/' || tn3.url_path
        			end political_entity_path,
        			s.name political_entity_name,
        			case 
        				when tn.url_path is null then '/node/' || tn.url_id
        				else tn.url_path
        			end person_path,
        			n.title person_name,
                    f.path file_path_portrait,
        			(
        				select
        					jsonb_agg(
        						jsonb_build_object(
        							'Name',
        							"name",
        							'Path',
        							path
        						) 
        					) document

        				from(
        					select
        						n1.title "name",
        						case 
        							when tn1.url_path is null then '/node/' || tn1.url_id
        							else tn1.url_path
        						end path
        						from document d
        						join node n1 on n1.id = d.id
        						join tenant_node tn1 on tn1.node_id = n1.id and tn1.tenant_id = 1
        						join document_type dt on dt.id = d.document_type_id
        						join node n2 on n2.id = dt.id
        						join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        						join documentable_document dd on dd.document_id = d.id
        						join tenant_node tn3 on tn3.node_id = dd.documentable_id and tn3.tenant_id = 1
        						where tn2.url_id = 61030 and tn3.url_id = tn.url_id
        						order by n2.title
        				) x
        			) documents,
        			(
        				select
        				jsonb_agg(
        					jsonb_build_object(
        						'Name',
        						"name",
        						'Path',
        						path
        					)
        				) document
        				from(
        				select
        				n2.title "name",
        				case 
        					when tn2.url_path is null then '/node/' || tn2.url_id
        					else tn2.url_path
        				end path
        				from person p
        				join professional_role pr on pr.person_id = p.id
        				join senator_senate_bill_action sba on sba.senator_id = pr.id
        				join senate_bill sb on sb.id = sba.senate_bill_id
        				join node n2 on n2.id = sb.id
        				join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        				join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = 1
        				where tn1.url_id = tn.url_id
        				union
        				select
        				n2.title "name",
        				case 
        					when tn2.url_path is null then '/node/' || tn2.url_id
        					else tn2.url_path
        				end path
        				from person p
        				join professional_role pr on pr.person_id = p.id
        				join representative_house_bill_action sba on sba.representative_id = pr.id
        				join house_bill sb on sb.id = sba.house_bill_id
        				join node n2 on n2.id = sb.id
        				join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        				join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = 1
        				where tn1.url_id = tn.url_id
        					) x
        			) bills,
        			(
        				select
        				jsonb_agg(
        					jsonb_build_object(
        						'Name',
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
        					case
        						when tn2.url_path is null then '/node/' || tn2.url_id
        						else tn2.url_path
        					end path,
        					lower(por.date_range) date_from,
        					upper(por.date_range) date_to
        					from person p
        					join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = 1
        					join person_organization_relation por on por.person_id = p.id
                            JOIN united_states_political_party pp on pp.id = por.organization_id
        					join node n2 on n2.id = por.organization_id
        					join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        					where tn1.url_id = tn.url_id
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
        								'Name',
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
        						case 
        							when tn3.url_path is null then '/node/' || tn3.url_id
        							else '/' || tn3.url_path
        						end state_path,
        						lower(pper.date_range) date_from,
        						upper(pper.date_range) date_to
        						from person p
        						join tenant_node tn1 on tn1.node_id = p.id and tn1.tenant_id = 1 
        						join party_political_entity_relation pper on pper.party_id = p.id
        						join node n2 on n2.id = pper.party_political_entity_relation_type_id
        						join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
        						join subdivision s on s.id = pper.political_entity_id
        						join tenant_node tn3 on tn3.node_id = s.id and tn3.tenant_id = 1
        						where tn2.url_id in (12660, 12662)
        						and tn1.url_id = tn.url_id
        						order by lower(pper.date_range)
        					) x
        			) terms
        		from person p
                left join file f on f.id = p.file_id_portrait
        		join node n on n.id = p.id
        		join party_political_entity_relation pper on pper.party_id = p.id
        		join node n1 on n1.id = pper.id
        		join node n2 on n2.id = pper.party_political_entity_relation_type_id
        		join tenant_node tn on tn.node_id = n.id AND tn.tenant_id = 1
        		join iso_coded_subdivision ics on ics.id = pper.political_entity_id
        		join subdivision s on s.id = ics.id
        		join tenant_node tn2 on tn2.node_id = n2.id AND tn2.tenant_id = 1
        		join tenant_node tn3 on tn3.node_id = s.id AND tn3.tenant_id = 1
        		join united_states_congressional_meeting m on m.date_range && pper.date_range
        		join node n4 on n4.id = m.id
        		join tenant_node tn4 on tn4.node_id = n4.id AND tn4.tenant_id = 1
        		where 
        		tn2.url_id = @chamber_type and m.number = @meeting_number
        	) x
        	group by meeting_name, date_from, date_to,political_entity_path, political_entity_name
        ) x
        group by meeting_name, date_from, date_to
        """;

}
public class UnitedStatesMeetingChamberDocumentReader : SingleItemDatabaseReader<Reader.Request, CongressionalMeetingChamber>
{
    public record Request
    {
        public required int Number { get; init; }

        public required int Type { get; init; }

    }
    public UnitedStatesMeetingChamberDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.MeetingNumberParameter, request.Number),
            ParameterValue.Create(Factory.ChamberTypeParameter, request.Type),
        };
    }
    protected override CongressionalMeetingChamber Read(NpgsqlDataReader reader)
    {
        return Factory.DocumentReader.GetValue(reader);
    }
}
