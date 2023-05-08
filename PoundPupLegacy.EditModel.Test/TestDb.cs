using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Common;
using PoundPupLegacy.Common.Test;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using Xunit.Abstractions;

namespace PoundPupLegacy.Edit.Test;

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

    [Theory]
    [InlineData("abuse", true)]
    [InlineData("sdjkrlj", false)]
    public async Task TopicsExists(string name, bool exists)
    {
        using var connection = DatabaseValidatorBase.GetConnection();
        await connection.OpenAsync();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEditModelReaders();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var topicExistReaderFactory = serviceProvider.GetRequiredService<IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest>>();
        var reader = await topicExistReaderFactory.CreateAsync(connection);
        Assert.Equal(exists, await reader.ReadAsync(new TopicExistsRequest { Name = name }));
        await connection.CloseAsync();

    }

}