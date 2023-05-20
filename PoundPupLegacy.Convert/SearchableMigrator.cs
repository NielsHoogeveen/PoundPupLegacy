namespace PoundPupLegacy.Convert;

internal class SearchableMigrator(
        IDatabaseConnections databaseConnections
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "searchables";

    protected override async Task MigrateImpl()
    {
        var sql = $"""
                UPDATE searchable
                set tsvector = subquery.tsvector
                from (
                    select
                    n.id,
                    to_tsvector(
                        'english', 
                        replace(
                            regexp_replace(
                                concat(
                                    a.text,
                                    ' ' ,
                                    n.title,
                                    ' ', 
                                    string_agg(cm.text, ' ')
                                ), 
                                E'<[^>]+>', 
                                '', 
                                'gi'
                            ),
                            '&nbsp;',
                            ' '
                        )
                    ) tsvector
                    from simple_text_node a
                    join blog_post b on b.id = a.id
                    join node n  on n.id = a.id
                    left join "comment" cm on cm.node_id = n.id
                    group by n.id, a.text, n.title
                ) subquery
                where searchable.id = subquery.id;
                UPDATE searchable
                set tsvector = subquery.tsvector
                from (
                    select
                    n.id,
                    to_tsvector('english', regexp_replace(concat(a.text,' ' ,n.title,' ', string_agg(cm.text, ' ')), E'<[^>]+>', '', 'gi')) tsvector
                    from simple_text_node a
                    join discussion b on b.id = a.id
                    join node n  on n.id = a.id
                    left join "comment" cm on cm.node_id = n.id
                    group by n.id, a.text, n.title
                ) subquery
                where searchable.id = subquery.id;
                UPDATE searchable
                set tsvector = subquery.tsvector
                from (
                    select
                        n.id,
                        to_tsvector(
                		    'english', 
                            replace(
                		        regexp_replace(
                			        concat(
                				        nm.description,
                				        ' ' ,
                				        n.title, 
                				        ' ',
                				        string_agg(cp.organizations, ' '),
                				        ' ',
                				        string_agg(cp.persons, ' '),
                				        ' ',
                				        string_agg(npo.title, ' '),
                				        ' ',
                				        string_agg(npp.title, ' '),
                                        ' ',
                                        string_agg(cm.text, ' ')
                			        ), 
                			        E'<[^>]+>', 
                			        '', 
                			        'gi'
                		        ),
                                '&nbsp;',
                                ' '
                            )
                	    ) tsvector
                    from "case" c
                    join nameable nm on nm.id = c.id
                    join node n  on n.id = c.id
                	left join case_case_parties ccp on ccp.case_id = c.id
                	left join case_parties cp on cp.id = ccp.case_parties_id
                	left join case_parties_organization cpo on cpo.case_parties_id = cp.id
                	left join case_parties_person cpp on cpp.case_parties_id = cp.id
                	left join node npo on npo.id = cpo.organization_id
                	left join node npp on npp.id = cpp.person_id
                    left join "comment" cm on cm.node_id = n.id
                	group by 
                	n.id, 
                	nm.description,
                	n.title
                ) subquery
                where searchable.id = subquery.id;
                UPDATE searchable
                set tsvector = subquery.tsvector
                from (
                    select
                    n.id,
                    to_tsvector(
                		'english', 
                		replace(
                			regexp_replace(
                				concat(
                					stn.text,
                					' ' , 
                					n.title, 
                					' ', 
                					a.source_url,
                					' ', 
                					string_agg(cm.text, ' ')
                				), 
                				E'<[^>]+>', 
                				'', 
                				'gi'
                			),
                			'&nbsp;',
                			' '
                		)
                	) tsvector
                    from "document" a
                    join simple_text_node stn on stn.id = a.id
                    join node n  on n.id = a.id
                    left join "comment" cm on cm.node_id = n.id
                    group by 
                    n.id,
                    stn.text, 
                    n.title,
                    a.source_url
                ) subquery
                where searchable.id = subquery.id;
                """;
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }
}
