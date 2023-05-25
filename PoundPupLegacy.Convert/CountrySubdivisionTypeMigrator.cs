namespace PoundPupLegacy.Convert;

internal sealed class CountrySubdivisionTypeMigratorPartOne(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
    IEntityCreatorFactory<CountrySubdivisionType> countrySubdivisionTypeCreatorFactory
) : CountrySubdivisionTypeMigrator(
    databaseConnections, 
    nodeIdReaderFactory, 
    vocabularyIdReaderByOwnerAndNameFactory, 
    termReaderByNameFactory, 
    countrySubdivisionTypeCreatorFactory
)
{
    protected override string FileName => "country_subdivision_types_part1.csv";
}
internal sealed class CountrySubdivisionTypeMigratorPartTwo(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
    IEntityCreatorFactory<CountrySubdivisionType> countrySubdivisionTypeCreatorFactory
) : CountrySubdivisionTypeMigrator(
    databaseConnections, 
    nodeIdReaderFactory, 
    vocabularyIdReaderByOwnerAndNameFactory, 
    termReaderByNameFactory, 
    countrySubdivisionTypeCreatorFactory
)
{
    protected override string FileName => "country_subdivision_types_part2.csv";
}
internal sealed class CountrySubdivisionTypeMigratorPartThree : CountrySubdivisionTypeMigrator
{
    public CountrySubdivisionTypeMigratorPartThree(IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
        IEntityCreatorFactory<CountrySubdivisionType> countrySubdivisionTypeCreatorFactory
    ) : base(databaseConnections, nodeIdReaderFactory, vocabularyIdReaderByOwnerAndNameFactory, termReaderByNameFactory, countrySubdivisionTypeCreatorFactory)
    {
    }

    protected override string FileName => "country_subdivision_types_part3.csv";
}

internal abstract class CountrySubdivisionTypeMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
        IEntityCreatorFactory<CountrySubdivisionType> countrySubdivisionTypeCreatorFactory
    ) : MigratorPPL(databaseConnections)
{

    protected abstract string FileName { get; }
    protected override string Name => "basic second level subdivisions";

    private async IAsyncEnumerable<CountrySubdivisionType> ReadCountrySubdivisionTypesCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReader
        )
    {

        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@$"..\..\..\files\{FileName}").Skip(1)) {
            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int id = int.Parse(parts[0]);
            var name = parts[1];
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[0])
            });
            var subdivisionType = await termReader.ReadAsync(new TermReaderByNameRequest {
                VocabularyId = vocabularyId,
                Name = name
            });
            if (subdivisionType is null) {
                Console.WriteLine(name);
            }
            yield return new CountrySubdivisionType {
                CountryId = countryId,
                SubdivisionTypeId = subdivisionType!.NameableId,
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var vocabularyIdReader = await vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);

        await using var countrySubdivisionTypeCreator = await countrySubdivisionTypeCreatorFactory.CreateAsync(_postgresConnection);
        await countrySubdivisionTypeCreator.CreateAsync(ReadCountrySubdivisionTypesCsv(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName
        ));
    }
}
