namespace PoundPupLegacy.Convert;

internal sealed class TermHierarchyMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderByUrlIdFactory,
        ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableIdFactory,
        IInsertingEntityCreatorFactory<TermHierarchy> termHierarchyCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "node term hierarchy";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);
        await using var termHierarchyCreator = await termHierarchyCreatorFactory.CreateAsync(_postgresConnection);
        await termHierarchyCreator.CreateAsync(ReadTermHierarchys(nodeIdReader, termReaderByNameableId));
    }
    private async IAsyncEnumerable<TermHierarchy> ReadTermHierarchys(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        ISingleItemDatabaseReader<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableId
    )
    {

        var sql = $"""
                SELECT
                DISTINCT
                n.nid node_id_child,
                case
                   when n4.node_id = 19316 then 39429
                   when n4.node_id = 4806 then 17310
                   when n4.node_id = 19326 then 35715
                   when n4.node_id = 19447 then 31716
                   when n4.node_id = 19597 then 12632
                   when n4.node_id = 20242 then 48309
                   when n4.node_id = 4816 then 12635
                   when n4.node_id = 27466 then 12635
                   when n4.node_id = 47130 then 12624
                   when n4.node_id = 74755 then 31586
                   when n4.node_id = 18644 then 14670
                   when n4.node_id = 20899 then 27215
                   when n4.node_id = 6167 then 27214
                   when n4.node_id = 4209 then 12625
                   when n4.node_id = 19256 then 6135
                   when n4.node_id = 22589 then 22591
                   when n4.node_id = 45656 then 41375
                   when n4.node_id = 74730 then 55660
                   when n3.node_id IS NULL then n4.node_id
                   ELSE n3.node_id
                end node_id_parent
                FROM(
                	SELECT
                		n.nid 
                	FROM node n
                	LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                	JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                	JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                	JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                	LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` IN ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case')
                	LEFT JOIN node n3 ON n3.nid = cc.field_related_page_nid
                	WHERE  (n3.nid IS NULL OR n3.`type` = 'group')
                	AND n.title NOT IN 
                	(
                	  'adoption',
                	  'adoptive mother',
                	  'foster care',
                	  'guardianship',
                	  'institutional care',
                	  'adoption agencies',
                	  'legal guardians',
                	  'church',
                	  'boot camp',
                	  'media',
                	  'blog',
                	  'orphanages',
                	  'orphanage',
                	  'adoption advocates',
                	  'boarding school',
                	  'adoption facilitators',
                	  'maternity homes',
                	  'sexual abuse',
                	  'sexual exploitation',
                	  'lethal neglect',
                	  'lethal deprivation',
                	  'economic exploitation',
                	  'verbal abuse',
                	  'medical abuse',
                	  'lawyers',
                	  'therapists',
                	  'Christianity',
                	  'Northern Ireland',
                	  'mega families',
                	  'Mary Landrieu',
                	  'Hilary Clinton',
                	  'Michele Bachmann',
                	  'Kevin and Kody Pribbernow'
                	)
                	AND n.nid NOT IN (
                	  22589, 74730
                	)
                	AND n2.nid IS  NULL
                ) n
                join category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                JOIN category c ON c.cid = n2.nid
                LEFT JOIN(
                     SELECT 
                     cc.nid,
                     n.nid node_id
                     FROM content_type_category_cat cc 
                     JOIN node n3 ON n3.nid = cc.nid AND n3.vid = cc.vid
                     JOIN node n ON n.nid = cc.field_related_page_nid
                     WHERE n.`type` NOT IN ('group')
                 ) n3 ON n3.nid = c.cid
                 LEFT JOIN(
                     SELECT 
                     case 
                 		when n2.nid is NULL then n.nid 
                 		ELSE n2.nid
                 	end node_id,
                     n.nid
                     FROM node n 
                     LEFT JOIN node n2 ON n2.title = n.title and n2.`type` in ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case')
                 ) n4 ON n4.nid = c.cid
                 WHERE  n.nid NOT IN (
                     22589,
                     54123
                 )
                 AND (n3.nid IS NOT NULL OR n4.nid IS NOT NULL)
                 AND n4.nid not in (4126)
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        async Task<TermHierarchy?> WriteTermHierarchy(int childNodeId, int parentNodeId)
        {
            var (childUrlId, childTenantId) = GetUrlIdAndTenant(childNodeId);
            var nodeIdChild = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = childTenantId,
                UrlId = childUrlId
            });
            var (parentUrlId, parentTenantId) = GetUrlIdAndTenant(parentNodeId);
            var nodeIdParent = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = parentTenantId,
                UrlId = parentUrlId
            });
            var termIdChild = await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.OWNER_SYSTEM,
                VocabularyName = Constants.VOCABULARY_TOPICS,
                NameableId = nodeIdChild
            });
            var termIdParent = await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.OWNER_SYSTEM,
                VocabularyName = Constants.VOCABULARY_TOPICS,
                NameableId = nodeIdParent
            });
            if (termIdChild is not null && termIdParent is not null) {
                return new TermHierarchy {

                    TermIdChild = termIdChild.Id!.Value,
                    TermIdPartent = termIdParent.Id!.Value,
                };
            }
            else {
                return null;
            }
        }
        while (await reader.ReadAsync()) {
            var res = await WriteTermHierarchy(reader.GetInt32("node_id_child"), reader.GetInt32("node_id_parent"));
            if (res is not null)
                yield return res;
        }
        await reader.CloseAsync();
        await WriteTermHierarchy(6638, 14781);
    }
}
