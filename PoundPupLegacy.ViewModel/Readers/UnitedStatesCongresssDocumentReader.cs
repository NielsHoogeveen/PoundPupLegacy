namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = UnitedStatesCongresssDocumentReaderRequest;

public sealed record UnitedStatesCongresssDocumentReaderRequest : IRequest
{
}

internal sealed class UnitedStatesCongresssDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, UnitedStatesCongress>
{
    private static readonly FieldValueReader<UnitedStatesCongress> DocumentReader = new() { Name = "document" };
    public override string Sql => SQL;

    private const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
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
        	) document
        	from(
                select
        	        'files/userimages/Image/Seal_of_the_United_States_Senate-250.png' image_path_senate,
        	        (
        		        select
        			        jsonb_agg(
        				        jsonb_build_object(
        					        'Title',
        					        n.title,
        					        'Path',
        					        '/united_states_senate/' || m.number,
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
        					        'Title',
        					        n.title,
        					        'Path',
        					        '/united_states_house_of_representatives/' || m.number,
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
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
        };
    }
    protected override UnitedStatesCongress Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
