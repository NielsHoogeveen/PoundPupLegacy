namespace PoundPupLegacy.Convert;

internal sealed class FormalIntermediateLevelSubdivisionMigrator : PPLMigrator
{
    protected override string Name => "formal intermediate level subdivisions";

    public FormalIntermediateLevelSubdivisionMigrator(MySqlToPostgresConverter converter) : base(converter) { }
    private async IAsyncEnumerable<FormalIntermediateLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv()
    {
        await using var vocabularyReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(_postgresConnection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\FormalIntermediateLevelSubdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[7])
            });
            var countryName = (await _termReaderByNameableId.ReadAsync(new TermReaderByNameableId.Request {
                OwnerId = Constants.PPL,
                VocabularyName = Constants.VOCABULARY_TOPICS,
                NameableId = countryId
            })).Name;
            yield return new FormalIntermediateLevelSubdivision {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { countryName },
                    }
                },
                Description = "",
                FileIdTileImage = null,
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
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
                PublisherId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = parts[9],
                ISO3166_2_Code = parts[10],
                FileIdFlag = null,
                SubdivisionTypeId = (await termReader.ReadAsync(new TermReaderByName.Request {
                    VocabularyId = vocabularyId,
                    Name = parts[11].Trim()
                })).NameableId
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await FormalIntermediateLevelSubdivisionCreator.CreateAsync(ReadFormalIntermediateLevelSubdivisionCsv(), _postgresConnection);
    }
}
