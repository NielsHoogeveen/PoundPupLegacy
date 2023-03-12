using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;
using System.Text.RegularExpressions;

namespace PoundPupLegacy.Services.Implementation;
public partial class CongressionalDataService : ICongressionalDataService
{

    private enum ChamberType
    {
        House = 12660,
        Senate = 12662
    }

    private record ChamberTypeAndMeetingNumber
    {
        public required ChamberType ChamberType { get; init; }
        public required int Number { get; init; }
    }

    private readonly NpgsqlConnection _connection;
    private readonly ILogger<CongressionalDataService> _logger;
    private readonly IRazorViewToStringService _razorViewToStringService;
    public CongressionalDataService(
        NpgsqlConnection connection,
        ILogger<CongressionalDataService> logger,
        IRazorViewToStringService razorViewToStringService)
    {
        _connection = connection;
        _logger = logger;
        _razorViewToStringService = razorViewToStringService;
    }
    private ChamberTypeAndMeetingNumber? GetChamberTypeAndMeetingNumber(HttpRequest request)
    {
        var path = request.Path;
        var match = MatchIfCongressionalMeetingChamber().Match(path);
        if (!match.Success) {
            return null;

        }
        if (match.Groups.Count != 4) {
            return null;
        }
        var chamberType = match.Groups[1].Value switch {
            "senate" => ChamberType.Senate,
            "house_of_representatives" => ChamberType.House,
            _ => throw new Exception("Cannot reach")
        };
        var number = int.Parse(match.Groups[2].Value);
        return new ChamberTypeAndMeetingNumber {
            ChamberType = chamberType,
            Number = number
        };

    }

    public async Task<string?> GetCongressionalMeetingChamberResult(HttpContext context)
    {
        var congressionalMeetingChamber = GetChamberTypeAndMeetingNumber(context.Request);
        if (congressionalMeetingChamber is null) {
            return null;
        }
        try {
            await _connection.OpenAsync();
            using var command = _connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = MEETING_SQL;
            command.Parameters.Add("meeting_number", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("chamber_type", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["meeting_number"].Value = congressionalMeetingChamber.Number;
            command.Parameters["chamber_type"].Value = (int)congressionalMeetingChamber.ChamberType;
            var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) {
                return null;
            }
            await reader.ReadAsync();
            return await _razorViewToStringService.GetFromView("/Views/Shared/CongressionalMeetingChamber.cshtml", reader.GetFieldValue<CongressionalMeetingChamber>(0), context);

        }
        finally {
            await _connection.CloseAsync();
        }
    }

    public async Task<string?> GetUnitedStatesCongress(HttpContext context)
    {
        try {
            await _connection.OpenAsync();
            using var command = _connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = CONGRESS_SQL;
            await command.PrepareAsync();
            var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) {
                return null;
            }
            await reader.ReadAsync();
            return await _razorViewToStringService.GetFromView("/Views/Shared/UnitedStatesCongress.cshtml", reader.GetFieldValue<UnitedStatesCongress>(0), context);

        }
        finally {
            await _connection.CloseAsync();
        }
    }


    private const string REGEX = "united_states_(senate|house_of_representatives)_([0-9]+)(th|st|nd|rd)_congress";

    [GeneratedRegex(
        @"united_states_(senate|house_of_representatives)_([0-9]+)(th|st|nd|rd)_congress",
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1000)]
    private static partial Regex MatchIfCongressionalMeetingChamber();

    private const string CONGRESS_SQL = """
        select
        	jsonb_build_object(
        		'Senate',
        		jsonb_build_object(
        			'ImagePath',
        			image_path_senate,
        			'Meetings',
        			senate_meetings
        		),
        		'House',
        		jsonb_build_object(
        			'ImagePath',
        			image_path_house,
        			'Meetings',
        			house_meetings
        		)
        	)
        	from(
                select
        	        'files/userimages/Image/Seal_of_the_United_States_Senate-250.png' image_path_senate,
        	        (
        		        select
        			        jsonb_agg(
        				        jsonb_build_object(
        					        'Name',
        					        n.title,
        					        'Path',
        					        'united_states_senate_'|| LOWER(REPLACE(n.title, ' ', '_')),
        					        'DateFrom',
        					        lower(m.date_range),
        					        'DateTo',
        					        upper(m.date_range)
        				        )
        			        )
        		        from united_states_congressional_meeting m
        		        join node n on n.id = m.id
        	        ) senate_meetings,
        	        'files/userimages/Image/Seal_of_the_United_States_House_of_Representatives-250.png' image_path_house,
        	        (
        		        select
        			        jsonb_agg(
        				        jsonb_build_object(
        					        'Name',
        					        n.title,
        					        'Path',
        					        'united_states_house_of_representatives_'|| LOWER(REPLACE(n.title, ' ', '_')),
        					        'DateFrom',
        					        lower(m.date_range),
        					        'DateTo',
        					        upper(m.date_range)
        				        )
        			        )
        		from united_states_congressional_meeting m
        		join node n on n.id = m.id
        	) house_meetings
        ) x
        """;

    private const string MEETING_SQL = """
        select
        	json_build_object(
        		'MeetingName',	
        		meeting_name,
        		'DateFrom',
        		date_from,
        		'DateTo',
        		date_to,
        		'States',
        		jsonb_agg(
        			json_build_object(
        				'State',
        				json_build_object(
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

