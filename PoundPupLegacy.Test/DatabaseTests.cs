using PoundPupLegacy.Common.Test;
using Xunit;

namespace PoundPupLegacy.Test;

public class DatabaseTests
{

    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Program));
    }


}
