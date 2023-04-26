namespace PoundPupLegacy.Convert;

internal sealed class PersonMigratorPPL : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> _fileIdReaderByTenantFileIdFactory;
    private readonly IEntityCreator<Person> _personCreator;
    public PersonMigratorPPL(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
        IEntityCreator<Person> personCreator
    ) : base(databaseConnections)
    {
        _fileIdReaderByTenantFileIdFactory = fileIdReaderByTenantFileIdFactory;
        _personCreator = personCreator;
    }

    protected override string Name => "persons (ppl)";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReader = await _fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await _personCreator.CreateAsync(ReadPersons(fileIdReader), _postgresConnection);
    }
    private static DateTime? GetDateOfDeath(int id, DateTime? dateTime)
    {
        return id switch {
            60412 => DateTime.Parse("2022-03-18"),
            10329 => DateTime.Parse("2018-08-25"),
            _ => dateTime
        };
    }
    private static DateTime? GetDateOfBirth(int id, DateTime? dateTime)
    {
        return id switch {
            10342 => DateTime.Parse("1958-01-26"),
            10732 => DateTime.Parse("1954-05-29"),
            10743 => DateTime.Parse("1941-07-18"),
            38317 => DateTime.Parse("1933-05-31"),
            38337 => DateTime.Parse("1943-08-15"),
            38614 => DateTime.Parse("1967-03-11"),
            39136 => DateTime.Parse("1929-02-09"),
            39202 => DateTime.Parse("1952-07-16"),
            39211 => DateTime.Parse("1948-05-20"),
            39248 => DateTime.Parse("1935-06-01"),
            39276 => DateTime.Parse("1948-07-13"),
            61293 => DateTime.Parse("1947-10-26"),
            39152 => DateTime.Parse("1932-01-28"),
            38322 => DateTime.Parse("1953-07-07"),
            62138 => DateTime.Parse("1963-10-19"),
            61467 => DateTime.Parse("1953-01-22"),
            62481 => DateTime.Parse("1958-12-03"),
            62517 => DateTime.Parse("1961-01-10"),
            62897 => DateTime.Parse("1957-12-10"),
            60392 => DateTime.Parse("1948-07-23"),
            10813 => DateTime.Parse("1934-07-16"),
            62393 => DateTime.Parse("1973-11-27"),
            59375 => DateTime.Parse("1958-05-30"),
            61798 => DateTime.Parse("1942-04-25"),
            61966 => DateTime.Parse("1958-11-22"),
            62011 => DateTime.Parse("1973-10-01"),
            62026 => DateTime.Parse("1972-08-25"),
            63210 => DateTime.Parse("1958-07-12"),
            74198 => DateTime.Parse("1966-03-22"),
            74201 => DateTime.Parse("1956-12-05"),
            74204 => DateTime.Parse("1967-11-18"),
            74207 => DateTime.Parse("1962-05-14"),
            74210 => DateTime.Parse("1952-03-31"),
            74213 => DateTime.Parse("1966-12-17"),
            74219 => DateTime.Parse("1969-03-29"),
            74222 => DateTime.Parse("1965-04-04"),
            38973 => DateTime.Parse("1942-10-15"),
            39119 => DateTime.Parse("1947-07-22"),
            39177 => DateTime.Parse("1957-10-11"),
            39232 => DateTime.Parse("1935-01-05"),
            51380 => DateTime.Parse("1976-07-27"),
            60412 => DateTime.Parse("1933-06-09"),
            60488 => DateTime.Parse("1973-08-03"),
            61082 => DateTime.Parse("1941-01-23"),
            61665 => DateTime.Parse("1963-08-17"),
            62556 => DateTime.Parse("1971-11-30"),
            63139 => DateTime.Parse("1936-11-29"),
            62570 => DateTime.Parse("1973-02-07"),
            62129 => DateTime.Parse("1948-05-16"),
            62983 => DateTime.Parse("1956-08-15"),
            62141 => DateTime.Parse("1954-10-02"),
            63244 => DateTime.Parse("1963-04-09"),
            74231 => DateTime.Parse("1963-01-31"),
            74234 => DateTime.Parse("1957-09-06"),
            74238 => DateTime.Parse("1960-04-22"),
            74241 => DateTime.Parse("1963-12-22"),
            74244 => DateTime.Parse("1951-11-07"),
            64530 => DateTime.Parse("1930-04-14"),
            62736 => DateTime.Parse("1934-10-21"),
            62051 => DateTime.Parse("1957-07-29"),
            74225 => DateTime.Parse("1959-02-16"),
            62643 => DateTime.Parse("1969-06-16"),
            62931 => DateTime.Parse("1974-09-09"),
            62934 => DateTime.Parse("1974-10-09"),
            65781 => DateTime.Parse("1946-04-15"),
            74263 => DateTime.Parse("1954-09-16"),
            74269 => DateTime.Parse("1953-11-01"),
            74275 => DateTime.Parse("1961-05-08"),
            74278 => DateTime.Parse("1967-03-18"),
            74284 => DateTime.Parse("1953-11-23"),
            74288 => DateTime.Parse("1954-10-18"),
            62865 => DateTime.Parse("1958-11-26"),
            74247 => DateTime.Parse("1967-07-01"),
            74250 => DateTime.Parse("1969-06-23"),
            74254 => DateTime.Parse("1960-12-30"),
            74257 => DateTime.Parse("1955-04-26"),
            74260 => DateTime.Parse("1968-05-11"),
            74359 => DateTime.Parse("1950-06-20"),
            74368 => DateTime.Parse("1971-06-05"),
            62776 => DateTime.Parse("1970-09-26"),
            62872 => DateTime.Parse("1931-12-20"),
            63075 => DateTime.Parse("1951-03-20"),
            74291 => DateTime.Parse("1961-03-03"),
            62396 => DateTime.Parse("1958-12-17"),
            61452 => DateTime.Parse("1940-07-03"),
            61540 => DateTime.Parse("1971-10-17"),
            61553 => DateTime.Parse("1971-06-04"),
            61750 => DateTime.Parse("1972-10-20"),
            61830 => DateTime.Parse("1974-02-27"),
            62323 => DateTime.Parse("1937-07-16"),
            64421 => DateTime.Parse("1969-04-27"),
            74152 => DateTime.Parse("1964-11-13"),
            74157 => DateTime.Parse("1949-12-10"),
            74172 => DateTime.Parse("1960-08-30"),
            74176 => DateTime.Parse("1954-10-24"),
            74192 => DateTime.Parse("1954-05-14"),
            _ => dateTime
        };
    }

    private async IAsyncEnumerable<Person> ReadPersons(
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId
    )
    {

        var sql = $"""
                SELECT
                n.nid id,
                n.uid access_role_id,
                case 
                    when n.title = 'Cory Brooker' then 'Cory Booker'
                    else n.title
                end title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                24 node_type_id,
                case 
                	when c.title IS NOT NULL then c.title
                	ELSE c2.title
                 	end topic_name,
                CASE WHEN o.field_image_fid = 0 THEN null ELSE o.field_image_fid END file_id_portrait,
                STR_TO_DATE(field_born_value,'%Y-%m-%d') date_of_birth,
                STR_TO_DATE(field_died_value,'%Y-%m-%d') date_of_death,
                ua.dst url_path
                FROM node n 
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                LEFT JOIN content_type_adopt_person o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` = 'category_cat'
                LEFT JOIN (
                	select
                	n.nid,
                	n.title,
                	cc.field_tile_image_title,
                	cc.field_related_page_nid,
                	p.nid parent_id,
                	p.title parent_name
                	FROM node n
                	JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                	LEFT JOIN (
                	    SELECT
                	    n.nid, 
                	    n.title,
                	    ch.cid
                	    FROM node n
                	    JOIN category_hierarchy ch ON ch.parent = n.nid
                	    WHERE n.`type` = 'category_cat'
                	) p ON p.cid = n.nid
                ) c ON c.field_related_page_nid = n.nid
                LEFT JOIN(
                    select
                    DISTINCT n.title
                    FROM node n
                    JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                    GROUP BY n.title
                ) c2 ON c2.title = n.title
                WHERE n.`type` = 'adopt_person'
                AND n.nid not in (45656, 74250, 26419)
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var title = id switch {
                61991 => "Paul Cook (representative)",
                _ => reader.GetString("title")
            };
            var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");
            var vocabularyNames = new List<VocabularyName> {
                new VocabularyName {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = title,
                    ParentNames = new List<string>(),
                }
            };

            yield return new Person {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = reader.GetInt16("node_type_id"),
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
                DateOfBirth = GetDateOfBirth(reader.GetInt32("id"), reader.IsDBNull("date_of_birth") ? null : reader.GetDateTime("date_of_birth")),
                DateOfDeath = GetDateOfDeath(reader.GetInt32("id"), reader.IsDBNull("date_of_death") ? null : reader.GetDateTime("date_of_death")),
                FileIdPortrait = reader.IsDBNull("file_id_portrait")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                        TenantId = Constants.PPL,
                        TenantFileId = reader.GetInt32("file_id_portrait")
                    }),
                FirstName = null,
                LastName = null,
                MiddleName = null,
                FullName = null,
                GovtrackId = null,
                Bioguide = null,
                Suffix = null,
                ProfessionalRoles = new List<ProfessionalRole>(),
                PersonOrganizationRelations = new List<PersonOrganizationRelation>()
            };

        }
        await reader.CloseAsync();
    }


}
