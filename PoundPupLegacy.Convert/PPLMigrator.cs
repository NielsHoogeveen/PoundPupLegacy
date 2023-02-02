using MySqlConnector;

namespace PoundPupLegacy.Convert;

internal abstract class PPLMigrator : Migrator
{
    protected override MySqlConnection MysqlConnection { get; }

    protected PPLMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
        MysqlConnection = mySqlToPostgresConverter.MysqlConnectionPPL;
    }

}
