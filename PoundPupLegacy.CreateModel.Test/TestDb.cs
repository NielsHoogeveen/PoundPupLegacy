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
}