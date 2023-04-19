using PoundPupLegacy.Common.Test;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.ViewModel.Test;

public class TestDb
{

    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Node));
    }

}