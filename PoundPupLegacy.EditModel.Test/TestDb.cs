using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Common.Test;
using PoundPupLegacy.EditModel;
using System.Reflection;

namespace PoundPupLegacy.Edit.Test;

public class TestDb
{

    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Node));
    }

}