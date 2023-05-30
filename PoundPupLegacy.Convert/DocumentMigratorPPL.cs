namespace PoundPupLegacy.Convert;

internal sealed class DocumentMigratorPPL(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IEntityCreatorFactory<Document.ToCreate> documentCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "documents ppl";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var documentCreator = await documentCreatorFactory.CreateAsync(_postgresConnection);
        await documentCreator.CreateAsync(ReadDocuments(nodeIdReader));
    }

    private async IAsyncEnumerable<Document.ToCreate> ReadDocuments(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {


        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    10 node_type_id,
                    r.field_report_date_value publication_date,
                    case when r.field_web_address_url = '' then null else r.field_web_address_url end source_url,
                    nr.body `text`,
                    c.document_type_id
                FROM node n
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN (
                select 
                    c.nid,
                    n2.nid document_type_id
                FROM category_node c 
                JOIN node n2 ON n2.nid = c.cid
                JOIN category cat ON cat.cid = c.cid AND cat.cnid = 42416 
                JOIN node n3 ON n3.nid = cat.cnid 
                ) c ON c.nid = n.nid 
                WHERE n.`type` = 'adopt_ind_rep'
                AND n.nid not in (
                14761,
                15678,
                15933,
                16184,
                16185,
                16186,
                16187,
                16188,
                16190,
                16193,
                16194,
                16202,
                16216,
                16218,
                16219,
                16224,
                16225,
                16226,
                16272,
                16273,
                16274,
                16276,
                16277,
                16278,
                16279,
                16280,
                16281,
                16288,
                16290,
                16291,
                16292,
                16293,
                16294,
                16295,
                16297,
                16299,
                16331,
                16332,
                16333,
                16334,
                16335,
                16336,
                16337,
                16338,
                16339,
                16345,
                16346,
                16347,
                16348,
                16353,
                16354,
                16355,
                16356,
                16357,
                16359,
                16360,
                16374,
                16386,
                16393,
                16394,
                16395,
                16396,
                16397,
                16398,
                16399,
                16400,
                16401,
                16402,
                16403,
                16404,
                16405,
                16406,
                16407,
                16408,
                16409,
                16410,
                16411,
                16412,
                16413,
                16414,
                16415,
                16416,
                16417,
                16419,
                16439,
                16450,
                16451,
                16452,
                16457,
                16459,
                16460,
                16462,
                16464,
                16466,
                16468,
                16473,
                16476,
                16477,
                16481,
                16482,
                16483,
                16487,
                16488,
                16491,
                16492,
                16493,
                16494,
                16495,
                16496,
                16499,
                16502,
                16503,
                16582,
                16583,
                16584,
                16585,
                16586,
                16587,
                16588,
                16589,
                16590,
                16591,
                16592,
                16593,
                16594,
                16595,
                16958,
                16959,
                16960,
                16961,
                16962,
                16963,
                16964,
                16965,
                16966,
                16967,
                16968,
                16969,
                16971,
                16972,
                17000,
                17007,
                17008,
                17009,
                17010,
                17015,
                17018,
                17235,
                17257,
                17267)
                UNION
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    10 node_type_id,
                    r.field_date_value publication_date,
                    case when r.field_source_url = '' then null else r.field_source_url end source_url,
                    r.field_body_value `text`,
                    c.document_type_id
                FROM node n
                JOIN content_type_case_file r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN (
                SELECT
                    c.nid,
                    n2.nid document_type_id
                FROM 
                category_node c 
                JOIN node n2 ON n2.nid = c.cid
                JOIN category cat ON cat.cid = c.cid AND cat.cnid = 42416 
                ) c ON c.nid = n.nid 
                WHERE n.`type` = 'case_file'
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var publicationDate = StringToDateTimeRange(reader.IsDBNull("publication_date") ? null : reader.GetString("publication_date"))?.ToFuzzyDate();
            var id = reader.GetInt32("id");
            yield return new Document.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.OWNER_DOCUMENTATION,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = 1,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id
                        },
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id < 33163 ? id : null
                        }
                    },
                    NodeTypeId = reader.GetInt16("node_type_id"),
                    TermIds = new List<int>(),
                },
                SimpleTextNodeDetails = new SimpleTextNodeDetails {
                    Text = TextToHtml(reader.GetString("text")),
                    Teaser = TextToTeaser(reader.GetString("text")),
                },
                DocumentDetails = new DocumentDetails {
                    Published = publicationDate,
                    SourceUrl = reader.IsDBNull("source_url") ? null : reader.GetString("source_url"),
                    DocumentTypeId = reader.IsDBNull("document_type_id")
                    ? null
                    : await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        UrlId = reader.GetInt32("document_type_id"),
                        TenantId = Constants.PPL,
                    }),
                }
            };
        }
        await reader.CloseAsync();
    }
}
