namespace PoundPupLegacy.Convert;

internal sealed class CountrySubdivisionTypeMigratorPartOne : CountrySubdivisionTypeMigrator
{
    public CountrySubdivisionTypeMigratorPartOne(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameFactory,
        IEntityCreator<CountrySubdivisionType> countrySubdivisionTypeCreator
    ) : base(databaseConnections, nodeIdReaderFactory, vocabularyIdReaderByOwnerAndNameFactory, termReaderByNameFactory, countrySubdivisionTypeCreator)
    {
    }

    protected override string FileName => "country_subdivision_types_part1.csv";
}
internal sealed class CountrySubdivisionTypeMigratorPartTwo : CountrySubdivisionTypeMigrator
{
    public CountrySubdivisionTypeMigratorPartTwo(IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameFactory,
        IEntityCreator<CountrySubdivisionType> countrySubdivisionTypeCreator
    ) : base(databaseConnections, nodeIdReaderFactory, vocabularyIdReaderByOwnerAndNameFactory, termReaderByNameFactory, countrySubdivisionTypeCreator)
    {
    }

    protected override string FileName => "country_subdivision_types_part2.csv";
}
internal sealed class CountrySubdivisionTypeMigratorPartThree : CountrySubdivisionTypeMigrator
{
    public CountrySubdivisionTypeMigratorPartThree(IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameFactory,
        IEntityCreator<CountrySubdivisionType> countrySubdivisionTypeCreator
    ) : base(databaseConnections, nodeIdReaderFactory, vocabularyIdReaderByOwnerAndNameFactory, termReaderByNameFactory, countrySubdivisionTypeCreator)
    {
    }

    protected override string FileName => "country_subdivision_types_part3.csv";
}

internal abstract class CountrySubdivisionTypeMigrator : MigratorPPL
{

    protected abstract string FileName { get; }
    protected override string Name => "basic second level subdivisions";

    private readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderByOwnerAndNameFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderByNameFactory;
    private readonly IEntityCreator<CountrySubdivisionType> _countrySubdivisionTypeCreator;

    public CountrySubdivisionTypeMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameFactory,
        IEntityCreator<CountrySubdivisionType> countrySubdivisionTypeCreator
    ) : base(databaseConnections)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _vocabularyIdReaderByOwnerAndNameFactory = vocabularyIdReaderByOwnerAndNameFactory;
        _termReaderByNameFactory = termReaderByNameFactory;
        _countrySubdivisionTypeCreator = countrySubdivisionTypeCreator;
    }

    private async IAsyncEnumerable<CountrySubdivisionType> ReadCountrySubdivisionTypesCsv(
        NodeIdReaderByUrlId nodeIdReader,
        VocabularyIdReaderByOwnerAndName vocabularyIdReader,
        TermReaderByName termReaderByName
        )
    {

        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@$"..\..\..\files\{FileName}").Skip(1)) {
            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int id = int.Parse(parts[0]);
            var name = parts[1];
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[0])
            });
            var subdivisionType = await termReaderByName.ReadAsync(new TermReaderByName.Request {
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
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var vocabularyIdReader = await _vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await _termReaderByNameFactory.CreateAsync(_postgresConnection);

        await _countrySubdivisionTypeCreator.CreateAsync(ReadCountrySubdivisionTypesCsv(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName
            ), _postgresConnection);

    }
}
