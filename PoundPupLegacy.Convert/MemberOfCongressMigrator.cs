using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;
using System.Text.Json;

namespace PoundPupLegacy.Convert;

public record MemberOfCongress
{
    public int? node_id { get; set; }
    public Id id { get; set; }
    public Bio bio { get; set; }
    public Name name { get; set; }
    public Term[] terms { get; set; }
    public LeadershipRole[] leadership_roles { get; set; }

}

public record Bio
{
    public DateTime? birthday { get; set; }
    public string gender { get; set; }
}

public record Id
{
    public string bioguide { get; set; }
    public string thomas { get; set; }
    public string lis { get; set; }
    public int govtrack { get; set; }
    public string opensecrets { get; set; }
    public int votesmart { get; set; }
    public string[] fec { get; set; }
    public int cspan { get; set; }
    public string wikipedia { get; set; }
    public long house_history { get; set; }
    public string ballotpedia { get; set; }
    public int maplight { get; set; }
    public int icpsr { get; set; }
    public string wikidata { get; set; }
    public string google_entity_id { get; set; }
}
public record Term
{
    public string type { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string state { get; set; }
    public int? district { get; set; }
    public int? @class { get; set; }
    public string party { get; set; }
    public string how { get; set; }

    public string state_rank { get; set; }

    public string caucus { get; set; }

    public PartyAffiliation[] party_affiliations { get; set; }
}
public record Name
{
    public string first { get; set; }
    public string middle { get; set; }
    public string last { get; set; }
    public string suffix { get; set; }

    public string nickname { get; set; }
    public string official_full { get; set; }
}

public record PartyAffiliation
{
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string party { get; set; }
}

public record LeadershipRole
{
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string title { get; set; }
    public string chamber { get; set; }
}

public enum MemberType
{
    Representative,
    Senator
}
public record TempTerm
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public MemberType MemberType { get; set; }

    public string State { get; set; }
}


public record StoredTerm
{
    public required int GovtrackId { get; init; }
    public required int PersonId { get; init; }

    public required string PersonName { get; init; }
    public required int? NodeId { get; init; }
    public required DateTime StartDate { get; set; }
    public required DateTime? EndDate { get; set; }
    public required int RelationTypeId { get; init; }
    public required string RelationTypeName { get; init; }
    public int PoliticalEntityId { get; set; }
    public required string PoliticalEntityCode { get; set; }
    public required bool Delete { get; set; }
    public required int? DocumentId { get; init; }
}

internal class MemberOfCongressMigrator : PPLMigrator
{

    private List<MemberOfCongress> _membersOfCongress = new List<MemberOfCongress>();

    public MemberOfCongressMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "members of congress";

    protected override async Task MigrateImpl()
    {

        _membersOfCongress = (await GetMembersOfCongress().ToListAsync()).OrderBy(x => x.id.govtrack).ToList();

        var parties = _membersOfCongress.SelectMany(x => x.terms.Select(y => y.party)).Distinct().ToList();
        var persons = await GetMembersOfCongressAsync().ToListAsync();
        await PersonCreator.CreateAsync(persons.ToAsyncEnumerable(), _postgresConnection);

        var files = await GetImageFiles().ToListAsync();
        await FileCreator.CreateAsync(files.ToAsyncEnumerable(), _postgresConnection);
        var nodeImages = await GetNodeFilesImage().ToListAsync();
        await NodeFileCreator.CreateAsync(nodeImages.ToAsyncEnumerable(), _postgresConnection);
        await UpdatePerson(nodeImages.ToAsyncEnumerable());
    }


