using Microsoft.VisualStudio.TestPlatform.TestHost;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Common.Test;
using PoundPupLegacy.ViewModel.Models;
using System.Reflection;

namespace PoundPupLegacy.ViewModel.Test;

public class TestDb
{

    [Fact]
    public async void AllDatabaseAccessorFactoriesCreateAnAccessorThatHasValidSQLAndTheCorrectParameters()
    {
        await new DatabaseValidator().ValidateDatabaseAccessors(typeof(Node));
    }

}