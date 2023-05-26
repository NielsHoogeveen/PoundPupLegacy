using PoundPupLegacy.Common.Test;
using PoundPupLegacy.CreateModel;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace PoundPupLegacy.Db.Test;



public class TestDb
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TestDb(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator(_testOutputHelper).ValidateDatabaseAccessors(typeof(Node));
    }

    [Fact]
    public async Task TermReaderByNameableIdReturnsParentTerms()
    {
        var connection = DatabaseValidator.GetConnection();
        await connection.OpenAsync();
        var factory = new PoundPupLegacy.CreateModel.Readers.TermReaderByNameableIdFactory();
        await using var reader = await factory.CreateAsync(connection);
        var result = await reader.ReadAsync(new CreateModel.Readers.TermReaderByNameableIdRequest { NameableId = 101444, VocabularyId = 100000 });
        Assert.NotNull(result);
        Assert.NotEmpty(result.ParentTermIds);
    }
    [Fact]
    public async Task TermReaderByNameableIdReturnsEmtptyParentTerms()
    {
        var connection = DatabaseValidator.GetConnection();
        await connection.OpenAsync();
        var factory = new PoundPupLegacy.CreateModel.Readers.TermReaderByNameableIdFactory();
        await using var reader = await factory.CreateAsync(connection);
        var result = await reader.ReadAsync(new CreateModel.Readers.TermReaderByNameableIdRequest { NameableId = 100033, VocabularyId = 100022 });
        Assert.NotNull(result);
        Assert.Empty(result.ParentTermIds);
    }
    [Fact]
    public async Task TermReaderByNameReturnsParentTerms()
    {
        var connection = DatabaseValidator.GetConnection();
        await connection.OpenAsync();
        var factory = new PoundPupLegacy.CreateModel.Readers.TermReaderByNameFactory();
        await using var reader = await factory.CreateAsync(connection);
        var result = await reader.ReadAsync(new CreateModel.Readers.TermReaderByNameRequest { Name = "lethal deprivation", VocabularyId = 100000 });
        Assert.NotNull(result);
        Assert.NotEmpty(result.ParentTermIds);
    }
    [Fact]
    public async Task TermReaderByNameReturnsEmptyParentTerms()
    {
        var connection = DatabaseValidator.GetConnection();
        await connection.OpenAsync();
        var factory = new PoundPupLegacy.CreateModel.Readers.TermReaderByNameFactory();
        await using var reader = await factory.CreateAsync(connection);
        var result = await reader.ReadAsync(new CreateModel.Readers.TermReaderByNameRequest { Name = "homestudy", VocabularyId = 100022 });
        Assert.NotNull(result);
        Assert.Empty(result.ParentTermIds);
    }
}