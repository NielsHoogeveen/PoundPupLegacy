namespace PoundPupLegacy.Convert;
internal sealed class PollMigrator : MigratorPPL
{
    private readonly IEntityCreator<SingleQuestionPoll> _singleQuestionPollCreator;
    private readonly IEntityCreator<MultiQuestionPoll> _multiQuestionPollCreator;
    public PollMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<SingleQuestionPoll> singleQuestionPollCreator,
        IEntityCreator<MultiQuestionPoll> multiQuestionPollCreator
    ) : base(databaseConnections)
    {
        _singleQuestionPollCreator = singleQuestionPollCreator;
        _multiQuestionPollCreator = multiQuestionPollCreator;
    }

    protected override string Name => "polls";

    protected override async Task MigrateImpl()
    {
        await _singleQuestionPollCreator.CreateAsync(ReadSingleQuestionPolls(), _postgresConnection);
        await _multiQuestionPollCreator.CreateAsync(ReadMultiQuestionPolls(), _postgresConnection);
    }
    private async IAsyncEnumerable<SingleQuestionPoll> ReadSingleQuestionPolls()
    {

        var sql = $"""
                SELECT
                distinct
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) `changed_date_time`,
                nr.body `text`,
                fp.field_poll_question question,
                fp.field_poll_active active,
                fp.delta,
                fp.field_poll_choice option_text,
                fp.field_poll_votes number_of_votes,
                case 
                	when pv.uid = 0 then NULL 
                	ELSE pv.uid 
                END user_id_vote,
                case
                	when pv.uid <>  0 then null
                	ELSE pv.hostname 
                end ip_address,
                ua.dst url_path
                FROM node n 
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN content_field_poll fp ON fp.nid = n.nid AND fp.vid = n.vid
                JOIN pollfield pf ON pf.nid = n.nid
                JOIN pollfield_votes pv ON pv.nid = n.nid AND pv.field_table = pf.field_table AND pv.field_name = pf.field_name AND pv.delta = fp.delta
                WHERE n.`type` = 'award_poll' and pv.uid <> 72
                AND n.nid NOT IN (48443, 48446,48447 )
                ORDER BY n.nid, fp.delta
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        SingleQuestionPoll? currentPoll = null;
        int? currentDelta = null;

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            if (currentPoll is not null && currentPoll.TenantNodes.First().UrlId != id) {
                yield return currentPoll;
                currentPoll = null;
                currentDelta = null;
            }
            currentPoll ??= new SingleQuestionPoll {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_CASES,
                TenantNodes = new List<TenantNode>
                    {
                        new TenantNode
                        {
                            Id = null,
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                            NodeId = null,
                            SubgroupId = null,
                            UrlId = id
                        }
                    },
                Text = TextToHtml(reader.GetString("text")),
                Teaser = TextToHtml(reader.GetString("text")),
                DateTimeClosure = DateTime.Now.AddYears(-5),
                PollStatusId = 0,
                Question = reader.GetString("question"),
                NodeTypeId = 53,
                PollVotes = new List<PollVote>(),
                PollOptions = new List<PollOption>()
            };
            var delta = reader.GetInt32("delta");
            if (currentDelta is null || currentDelta != delta) {
                currentDelta = delta;
                var opition = new PollOption {
                    PollQuestionId = null,
                    Delta = delta,
                    Text = reader.GetString("option_text"),
                    NumberOfVotes = reader.GetInt32("number_of_votes")
                };
                currentPoll.PollOptions.Add(opition);
            }
            currentPoll.PollVotes.Add(new PollVote {
                PollId = null,
                Delta = delta,
                UserId = reader.IsDBNull("user_id_vote") ? null : reader.GetInt32("user_id_vote"),
                IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address")
            });
        }
        if (currentPoll is not null) {
            yield return currentPoll;
        }
        await reader.CloseAsync();
    }
    private async IAsyncEnumerable<MultiQuestionPoll> ReadMultiQuestionPolls()
    {
        MultiQuestionPoll? multiQuestionPoll = null;

        using (var readCommand = _mySqlConnection.CreateCommand()) {
            var sql = $"""
                SELECT
                distinct
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) `changed_date_time`,
                nr.body `text`,
                		'fifth_demons_of_adoption_awards' url_path
                FROM node n 
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                WHERE n.nid = 48445
                """;

            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;


            var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();

            var id = reader.GetInt32("id");
            var name = reader.GetString("title");


            multiQuestionPoll = new MultiQuestionPoll {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 54,
                Text = TextToHtml(reader.GetString("text")),
                Teaser = TextToHtml(reader.GetString("text")),
                DateTimeClosure = DateTime.Now.AddYears(-5),
                PollStatusId = 0,
                PollQuestions = new List<BasicPollQuestion>()
            };
            await reader.CloseAsync();
        }

        if (multiQuestionPoll is not null) {
            using (var readCommand = _mySqlConnection.CreateCommand()) {
                var sql = $"""
                SELECT
                distinct
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) `changed_date_time`,
                nr.body `text`,
                fp.field_poll_question question,
                fp.field_poll_active active,
                fp.delta,
                fp.field_poll_choice option_text,
                fp.field_poll_votes number_of_votes,
                case 
                	when pv.uid = 0 then NULL 
                	ELSE pv.uid 
                END user_id_vote,
                case
                	when pv.uid <>  0 then null
                	ELSE pv.hostname 
                end ip_address,
                ua.dst url_path
                FROM node n 
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN content_field_poll fp ON fp.nid = n.nid AND fp.vid = n.vid
                JOIN pollfield pf ON pf.nid = n.nid
                JOIN pollfield_votes pv ON pv.nid = n.nid AND pv.field_table = pf.field_table AND pv.field_name = pf.field_name AND pv.delta = fp.delta
                WHERE n.`type` = 'award_poll' and pv.uid <> 72
                AND n.nid IN (48443, 48446,48447 )
                ORDER BY n.nid, fp.delta
                """;

                readCommand.CommandType = CommandType.Text;
                readCommand.CommandTimeout = 300;
                readCommand.CommandText = sql;


                var reader = await readCommand.ExecuteReaderAsync();

                BasicPollQuestion? currentQuestion = null;
                int? currentDelta = null;

                while (await reader.ReadAsync()) {
                    var id = reader.GetInt32("id");
                    var name = reader.GetString("title");

                    if (currentQuestion is not null && currentQuestion.TenantNodes.First().UrlId != id) {
                        multiQuestionPoll.PollQuestions.Add(currentQuestion);
                        currentQuestion = null;
                        currentDelta = null;
                    }
                    currentQuestion ??= new BasicPollQuestion {
                        Id = null,
                        PublisherId = reader.GetInt32("user_id"),
                        CreatedDateTime = reader.GetDateTime("created_date_time"),
                        ChangedDateTime = reader.GetDateTime("changed_date_time"),
                        Title = name,
                        OwnerId = Constants.OWNER_CASES,
                        TenantNodes = new List<TenantNode>
                        {
                            new TenantNode
                            {
                                Id = null,
                                TenantId = Constants.PPL,
                                PublicationStatusId = reader.GetInt32("status"),
                                UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                                NodeId = null,
                                SubgroupId = null,
                                UrlId = id
                            }
                        },
                        Text = TextToHtml(reader.GetString("text")),
                        Teaser = TextToHtml(reader.GetString("text")),
                        Question = reader.GetString("question"),
                        NodeTypeId = 55,
                        PollVotes = new List<PollVote>(),
                        PollOptions = new List<PollOption>()
                    };
                    var delta = reader.GetInt32("delta");
                    if (currentDelta is null || currentDelta != delta) {
                        currentDelta = delta;
                        var opition = new PollOption {
                            PollQuestionId = null,
                            Delta = delta,
                            Text = reader.GetString("option_text"),
                            NumberOfVotes = reader.GetInt32("number_of_votes")
                        };
                        currentQuestion.PollOptions.Add(opition);
                    }
                    currentQuestion.PollVotes.Add(new PollVote {
                        PollId = null,
                        Delta = delta,
                        UserId = reader.IsDBNull("user_id_vote") ? null : reader.GetInt32("user_id_vote"),
                        IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address")
                    });
                }
                if (currentQuestion is not null) {
                    multiQuestionPoll.PollQuestions.Add(currentQuestion);
                }
                await reader.CloseAsync();
                yield return multiQuestionPoll;
            }
        }
    }
}
