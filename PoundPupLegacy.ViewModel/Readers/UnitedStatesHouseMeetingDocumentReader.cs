namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = UnitedStatesHouseMeetingDocumentReaderRequest;

public sealed record UnitedStatesHouseMeetingDocumentReaderRequest : IRequest
{
    public required int Number { get; init; }
    public required int TenantId { get; init; }
}
internal sealed class UnitedStatesHouseMeetingDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, CongressionalMeetingChamber>
{
    private static readonly NonNullableIntegerDatabaseParameter MeetingNumberParameter = new() { Name = "meeting_number" };
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly FieldValueReader<CongressionalMeetingChamber> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        party_affiliations as(
        	select
        	tpa.congressional_term_id house_term_id,
        	jsonb_agg(
        		jsonb_build_object(
        			'Title',
        			n.title,
                    'Path',
                    '/' || nt.viewer_path || '/' || n.id,
        			'From',
        			lower(tpa.date_range),
        			'To',
        			upper(tpa.date_range)
        		)
        	) "document"
        	from congressional_term_political_party_affiliation tpa
        	join united_states_political_party_affiliation pa on pa.id = tpa.united_states_political_party_affiliation_id
        	join node n on n.id = pa.id
            join node_type nt on nt.id = n.node_type_id
        	group by tpa.congressional_term_id

        ),
        bills as(
        	select
        	sba.representative_id,
        	jsonb_agg(
        		jsonb_build_object(
        			'Bill',
        			jsonb_build_object(
        				'Path',
        				'/' || nt.viewer_path || '/' || n.id,
        				'Title',
        				n.title
        			),
        			'BillAction',
        			jsonb_build_object(
        				'Path',
        				'/' || nt2.viewer_path || '/' || n2.id,
        				'Title',
        				n2.title
        			)
        		)
        	) "document"
        	from house_bill sb
        	join node n on n.id = sb.id
        	join node_type nt on nt.id = n.node_type_id
        	join representative_house_bill_action sba on sba.house_bill_id = sb.id
        	join bill_action_type ba on ba.id = sba.bill_action_type_id
        	join node n2 on n2.id = ba.id
        	join node_type nt2 on nt2.id = n2.node_type_id
        	group by sba.representative_id
        ),
        letters as(
        	select
        	n2.id person_id,
        	jsonb_agg(
        		jsonb_build_object(
        			'Path',
        			'/' || nt.viewer_path || '/' || n.id,
        			'Title',
        			n.title
        		)
        	) "document"
        	from "document" d 
        	join node n on n.id = d.id
        	join node_type nt on nt.id = n.node_type_id
        	join node_term ntm on ntm.node_id = d.id
        	join term t on t.id = ntm.term_id
        	join node n2 on n2.id = t.nameable_id
        	where d.document_type_id = 101489
        	and t.vocabulary_id = 100000
        	group by n2.id
        )
        select
        jsonb_build_object(
        	'MeetingName',
        	x.meeting_name,
        	'DateFrom',
        	"from",
        	'DateTo',
        	"to",
        	'States',
        	jsonb_agg(
        		jsonb_build_object(
                    'State',
                    jsonb_build_object(
        			    'Title',
        			    state_title,
        			    'Path',
        			    state_path
                    ),
        			'Members',
        			representatives
        		)
        	)
        ) "document"
        from(
        	select
        	'United States House - ' || n3.title meeting_name,
        	lower(m.date_range) "from",
        	upper(m.date_range) "to",
        	replace(n2.title, ' (state of the USA)', '') state_title,
        	'/' || nt2.viewer_path || '/' || n2.id state_path,
        	jsonb_agg(
        		jsonb_build_object(
        			'Title',
        			n.title,
        			'Path',
        			'/' || nt.viewer_path || '/' || n.id,
                    'From',
                    lower(st.date_range),
                    'To',
                    upper(st.date_range),
        			'FilePathPortrait',
        			'/attachment/' || f.id,
        			'Bills',
        			(select "document" from bills b where b.representative_id = s.id),
        			'Letters',
        			(select "document" from letters l where l.person_id = p.id),
        			'Parties',
        			(select "document" from party_affiliations pa where pa.house_term_id = st.id)
        		)
        	) representatives
        	from person p
        	join node n on n.id = p.id
        	join node_type nt on nt.id = n.node_type_id
        	left join "file" f on f.id = p.file_id_portrait
        	join professional_role pr on pr.person_id = p.id
        	join representative s on s.id = pr.id
        	join house_term st on st.representative_id = s.id
        	join subdivision sd on sd.id = st.subdivision_id
        	join node n2 on n2.id = sd.id
        	join node_type nt2 on nt2.id = n2.node_type_id
        	join united_states_congressional_meeting m on m.date_range && st.date_range
        	join node n3 on n3.id = m.id
        	join node_type nt3 on nt3.id = n3.node_type_id
        	where m.number = @meeting_number
        	group by 
        	n3.title, 
        	m.date_range,
        	n2.title,
        	nt2.viewer_path,
        	n2.id
        ) x
        group by 
        x.meeting_name,
        x.from,
        x.to
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(MeetingNumberParameter, request.Number),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }
    protected override CongressionalMeetingChamber Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
