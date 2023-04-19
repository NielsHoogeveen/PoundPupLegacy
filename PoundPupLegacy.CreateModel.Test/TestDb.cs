using PoundPupLegacy.Common.Test;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Db.Test;



public class TestDb
{


    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Node));
    }

}