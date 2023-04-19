using PoundPupLegacy.Common.Test;
using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Edit.Test;

public class TestDb
{

    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Node));
    }

}