using PoundPupLegacy.CreateModel;
using PoundPupLegacy.Common.Test;

namespace PoundPupLegacy.Db.Test;



public class TestDb
{


    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Node));
    }

}