    private async IAsyncEnumerable<(string, int)> GetStates()
    {
        using (var command = _postgresConnection.CreateCommand()) {
            var sql = """
                select
                    s.id,
                    iso_3166_2_code
                from iso_coded_subdivision ics
                join subdivision s on s.id = ics.id
                join country c on c.id = s.country_id
                join tenant_node tn on tn.node_id = c.id
                where tn.tenant_id = 1 and tn.url_id = 3805
                """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            await command.PrepareAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                yield return (reader.GetString(1), reader.GetInt32(0));
            }
            await reader.CloseAsync();
        }

    }
    private async IAsyncEnumerable<(string, int)> GetPoliticalPartyAffiliations()
    {
        using (var command = _postgresConnection.CreateCommand()) {
            var sql = """
                SELECT 
                    usppa.id,
                	t.name
                FROM united_states_political_party_affiliation usppa
                join term t on t.nameable_id =  usppa.id
                join tenant_node tn on tn.node_id = t.vocabulary_id and tn.tenant_id = 1
                where tn.url_id = 150
                """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            await command.PrepareAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                yield return (reader.GetString(1), reader.GetInt32(0));
            }
            await reader.CloseAsync();
        }

    }

    private async Task<int?> FindIdByGovtackId(int govtrack)
    {
        int? id = govtrack switch
        {
            412236 => 60496,
            412434 => 62239,
            400199 => 38347,
            400355 => 10518,
            412576 => 62603,
            412201 => 10612,
            412468 => 51383,
            400148 => 63107,
            412499 => 62786,
            412453 => 61589,
            400191 => 10511,
            412283 => 62038,
            412383 => 62410,
            412291 => 62980,
            400098 => 60381,
            400571 => 39152,
            400365 => 11240,
            412584 => 62677,
            412311 => 62115,
            400562 => 62304,
            400254 => 60517,
            400132 => 60521,
            400007 => 60389,
            400260 => 38977,
            412636 => 74281,
            400275 => 61196,
            412420 => 51342,
            412537 => 62192,
            400635 => 62966,
            400305 => 63048,
            400342 => 10781,
            400084 => 10323,
            400139 => 10820,
            412233 => 11149,
            400114 => 11146,
            400312 => 10813,
            400366 => 38369,
            400137 => 38413,
            300051 => 10727,
            300083 => 10729,
            300058 => 11155,
            400404 => 11171,
            400568 => 39136,
            400582 => 39202,
            400585 => 39211,
            400604 => 39248,
            400516 => 39276,
            400519 => 61293,
            400074 => 38951,
            400236 => 38973,
            400428 => 39119,
            400094 => 39177,
            400205 => 39232,
            400398 => 61082,
            400207 => 61948,
            400202 => 63139,
            412630 => 74263,
            412638 => 74288,
            412506 => 62396,
            412389 => 61830,
            400557 => 62323,
            400556 => 39098,
            _ => null,
        };
        if (!id.HasValue)
        {
            return null;
        }
        return await _nodeIdReader.ReadAsync(Constants.PPL, id.Value);
    }

    private async IAsyncEnumerable<Person> GetMembersOfCongressAsync()
    {
        var states = await GetStates().ToListAsync();
        var politicalPartyAffiliations = await GetPoliticalPartyAffiliations().ToListAsync();

        const string updateSql = """
            UPDATE person SET 
                first_name = @first_name, 
                last_name = @last_name, 
                middle_name = @middle_name, 
                nick_name = @nick_name, 
                suffix = @suffix, 
                full_name = @full_name, 
                govtrack_id = @govtrack_id,
                bioguide = @bioguide
                WHERE id = @id
            """;
        const string readSql = """
            SELECT
            n.id,
            n.title,
            p.date_of_birth
            FROM node n
            join person p on p.id = n.id
            where 
                (
                    (
                        (n.title like @first_name and n.title like @last_name) 
                        or 
                        n.title = @wikiname 
                        or 
                        n.title = @ballotpedia 
                        or 
                        n.title = @wikipedia2
                    ) 
                    and p.date_of_birth = @date_of_birth
                    and 
                    n.id in (
                        select
                            p.party_id
                        from party_political_entity_relation p
                        join tenant_node tn on tn.node_id = p.party_political_entity_relation_type_id
                        where tn.url_id in (12660, 12662)
                    )
                )
            """;

        using var senCommand = _postgresConnection.CreateCommand();
        senCommand.CommandType = CommandType.Text;
        senCommand.CommandTimeout = 300;
        senCommand.CommandText = "select n.id from profession p join node n on n.id = p.id where n.title = 'Senator'";
        int senatorRoleId = (int)(await senCommand.ExecuteScalarAsync())!;

        using var repCommand = _postgresConnection.CreateCommand();
        repCommand.CommandType = CommandType.Text;
        repCommand.CommandTimeout = 300;
        repCommand.CommandText = "select n.id from profession p join node n on n.id = p.id where n.title = 'Representative'";
        int repRoleId = (int)(await repCommand.ExecuteScalarAsync())!;

        using (var readCommand = _postgresConnection.CreateCommand())
        {
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = readSql;
            readCommand.Parameters.Add("first_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            readCommand.Parameters.Add("last_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            readCommand.Parameters.Add("wikiname", NpgsqlTypes.NpgsqlDbType.Varchar);
            readCommand.Parameters.Add("wikipedia2", NpgsqlTypes.NpgsqlDbType.Varchar);
            readCommand.Parameters.Add("ballotpedia", NpgsqlTypes.NpgsqlDbType.Varchar);
            readCommand.Parameters.Add("date_of_birth", NpgsqlTypes.NpgsqlDbType.Timestamp);
            await readCommand.PrepareAsync();


            foreach (var memberOfCongress in _membersOfCongress)
            {
                var id = await FindIdByGovtackId(memberOfCongress.id.govtrack);
                if (id.HasValue)
                {
                    memberOfCongress.node_id = id.Value;
                }
                else
                {
                    readCommand.Parameters["first_name"].Value = $"%{memberOfCongress.name.first}%";
                    readCommand.Parameters["last_name"].Value = $"%{memberOfCongress.name.last}%";
                    readCommand.Parameters["wikiname"].Value = string.IsNullOrEmpty(memberOfCongress.id.wikipedia) ? "" : memberOfCongress.id.wikipedia;
                    readCommand.Parameters["wikipedia2"].Value = string.IsNullOrEmpty(memberOfCongress.id.wikipedia) ? "" : memberOfCongress.id.wikipedia.Replace("(politician)", "").Trim();
                    readCommand.Parameters["ballotpedia"].Value = string.IsNullOrEmpty(memberOfCongress.id.ballotpedia) ? "" : memberOfCongress.id.ballotpedia;
                    readCommand.Parameters["date_of_birth"].Value = memberOfCongress.bio.birthday is null ? DBNull.Value : memberOfCongress.bio.birthday;
                    using var reader = await readCommand.ExecuteReaderAsync();
                    bool found = false;
                    while (found == false && await reader.ReadAsync())
                    {
                        memberOfCongress.node_id = reader.GetInt32("id");
                        found = true;
                        break;
                    }
                }
            }
        }
        using (var updateCommand = _postgresConnection.CreateCommand())
        {
            updateCommand.CommandTimeout = 300;
            updateCommand.CommandType = CommandType.Text;
            updateCommand.CommandTimeout = 300;
            updateCommand.CommandText = updateSql;
            updateCommand.Parameters.Add("first_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("last_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("middle_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("full_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("suffix", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("nick_name", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("govtrack_id", NpgsqlTypes.NpgsqlDbType.Integer);
            updateCommand.Parameters.Add("bioguide", NpgsqlTypes.NpgsqlDbType.Varchar);
            updateCommand.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            await updateCommand.PrepareAsync();
            foreach (var memberOfCongress in _membersOfCongress)
            {
                var isSenator = memberOfCongress.terms.Any(x => x.type == "sen");
                var isRepresentative = memberOfCongress.terms.Any(x => x.type == "rep");

                var name = memberOfCongress.name.official_full is null ? $"{memberOfCongress.name.first} {memberOfCongress.name.middle} {memberOfCongress.name.last} {memberOfCongress.name.suffix}".Replace("  ", " ") : memberOfCongress.name.official_full;

                if (memberOfCongress.id.govtrack == 400568)
                {
                    Console.WriteLine($"{memberOfCongress.name.official_full} isSenator {isSenator} isRepresentative {isRepresentative}");
                }

                var professionalRoles = new List<ProfessionalRole>();

                int GetPoliticalPartyAffiliationId(Term term)
                {
                    return politicalPartyAffiliations!.First(x => x.Item1 == term.party.ToLower()).Item2;
                }

                List<CongressionalTermPoliticalPartyAffiliation> GetPartyAffiliations(Term term)
                {
                    if (term.party_affiliations == null) {
                        return new List<CongressionalTermPoliticalPartyAffiliation> {
                            new CongressionalTermPoliticalPartyAffiliation {
                                Id = null,
                                PublisherId = 2,
                                CreatedDateTime = DateTime.Now,
                                ChangedDateTime = DateTime.Now,
                                Title = $"{name} is {term.party} from {term.start.ToString("dd MMMM yyyy")} to {term.end.ToString("dd MMMM yyyy")}",
                                OwnerId = Constants.OWNER_PARTIES,
                                TenantNodes = new List<TenantNode> {
                                    new TenantNode {
                                        Id = null,
                                        TenantId = 1,
                                        PublicationStatusId = 1,
                                        UrlPath = null,
                                        NodeId = null,
                                        SubgroupId = null,
                                        UrlId = null
                                    }
                                },
                                NodeTypeId = 64,
                                PoliticalPartyAffiliationId = GetPoliticalPartyAffiliationId(term),
                                CongressionalTermId = null,
                                DateTimeRange = new DateTimeRange(term.start, term.end)
                            }
                        };
                    }
                    return term.party_affiliations.Select(party_affiliations => new CongressionalTermPoliticalPartyAffiliation 
                        {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is {term.party} from {term.start.ToString("dd MMMM yyyy")} to {term.end.ToString("dd MMMM yyyy")}",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                        new TenantNode {
                                            Id = null,
                                            TenantId = 1,
                                            PublicationStatusId = 1,
                                            UrlPath = null,
                                            NodeId = null,
                                            SubgroupId = null,
                                            UrlId = null
                                        }
                                    },
                            NodeTypeId = 64,
                            PoliticalPartyAffiliationId = GetPoliticalPartyAffiliationId(term),
                            CongressionalTermId = null,
                            DateTimeRange = new DateTimeRange(party_affiliations.start, party_affiliations.end)
                        }
                    ).ToList();
                }

                int GetStateId(Term term)
                {
                    return states!.First(x => x.Item1 == $"US-{term.state}").Item2;
                }

                List<SenateTerm> GetSenateTerms()
                {
                    return memberOfCongress.terms.Where(x => x.type == "sen").Select(term => {
                        var subdivisionId = GetStateId(term);
                        var partyAffiliations = GetPartyAffiliations(term);
                        var senateTerm = new SenateTerm {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is senator",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                new TenantNode {
                                    Id = null,
                                    TenantId = 1,
                                    PublicationStatusId = 1,
                                    UrlPath = null,
                                    NodeId = null,
                                    SubgroupId = null,
                                    UrlId = null
                                }
                            },
                            NodeTypeId = 65,
                            SenatorId = null,
                            DateTimeRange = new DateTimeRange(term.start, term.end),
                            SubdivisionId = subdivisionId,
                            PartyAffiliations = partyAffiliations
                        };
                        return senateTerm;
                    }).ToList();
                }
                List<HouseTerm> GetHouseTerms()
                {
                    return memberOfCongress.terms.Where(x => x.type == "rep").Select(term => {
                        var subdivisionId = GetStateId(term);
                        var partyAffiliations = GetPartyAffiliations(term);

                        var houseTerm = new HouseTerm {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is representative",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                    new TenantNode {
                                        Id = null,
                                        TenantId = 1,
                                        PublicationStatusId = 1,
                                        UrlPath = null,
                                        NodeId = null,
                                        SubgroupId = null,
                                        UrlId = null
                                    }
                                },
                            NodeTypeId = 65,
                            RepresentativeId = null,
                            District = term.district,
                            DateTimeRange = new DateTimeRange(term.start, term.end),
                            SubdivisionId = subdivisionId,
                            PartyAffiliations = partyAffiliations
                        };
                        return houseTerm;
                    }).ToList();

                }

                if (memberOfCongress.node_id.HasValue)
                {
                    if (isSenator)
                    {
                        professionalRoles.Add(new Senator
                        {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is senator",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                new TenantNode {
                                    Id = null,
                                    TenantId = 1,
                                    PublicationStatusId = 1,
                                    UrlPath = null,
                                    NodeId = null,
                                    SubgroupId = null,
                                    UrlId = null
                                }
                            },
                            NodeTypeId = 59,
                            PersonId = memberOfCongress.node_id,
                            DateTimeRange = null,
                            ProfessionId = senatorRoleId,
                            SenateTerms = GetSenateTerms(),
                        });
                    }
                    if (isRepresentative)
                    {
                        professionalRoles.Add(new Representative
                        {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is representative",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                new TenantNode {
                                    Id = null,
                                    TenantId = 1,
                                    PublicationStatusId = 1,
                                    UrlPath = null,
                                    NodeId = null,
                                    SubgroupId = null,
                                    UrlId = null
                                }
                            },
                            NodeTypeId = 60,
                            PersonId = memberOfCongress.node_id,
                            DateTimeRange = null,
                            ProfessionId = repRoleId,
                            HouseTerms = GetHouseTerms(),
                        });
                    }
                    updateCommand.Parameters["first_name"].Value = memberOfCongress.name.first is null ? DBNull.Value : memberOfCongress.name.first;
                    updateCommand.Parameters["last_name"].Value = memberOfCongress.name.last is null ? DBNull.Value : memberOfCongress.name.last;
                    updateCommand.Parameters["middle_name"].Value = memberOfCongress.name.middle is null ? DBNull.Value : memberOfCongress.name.middle;
                    updateCommand.Parameters["full_name"].Value = memberOfCongress.name.official_full is null ? DBNull.Value : memberOfCongress.name.official_full;
                    updateCommand.Parameters["suffix"].Value = memberOfCongress.name.suffix is null ? DBNull.Value : memberOfCongress.name.suffix;
                    updateCommand.Parameters["nick_name"].Value = memberOfCongress.name.nickname is null ? DBNull.Value : memberOfCongress.name.nickname;
                    updateCommand.Parameters["govtrack_id"].Value = memberOfCongress.id.govtrack;
                    updateCommand.Parameters["bioguide"].Value = memberOfCongress.id.bioguide;
                    updateCommand.Parameters["id"].Value = memberOfCongress.node_id;
                    await updateCommand.ExecuteNonQueryAsync();
                    await ProfessionalRoleCreator.CreateAsync(professionalRoles.ToAsyncEnumerable(), _postgresConnection);
                }
                else
                {
                    
                    if (isSenator)
                    {
                        

                        professionalRoles.Add(new Senator
                        {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is senator",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                new TenantNode {
                                    Id = null,
                                    TenantId = 1,
                                    PublicationStatusId = 1,
                                    UrlPath = null,
                                    NodeId = null,
                                    SubgroupId = null,
                                    UrlId = null
                                }
                            },
                            NodeTypeId = 59,
                            PersonId = null,
                            DateTimeRange = null,
                            ProfessionId = senatorRoleId,
                            SenateTerms = GetSenateTerms(),
                        });
                    }
                    if (isRepresentative)
                    {
                        professionalRoles.Add(new Representative
                        {
                            Id = null,
                            PublisherId = 2,
                            CreatedDateTime = DateTime.Now,
                            ChangedDateTime = DateTime.Now,
                            Title = $"{name} is representative",
                            OwnerId = Constants.OWNER_PARTIES,
                            TenantNodes = new List<TenantNode> {
                                new TenantNode {
                                    Id = null,
                                    TenantId = 1,
                                    PublicationStatusId = 1,
                                    UrlPath = null,
                                    NodeId = null,
                                    SubgroupId = null,
                                    UrlId = null
                                }
                            },
                            NodeTypeId = 60,
                            PersonId = null,
                            DateTimeRange = null,
                            ProfessionId = repRoleId,
                            HouseTerms = GetHouseTerms(),
                        });
                    }

                    var terms = memberOfCongress.terms.Select(x => (x.start, x.end, x.party)).GroupBy(x => x.party).ToList();

                    var relations = new List<PersonOrganizationRelation>();
                    if(terms.Count == 1) {

                    }

                    yield return new Person {
                        Id = null,
                        PublisherId = 2,
                        CreatedDateTime = DateTime.Now,
                        ChangedDateTime = DateTime.Now,
                        Title = memberOfCongress.name.official_full is null ? $"{memberOfCongress.name.first} {memberOfCongress.name.middle} {memberOfCongress.name.last} {memberOfCongress.name.suffix}".Replace("  ", " ") : memberOfCongress.name.official_full,
                        OwnerId = Constants.OWNER_PARTIES,
                        TenantNodes = new List<TenantNode> {
                            new TenantNode {
                                Id = null,
                                TenantId = 1,
                                PublicationStatusId = 1,
                                UrlPath = null,
                                NodeId = null,
                                SubgroupId = null,
                                UrlId = null
                            }
                        },
                        NodeTypeId = 24,
                        Description = "",
                        FileIdTileImage = null,
                        VocabularyNames = new List<VocabularyName>(),
                        DateOfBirth = memberOfCongress.bio.birthday,
                        DateOfDeath = null,
                        FileIdPortrait = null,
                        FirstName = memberOfCongress.name.first,
                        LastName = memberOfCongress.name.last,
                        MiddleName = memberOfCongress.name.middle,
                        FullName = memberOfCongress.name.official_full,
                        GovtrackId = memberOfCongress.id.govtrack,
                        Suffix = memberOfCongress.name.suffix,
                        ProfessionalRoles = professionalRoles,
                        Bioguide = memberOfCongress.id.bioguide,
                        PersonOrganizationRelations = new List<PersonOrganizationRelation>()
                    };
                }
            }
        }
    }

    private async IAsyncEnumerable<MemberOfCongress> GetMembersOfCongress()
    {


        string fileName = @"..\..\..\files\legislators-current.json";
        var jsonUtf8Bytes = await System.IO.File.ReadAllBytesAsync(fileName);
        foreach (var member in JsonSerializer.Deserialize<List<MemberOfCongress>>(jsonUtf8Bytes)!)
        {
            if (member.terms.Any(x => x.end >= DateTime.Parse("1999-01-03")))
            {
                yield return member;
            }
        }
        string fileName2 = @"..\..\..\files\legislators-historical.json";
        var jsonUtf8Bytes2 = await System.IO.File.ReadAllBytesAsync(fileName2);
        foreach (var member in JsonSerializer.Deserialize<List<MemberOfCongress>>(jsonUtf8Bytes2)!)
        {
            if (member.terms.Any(x => x.end >= DateTime.Parse("1999-01-03")))
            {
                yield return member;
            }
        }
    }

    private async IAsyncEnumerable<Model.File> GetImageFiles()
    {
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = """
            select
            p.id,
            p.bioguide
            from person p
            where p.file_id_portrait is null
            and p.bioguide is not null
            """;
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var personId = reader.GetInt32(0);
            var bioguid = reader.GetString(1);
            var fileNameSource = $"\\\\wsl.localhost\\Ubuntu\\home\\niels\\ppl\\files\\members_of_congress\\{reader.GetString(1)}.jpg";
            var file = new FileInfo(fileNameSource);
            if(file.Exists) 
            {
                yield return new Model.File {
                    Id = null,
                    Path = $"files/members_of_congress/{reader.GetString(1)}.jpg",
                    Name = $"{reader.GetString(1)}.jpg",
                    MimeType = "image/jpeg",
                    Size = (int)file.Length,
                    TenantFiles = new List<TenantFile>{
                    new TenantFile
                    {
                        TenantId = Constants.PPL,
                        FileId = null,
                        TenantFileId = null
                    },
                    new TenantFile
                    {
                        TenantId = Constants.CPCT,
                        FileId = null,
                        TenantFileId = null
                    }
                }
                };
            }
        }
    }

    private async Task UpdatePerson(IAsyncEnumerable<NodeFile> nodeFIles)
    {
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = """
            update person
            set file_id_portrait = @file_id_portrait
            where id = @id
            """;

        command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("file_id_portrait", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        await foreach (var item in nodeFIles) {

            command.Parameters["id"].Value = item.NodeId;
            command.Parameters["file_id_portrait"].Value = item.FileId;
            await command.ExecuteNonQueryAsync();
        }
    }

    private async IAsyncEnumerable<NodeFile> GetNodeFilesImage()
    {
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = """
            select
            p.id person_id,
            f.id file_id
            from person p
            join file f on f.path = 'files/members_of_congress/' || p.bioguide || '.jpg'
            where p.file_id_portrait is null
            and p.bioguide is not null
            """;
        using var reader = await command.ExecuteReaderAsync();

        
        while (await reader.ReadAsync()) {
            yield return new NodeFile { FileId = reader.GetInt32(1), NodeId = reader.GetInt32(0) };
        }
    }

    private async Task DownloadCongressionalImages()
    {
        using var httpClient = new HttpClient();
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = """
            select
            'd:\images\congress\ || p.bioguide ||'.jpg',
            from person p
            where p.file_id_portrait is null
            and p.bioguide is not null
            """;
        using var reader = await command.ExecuteReaderAsync();
        while(reader.Read()) {
            var uri = new Uri($"{reader.GetString(0)}");
            var fileName = $"D:\\images\\congress\\{reader.GetString(1)}";
            using (var response = await httpClient.GetAsync(uri)) {
                if (response.IsSuccessStatusCode) {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    var fileStream = System.IO.File.Create(fileName);

                    await using var inputStream = await response.Content.ReadAsStreamAsync();
                    inputStream.Seek(0, SeekOrigin.Begin);
                    inputStream.CopyTo(fileStream);
                    fileStream.Close();
                }
                else {
                    Console.WriteLine($"Missing: {uri}, reponse {response.StatusCode}");
                }
            }
            await Task.Delay(TimeSpan.FromMilliseconds(500));
        }
    }
}
