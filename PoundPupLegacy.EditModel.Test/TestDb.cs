using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Common;
using PoundPupLegacy.Common.Test;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.UI;
using PoundPupLegacy.EditModel.UI.Services;
using System.Data;
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
    [Fact]
    public async Task SearchServicesSynchronizeProperly()
    {
        using var connection = DatabaseValidatorBase.GetConnection();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IDbConnection>((sp) => {
            return connection;
        });
        serviceCollection.AddEditModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var personListSearch = serviceProvider.GetRequiredService<ISearchService<PersonItem.PersonListItem>>();
        async Task GetList(string str)
        {
            var items = await personListSearch!.GetItems(1, str);
            foreach (var item in items) {
                _testOutputHelper.WriteLine(item.Name);
            };
        }
        var tasks = new List<Task> {
            GetList("a"),
            GetList("b"),
            GetList("c"),
            GetList("d"),
            GetList("e"),
            GetList("f"),
            GetList("g"),
        };
        try {
            await Task.WhenAll(tasks.ToArray());
        }catch (Exception ex) {
            Assert.Fail($"Expected no exception, but found {ex.Message}");
        }
    }
}