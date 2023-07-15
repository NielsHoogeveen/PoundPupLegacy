using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;

namespace PoundPupLegacy.Convert;
internal sealed class PollMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<SingleQuestionPoll.ToCreate> singleQuestionPollCreatorFactory,
    IEntityCreatorFactory<MultiQuestionPoll.ToCreate> multiQuestionPollCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "polls";

    protected override async Task MigrateImpl()
    {
        await using var singleQuestionPollCreator = await singleQuestionPollCreatorFactory.CreateAsync(_postgresConnection);
        await using var multiQuestionPollCreator = await multiQuestionPollCreatorFactory.CreateAsync(_postgresConnection);
        await singleQuestionPollCreator.CreateAsync(ReadSingleQuestionPolls());
        await multiQuestionPollCreator.CreateAsync(ReadMultiQuestionPolls());
    }
    private async IAsyncEnumerable<SingleQuestionPoll.ToCreate> ReadSingleQuestionPolls()
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

        SingleQuestionPoll.ToCreate? currentPoll = null;
        int? currentDelta = null;

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            if (currentPoll is not null && currentPoll.NodeDetails.TenantNodes.First().UrlId != id) {
                yield return currentPoll;
                currentPoll = null;
                currentDelta = null;
            }
            currentPoll ??= new SingleQuestionPoll.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                    {
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                            SubgroupId = null,
                            UrlId = id
                        }
                    },
                    NodeTypeId = 53,
                    TermIds = new List<int>(),
                },
                SimpleTextNodeDetails = new SimpleTextNodeDetails {
                    Text = TextToHtml(reader.GetString("text")),
                    Teaser = TextToHtml(reader.GetString("text")),
                },
                PollDetails = new PollDetails {
                    DateTimeClosure = DateTime.Now.AddYears(-5),
                    PollStatusId = 0,
                },
                PollQuestionDetails = new PollQuestionDetails {
                    Question = reader.GetString("question"),
                    PollVotes = new List<PollVote>(),
                    PollOptions = new List<PollOption>(),
                },
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
                currentPoll.PollQuestionDetails.PollOptions.Add(opition);
            }
            currentPoll.PollQuestionDetails.PollVotes.Add(new PollVote {
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
    private async IAsyncEnumerable<MultiQuestionPoll.ToCreate> ReadMultiQuestionPolls()
    {
        MultiQuestionPoll.ToCreate? multiQuestionPoll = null;

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


            multiQuestionPoll = new MultiQuestionPoll.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                    {
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                            SubgroupId = null,
                            UrlId = id
                        }
                    },
                    NodeTypeId = 54,
                    TermIds = new List<int>(),
                },
                SimpleTextNodeDetails = new SimpleTextNodeDetails {
                    Text = TextToHtml(reader.GetString("text")),
                    Teaser = TextToHtml(reader.GetString("text")),
                },
                PollDetails =new PollDetails {
                    DateTimeClosure = DateTime.Now.AddYears(-5),
                    PollStatusId = 0,
                },
                MultiQuestionPollDetails = new MultiQuestionPollDetailsForCreate {
                    PollQuestions = new List<MultiQuestionPollQuestion.ToCreate>(),
                }
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

                MultiQuestionPollQuestion.ToCreate? currentQuestion = null;
                int? currentDelta = null;

                while (await reader.ReadAsync()) {
                    var id = reader.GetInt32("id");
                    var name = reader.GetString("title");

                    if (currentQuestion is not null && currentQuestion.NodeDetails.TenantNodes.First().UrlId != id) {
                        multiQuestionPoll.MultiQuestionPollDetails.PollQuestions.Add(currentQuestion);
                        currentQuestion = null;
                        currentDelta = null;
                    }
                    currentQuestion ??= new MultiQuestionPollQuestion.ToCreate {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        NodeDetails = new NodeDetails.ForCreate {
                            PublisherId = reader.GetInt32("user_id"),
                            CreatedDateTime = reader.GetDateTime("created_date_time"),
                            ChangedDateTime = reader.GetDateTime("changed_date_time"),
                            Title = name,
                            OwnerId = Constants.OWNER_CASES,
                            AuthoringStatusId = 1,
                            TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                            {
                                new TenantNode.ToCreate.ForNewNode
                                {
                                    Identification = new Identification.Possible {
                                        Id = null
                                    },
                                    TenantId = Constants.PPL,
                                    PublicationStatusId = reader.GetInt32("status"),
                                    UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                                    SubgroupId = null,
                                    UrlId = id
                                }
                            },
                            TermIds = new List<int>(),
                            NodeTypeId = 55,
                        },
                        SimpleTextNodeDetails = new SimpleTextNodeDetails {
                            Text = TextToHtml(reader.GetString("text")),
                            Teaser = TextToHtml(reader.GetString("text")),
                        },
                        PollQuestionDetails = new PollQuestionDetails {
                            Question = reader.GetString("question"),
                            PollOptions = new List<PollOption>(),
                            PollVotes = new List<PollVote>(),
                        },
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
                        currentQuestion.PollQuestionDetails.PollOptions.Add(opition);
                    }
                    currentQuestion.PollQuestionDetails.PollVotes.Add(new PollVote {
                        PollId = null,
                        Delta = delta,
                        UserId = reader.IsDBNull("user_id_vote") ? null : reader.GetInt32("user_id_vote"),
                        IpAddress = reader.IsDBNull("ip_address") ? null : reader.GetString("ip_address")
                    });
                }
                if (currentQuestion is not null) {
                    multiQuestionPoll.MultiQuestionPollDetails.PollQuestions.Add(currentQuestion);
                }
                await reader.CloseAsync();
                yield return multiQuestionPoll;
            }
        }
    }
}
