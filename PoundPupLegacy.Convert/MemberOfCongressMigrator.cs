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

internal class MemberOfCongressMigrator : PPLMigrator
{

    private List<MemberOfCongress> _membersOfCongress = new List<MemberOfCongress>();

    public MemberOfCongressMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "members of congress";

    protected override async Task MigrateImpl()
    {
        _membersOfCongress = await GetMembersOfCongress().ToListAsync();

        await PersonCreator.CreateAsync(GetMembersOfCongressAsync(), _postgresConnection);
    }

    private async IAsyncEnumerable<TempTerm> GetTerms()
    {
        await foreach (var term in GetTerms(MemberType.Representative))
        {
            yield return term;

        }
        await foreach (var term in GetTerms(MemberType.Senator))
        {
            yield return term;

        }
    }
    private async IAsyncEnumerable<TempTerm> GetTerms(MemberType memberType)
    {
        var t = memberType == MemberType.Representative ? "rep" : "sen";
        foreach (var member in _membersOfCongress.Where(x => x.terms.Any(y => y.start >= DateTime.Parse("1999-01-03"))))
        {
            var terms = member.terms.Where(x => x.type == t).OrderBy(x => x.start);
            TempTerm? tempTerm = null;
            DateTime? endPrevious = null;
            foreach (var term in terms)
            {
                if (tempTerm is null)
                {
                    tempTerm = new TempTerm
                    {
                        Id = member.id.govtrack,
                        StartDate = term.start,
                        EndDate = null,
                        MemberType = memberType,
                        State = term.state,
                    };
                }
                if (endPrevious is not null)
                {
                    if (term.start.Subtract(endPrevious.Value) < TimeSpan.FromDays(30))
                    {
                        if (term.end <= DateTime.Parse("2023-01-03"))
                        {
                            endPrevious = term.end;
                        }
                        else
                        {
                            endPrevious = null;
                        }
                    }
                    else
                    {
                        tempTerm.EndDate = endPrevious;
                        yield return tempTerm;
                        tempTerm = new TempTerm
                        {
                            Id = member.id.govtrack,
                            StartDate = term.start,
                            EndDate = null,
                            MemberType = memberType,
                            State = term.state,
                        };
                        endPrevious = term.end;
                    }
                }
                else if (term.end <= DateTime.Parse("2023-01-03"))
                {
                    endPrevious = term.end;
                }

            }
            if (tempTerm is not null)
            {
                tempTerm.EndDate = endPrevious;
                yield return tempTerm;
            }
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
        const string updateSql = """
            UPDATE person SET 
                first_name = @first_name, 
                last_name = @last_name, 
                middle_name = @middle_name, 
                nick_name = @nick_name, 
                suffix = @suffix, 
                full_name = @full_name, 
                govtrack_id = @govtrack_id
                WHERE id = @id
            """;
        const string readSql = """
            SELECT
            n.id,
            n.title,
            p.date_of_birth
            FROM node n
            join person p on p.id = n.id
            where ((n.title like @first_name and n.title like @last_name) or n.title = @wikiname or n.title = @ballotpedia or n.title = @wikipedia2) and p.date_of_birth = @date_of_birth
            and n.id in (
                select
                    p.party_id
                from party_political_entity_relation p
                join tenant_node tn on tn.node_id = p.party_political_entity_relation_type_id
                where tn.url_id in (12660, 12662)
            )
            """;



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
                    while (await reader.ReadAsync())
                    {
                        memberOfCongress.node_id = reader.GetInt32("id");
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
            updateCommand.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            await updateCommand.PrepareAsync();
            foreach (var memberOfCongress in _membersOfCongress)
            {
                if (memberOfCongress.node_id.HasValue)
                {
                    updateCommand.Parameters["first_name"].Value = memberOfCongress.name.first is null ? DBNull.Value : memberOfCongress.name.first;
                    updateCommand.Parameters["last_name"].Value = memberOfCongress.name.last is null ? DBNull.Value : memberOfCongress.name.last;
                    updateCommand.Parameters["middle_name"].Value = memberOfCongress.name.middle is null ? DBNull.Value : memberOfCongress.name.middle;
                    updateCommand.Parameters["full_name"].Value = memberOfCongress.name.official_full is null ? DBNull.Value : memberOfCongress.name.official_full;
                    updateCommand.Parameters["suffix"].Value = memberOfCongress.name.suffix is null ? DBNull.Value : memberOfCongress.name.suffix;
                    updateCommand.Parameters["nick_name"].Value = memberOfCongress.name.nickname is null ? DBNull.Value : memberOfCongress.name.nickname;
                    updateCommand.Parameters["govtrack_id"].Value = memberOfCongress.id.govtrack;
                    updateCommand.Parameters["id"].Value = memberOfCongress.node_id;
                    await updateCommand.ExecuteNonQueryAsync();
                }
                else if (memberOfCongress.terms.Any(x => x.end > DateTime.Parse("1999-03-01")))
                {
                    yield return new Person
                    {
                        Id = null,
                        PublisherId = 2,
                        CreatedDateTime = DateTime.Now,
                        ChangedDateTime = DateTime.Now,
                        Title = memberOfCongress.name.official_full is null ? $"{memberOfCongress.name.first} {memberOfCongress.name.middle} {memberOfCongress.name.last} {memberOfCongress.name.suffix}".Replace("  ", " ") : memberOfCongress.name.official_full,
                        OwnerId = Constants.OWNER_PARTIES,
                        TenantNodes = new List<TenantNode>
                        {
                            new TenantNode
                            {
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
            yield return member;
        }
        string fileName2 = @"..\..\..\files\legislators-historical.json";
        var jsonUtf8Bytes2 = await System.IO.File.ReadAllBytesAsync(fileName2);
        foreach (var member in JsonSerializer.Deserialize<List<MemberOfCongress>>(jsonUtf8Bytes2)!)
        {
            yield return member;
        }
    }

}
