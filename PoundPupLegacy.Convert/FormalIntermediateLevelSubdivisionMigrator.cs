using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Convert;

internal sealed class FormalIntermediateLevelSubdivisionMigrator : Migrator
{
    protected override string Name => "formal intermediate level subdivisions";

    public FormalIntermediateLevelSubdivisionMigrator(MySqlToPostgresConverter converter) : base(converter) { }    
    private async IAsyncEnumerable<FormalIntermediateLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv()
    {
        await using var vocabularyReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(_postgresConnection);
        await using var termReader = await TermReaderByName.CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(Constants.OWNER_GEOGRAPHY, "Subdivision type");

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\FormalIntermediateLevelSubdivisions.csv").Skip(1))
        {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0? null: int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await _nodeIdReader.ReadAsync(Constants.PPL, int.Parse(parts[7]));
            var countryName = (await _termReaderByNameableId.ReadAsync(Constants.PPL, Constants.VOCABULARY_TOPICS, countryId)).Name;
            yield return new FormalIntermediateLevelSubdivision
            {
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
                        TenantId = 1,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                PublisherId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = parts[9],
                ISO3166_2_Code = parts[10],
                FileIdFlag = null,
                SubdivisionTypeId = (await termReader.ReadAsync(vocabularyId, parts[11].Trim())).NameableId
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await FormalIntermediateLevelSubdivisionCreator.CreateAsync(ReadFormalIntermediateLevelSubdivisionCsv(), _postgresConnection);
    }
}
