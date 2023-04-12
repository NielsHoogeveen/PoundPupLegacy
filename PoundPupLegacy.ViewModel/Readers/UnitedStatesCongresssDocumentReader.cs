using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel.Readers;
public class UnitedStatesCongresssDocumentReaderFactory : DatabaseReaderFactory<UnitedStatesCongresssDocumentReader>
{
    public override string Sql => SQL;

    private const string SQL = """
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

}
public class UnitedStatesCongresssDocumentReader : SingleItemDatabaseReader<UnitedStatesCongresssDocumentReader.UnitedStatesCongresssDocumentRequest, UnitedStatesCongress>
{
    public record UnitedStatesCongresssDocumentRequest
    {
    }
    public UnitedStatesCongresssDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async Task<UnitedStatesCongress> ReadAsync(UnitedStatesCongresssDocumentRequest request)
    {
        var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetFieldValue<UnitedStatesCongress>(0);

    }

}
