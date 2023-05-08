using PoundPupLegacy.Common.Test;
using Xunit;
using Xunit.Abstractions;

namespace PoundPupLegacy.Test;

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
        await new DatabaseValidator(_testOutputHelper).ValidateDatabaseAccessors(typeof(Program));
    }


}
