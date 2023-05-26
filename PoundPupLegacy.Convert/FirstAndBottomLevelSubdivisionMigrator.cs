using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class FirstAndBottomLevelSubdivisionMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, ImmediatelyIdentifiableTerm> termReaderByNameableIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, ImmediatelyIdentifiableTerm> termReaderByNameFactory,
        IEntityCreatorFactory<EventuallyIdentifiableFirstAndBottomLevelSubdivision> firstAndBottomLevelSubdivisionCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "first and bottom level subdivisions";
    private async IAsyncEnumerable<NewFirstAndBottomLevelSubdivision> ReadDirectSubDivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        ISingleItemDatabaseReader<TermReaderByNameableIdRequest, ImmediatelyIdentifiableTerm> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, ImmediatelyIdentifiableTerm> termReaderByName
        )
    {

        var vocabularyIdSubdivision = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\direct_subdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            var title = parts[8];
            var name = parts[9];
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[7])
            });
            var countryName = (await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                NameableId = countryId,
                VocabularyId = vocabularyIdTopics
            }))!.Name;
            yield return new NewFirstAndBottomLevelSubdivision {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                Description = "",
                Terms = new List<NewTermForNewNameble>
                {
                    new NewTermForNewNameble
                    {
                        VocabularyId = vocabularyIdTopics,
                        Name = title,
                        ParentTermIds = new List<int> {
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = countryName ,
                                VocabularyId = vocabularyIdTopics
                            })
                        },
                    }
                },
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                PublisherId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = name,
                ISO3166_2_Code = parts[10],
                FileIdFlag = null,
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
                    VocabularyId = vocabularyIdSubdivision,
                    Name = parts[11].Trim()
                })).NameableId,
                NodeTermIds = new List<int>(),
            };
        }
    }

    private static string GetISO3166Code2(int id, string code)
    {
        return id switch {
            11568 => "GB-NBL",
            11577 => "GB-IOW",
            11598 => "GB-LIV",
            11601 => "GB-BST",
            11610 => "GB-YOR",
            11614 => "GB-BKM",
            11616 => "GB-BDF",
            11617 => "GB-LND",
            11618 => "GB-BIR",
            11620 => "GB-WOR",
            11623 => "GB-KEN",
            11628 => "GB-SFK",
            11635 => "GB-WLV",
            11637 => "GB-MDW",
            11639 => "GB-LAN",
            11787 => "GB-OXF",
            11788 => "GB-ESS",
            11789 => "GB-CRF",
            11790 => "GB-DEV",
            11791 => "GB-SRY",
            11792 => "GB-DUR",
            11793 => "GB-SHF",
            11794 => "GB-DOR",
            11795 => "GB-STS",
            11806 => "GB-GLS",
            11807 => "GB-WIL",
            11808 => "GB-ERY",
            11809 => "GB-WSX",
            11810 => "GB-MAN",
            11811 => "GB-DBY",
            11812 => "GB-LCE",
            11813 => "GB-HAM",
            11814 => "GB-CMA",
            11815 => "GB-CHE",
            11816 => "GB-KHL",
            11817 => "GB-BRD",
            11818 => "GB-ESX",
            11819 => "GB-LIN",
            11820 => "GB-DNC",
            11821 => "GB-CAM",
            11822 => "GB-NTT",
            11823 => "GB-HRT",
            11824 => "GB-NYK",
            11825 => "GB-NFK",
            11826 => "GB-WAR",
            11827 => "GB-VGL",
            11828 => "GB-PTE",
            11829 => "GB-BGW",
            11830 => "GB-WBK",
            11831 => "GB-CHW",
            11832 => "GB-SOM",
            11834 => "GB-SHR",
            11835 => "GB-SKP",
            11836 => "GB-RUT",
            11837 => "GB-SWA",
            45209 => "VN-HN",
            45263 => "VN-21",
            45270 => "VN-SG",
            45218 => "VN-CT",
            45320 => "VN-DN",

            58068 => "CN-AH",
            58075 => "CN-BJ",
            58092 => "CN-CQ",
            58069 => "CN-FJ",
            58096 => "CN-GS",
            58073 => "CN-GD",
            58067 => "CN-GX",
            58066 => "CN-GZ",
            58090 => "CN-HI",
            58065 => "CN-HE",
            58084 => "CN-HL",
            58064 => "CN-HA",
            58071 => "CN-HB",
            58072 => "CN-HN",
            58079 => "CN-NM",
            58063 => "CN-JS",
            58059 => "CN-JX",
            58082 => "CN-JL",
            58080 => "CN-LN",
            58100 => "CN-NX",
            58098 => "CN-QH",
            58074 => "CN-SN",
            58070 => "CN-SD",
            58086 => "CN-SH",
            58057 => "CN-SX",
            58053 => "CN-SC",
            58077 => "CN-TJ",
            58094 => "CN-XZ",
            58102 => "CN-XJ",
            58060 => "CN-YN",
            58088 => "CN-ZJ",
            _ => code
        };
    }

    private static string GetSubdivisionTitle(int id, string name, string code)
    {
        var adjustedName = GetSubdivisionName(id, name, code);
        if (code.StartsWith("AU")) {
            if (id == 58386 || id == 58393) {
                return $"{adjustedName} (Australian territory)";
            }
            return $"{adjustedName} (Australian state)";
        }
        if (code.StartsWith("CA")) {
            if (id == 57988 || id == 57989 || id == 57990) {
                return $"{adjustedName} (Canadian territory)";
            }
            return $"{adjustedName} (Canadian state)";
        }
        if (code.StartsWith("CN")) {
            if (id == 58079) {
                return "Nei Mongol (Chinese autonomous region)";
            }
            if (id == 58067) {
                return "Guangxi Zhuang (Chinese autonomous region)";
            }
            if (id == 58094) {
                return "Xizang (Chinese autonomous region)";
            }
            if (id == 58094) {
                return "Ningxia Hui (Chinese autonomous region)";
            }
            if (id == 58102) {
                return "Xinjiang Uyghur (Chinese autonomous region)";
            }
            if (name.ToLower().Contains(" province")) {
                return $"{adjustedName} (Chinese province)";
            }
            if (name.ToLower().Contains(" municipality")) {
                return $"{adjustedName} (Chinese municipality)";
            }
        }
        if (code.StartsWith("IN")) {
            return $"{adjustedName} (Indian state)";
        }
        if (code.StartsWith("RU")) {
            if (code == "RU-YEV") {
                return $"{adjustedName} (Russian autonomous region)";
            }
            if (code == "RU-CHU" || code == "RU-KHM" || code == "RU-NEN" || code == "RU-YAN") {
                return $"{adjustedName} (Russian autonomous district)";
            }
            if (name.ToLower().Contains(" oblast") || name.ToLower().Contains(" krai")) {
                return $"{adjustedName} (Russian administrative territory)";
            }
            if (name.ToLower().Contains(" republic")) {
                return $"{adjustedName} (Russian republic)";
            }

        }
        if (code.StartsWith("VN")) {
            if (id == 45218 || id == 45320 || id == 45209 || id == 45270) {
                return $"{adjustedName} (Vietnamese municipality)";
            }
            else {
                return $"{adjustedName} (Vietnamese province)";
            }
        }
        return name;
    }
    private static string GetSubdivisionName(int id, string name, string code)
    {
        if (code.StartsWith("CN")) {
            if (id == 58079) {
                return "Nei Mongol";
            }
            if (id == 58067) {
                return "Guangxi Zhuang";
            }
            if (id == 58094) {
                return "Xizang";
            }
            if (id == 58094) {
                return "Ningxia Hui";
            }
            if (id == 58102) {
                return "Xinjiang Uyghur";
            }
            return name.Replace(" Province", "").Replace(" province", "").Replace(" municipality", "").Replace(" Municipality", "");
        }
        return id switch {
            11830 => "West Berkshire",
            11831 => "Cheshire West and Chester",
            11815 => "Cheshire East",
            11829 => "Blaenau Gwent",
            11816 => "Kingston upon Hull",
            11819 => "Lincolnshire",
            11827 => "Vale of Glamorgan",
            11637 => "Medway",
            11835 => "Stockport",
            11837 => "Swansea",
            11817 => "Bradford",

            58156 => "Bắc Kạn",
            45394 => "Yên Bái",
            45391 => "Vĩnh Phúc",
            45386 => "Vĩnh Long",
            45384 => "Tuyên Quang",
            45378 => "Thái Bình",
            45374 => "Sóc Trăng",
            45371 => "Quảng Ngãi",
            45369 => "Quảng Bình",
            45366 => "Ninh Thuận",
            45363 => "Nghệ An",
            45360 => "Lào Cai",
            45358 => "Lạng Sơn",
            45356 => "Lâm Đồng",
            45353 => "Khánh Hòa",
            45350 => "Hưng Yên",
            45347 => "Hậu Giang",
            45343 => "Hòa Bình",
            45340 => "Hải Dương",
            45337 => "Hà Tĩnh",
            45334 => "Hà Nam",
            45332 => "Đồng Tháp",
            45329 => "Đồng Nai",
            45320 => "Đà Nẵng",
            45317 => "Cao Bằng",
            45312 => "Cà Mau",
            45301 => "Bình Dương",
            45297 => "Bắc Ninh",
            45294 => "Bắc Giang",
            45291 => "Bà Rịa - Vũng Tàu",
            45288 => "Bến Tre",
            45283 => "Kiến Giang",
            45270 => "Hồ Chí Minh",
            45267 => "Ninh Bình",
            45263 => "Thanh Hóa",
            45260 => "Thừa Thiên-Huế",
            45257 => "Bình Thuận",
            45250 => "Quảng Nam",
            45246 => "An Giang",
            45244 => "Hà Tây",
            45222 => "Phú Thọ",
            45218 => "Cần Thơ",
            45213 => "Quảng Ninh",
            45209 => "Hà Nội",
            45205 => "Nam Định",
            45203 => "Thái Nguyên",
            _ => name
        };
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);

        await using var firstAndBottomLevelSubdivisionCreator = await firstAndBottomLevelSubdivisionCreatorFactory.CreateAsync(_postgresConnection);
        await firstAndBottomLevelSubdivisionCreator.CreateAsync(ReadDirectSubDivisionCsv(
            nodeIdReader,
            termIdReader,
            termReaderByNameableId,
            termReaderByName
        ));
        await firstAndBottomLevelSubdivisionCreator.CreateAsync(ReadFormalFirstLevelSubdivisions(
            nodeIdReader,
            termIdReader,
            termReaderByName
        ));
    }
    private async IAsyncEnumerable<NewFirstAndBottomLevelSubdivision> ReadFormalFirstLevelSubdivisions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, ImmediatelyIdentifiableTerm> termReaderByName
        )
    {

        var vocabularyIdSubdisionTypes = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        var continentIds = new List<int> { 3806, 3810, 3811, 3816, 3822, 3823 };

        var sql = $"""
             SELECT
            n.nid id,
            n.uid access_role_id,
            n.title,
            n.`status` node_status_id,
            FROM_UNIXTIME(n.created) created_date_time, 
            FROM_UNIXTIME(n.changed) changed_date_time,
            n2.nid country_id,
            case 
                when n2.title = 'Russian Federation' then 'Russia'
                else n2.title
            end country_name,
            n3.title topic_name,
            s.field_statecode_value iso_3166_2_code,
            ua.dst url_path,
            case 
                when n3.title LIKE '%Autonomous region%' then 'Autonomous region'
                when n3.title LIKE '%Province%' then 'Province'
                when n3.title LIKE '%province' then 'Province'
            		when n3.title LIKE '%Municipality%' then 'Municipality'
            		when n3.title LIKE '%Republic%' then 'Republic'
            		when n3.title LIKE '%Oblast%' then 'Administrative region'
            		when n3.title LIKE '%Krai%' then 'Administrative region'
            		when n3.title LIKE '%Okrug%' then 'Autonomous district'
            		when n3.title LIKE '%Territory%' then 'Territory'
            		when n.nid IN (58303, 58305) then 'Autonomous city'
            		when n.nid IN (45270) then 'Municipality'
            		when n.nid IN (57989) then 'Territory'
            		when n2.nid = 3885 then 'Province'
            		when n2.nid = 4038 then 'State'
            		when n.nid IN (58121) then 'Union territory'
            		when n2.nid = 3959 then 'State'
            	END subdivision_type_name
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid
            JOIN node n3 ON n3.nid = cc.nid AND n3.vid = cc.vid
            JOIN content_type_statefact s ON s.nid = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'statefact'
            AND n2.`type` = 'country_type'
            AND s.field_statecode_value IS NOT NULL
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync()) {
            var isoCode = reader.IsDBNull("iso_3166_2_code") ? GetISO3166Code2(reader.GetInt32("id"), "") :
                                GetISO3166Code2(reader.GetInt32("id"), reader.GetString("iso_3166_2_code"));
            var id = reader.GetInt32("id");
            var name = GetSubdivisionName(reader.GetInt32("id"), reader.GetString("title"), isoCode);

            var topicName = reader.GetString("topic_name");
            var countryName = reader.GetString("country_name");
            var vocabularyNames = new List<NewTermForNewNameble>
            {
                new NewTermForNewNameble
                {
                    VocabularyId = vocabularyIdTopics,
                    Name = topicName,
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = countryName ,
                            VocabularyId = vocabularyIdTopics
                        })
                    },
                }
            };

            yield return new NewFirstAndBottomLevelSubdivision {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                Name = name,
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 17,
                Terms = vocabularyNames,
                Description = "",
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("id") == 11827 ? 11571 :
                            reader.GetInt32("id") == 11789 ? 11571 :
                            reader.GetInt32("country_id")
                }),
                ISO3166_2_Code = isoCode,
                FileIdFlag = null,
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
                    VocabularyId = vocabularyIdSubdisionTypes,
                    Name = reader.GetString("subdivision_type_name")
                })).NameableId,
                NodeTermIds = new List<int>(),
            };

        }
        await reader.CloseAsync();
    }
}
