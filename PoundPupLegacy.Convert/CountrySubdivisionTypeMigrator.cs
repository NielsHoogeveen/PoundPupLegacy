using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class CountrySubdivisionTypeMigratorPartOne: CountrySubdivisionTypeMigrator
{
    public CountrySubdivisionTypeMigratorPartOne(MySqlToPostgresConverter converter): base(converter) { }

    protected override string FileName => "country_subdivision_types_part1.csv";
}
internal sealed class CountrySubdivisionTypeMigratorPartTwo : CountrySubdivisionTypeMigrator
{
    public CountrySubdivisionTypeMigratorPartTwo(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string FileName => "country_subdivision_types_part2.csv";
}
internal sealed class CountrySubdivisionTypeMigratorPartThree : CountrySubdivisionTypeMigrator
{
public CountrySubdivisionTypeMigratorPartThree(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string FileName => "country_subdivision_types_part3.csv";
}

internal abstract class CountrySubdivisionTypeMigrator : Migrator
{

    protected abstract string FileName { get; }
    protected override string Name => "basic second level subdivisions";

    public CountrySubdivisionTypeMigrator(MySqlToPostgresConverter converter): base(converter) { } 

    private async IAsyncEnumerable<CountrySubdivisionType> ReadCountrySubdivisionTypesCsv()
    {
        await using var vocabularyReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(_postgresConnection);
        await using var termReader = await TermReaderByName.CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(Constants.OWNER_GEOGRAPHY, "Subdivision type");

        await foreach (string line in System.IO.File.ReadLinesAsync(@$"..\..\..\files\{FileName}").Skip(1))
        {
            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int id = int.Parse(parts[0]);
            var name = parts[1];
            var countryId = await _nodeIdReader.ReadAsync(Constants.PPL, int.Parse(parts[0]));
            var subdivisionType = await termReader.ReadAsync(vocabularyId, name);
            if(subdivisionType is null)
            {
                Console.WriteLine(name);
            }
            yield return new CountrySubdivisionType
            {
                CountryId = countryId,
                SubdivisionTypeId = subdivisionType!.NameableId,
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await CountrySubdivisionTypeCreator.CreateAsync(ReadCountrySubdivisionTypesCsv(), _postgresConnection);

    }
}
