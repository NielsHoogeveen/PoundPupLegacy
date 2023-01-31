using MySqlConnector;

namespace PoundPupLegacy.Convert;

internal abstract class PPLMigrator : Migrator
{
    protected readonly MySqlConnection _mysqlConnection;

    protected PPLMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
        _mysqlConnection = mySqlToPostgresConverter.MysqlConnectionPPL;
    }

}
