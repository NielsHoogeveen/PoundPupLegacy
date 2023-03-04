using MySqlConnector;

namespace PoundPupLegacy.Convert;

internal abstract class CPCTMigrator : Migrator
{
    protected override MySqlConnection MysqlConnection { get; }

    protected CPCTMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
        MysqlConnection = mySqlToPostgresConverter.MysqlConnectionCPCT;
    }

    protected async Task<(int, int)> GetNodeId(int urlId)
    {
        var (id, ownerId) = GetUrlIdAndTenant(urlId);
        if (urlId >= 33162 && ownerId == Constants.CPCT) {
            var nodeId = await _nodeIdReader.ReadAsync(Constants.CPCT, id);
            var node = await _tenantNodeByUrlIdReader.ReadAsync(Constants.PPL, id);
            if (node is not null) {
                return (nodeId, node.PublicationStatusId);
            }
            else {
                return (nodeId, 0);
            }

        }
        return (await _nodeIdReader.ReadAsync(Constants.PPL, id), 1);
    }



    protected (int, int) GetUrlIdAndTenant(int urlId)
    {
        return urlId switch {
            10793 => (6138, Constants.PPL),
            34880 => (35760, Constants.PPL),
            35725 => (37560, Constants.PPL),
            33644 => (38279, Constants.PPL),
            36680 => (39755, Constants.PPL),
            34973 => (40525, Constants.PPL),
            45964 => (41694, Constants.PPL),
            35124 => (45974, Constants.PPL),
            34126 => (48192, Constants.PPL),
            34082 => (52502, Constants.PPL),
            34015 => (53756, Constants.PPL),
            49152 => (60032, Constants.PPL),
            35138 => (73657, Constants.PPL),
            35146 => (73661, Constants.PPL),
            33255 => (33987, Constants.PPL),
            44216 => (29148, Constants.PPL),
            49224 => (35567, Constants.PPL),
            35233 => (44675, Constants.PPL),
            33454 => (35190, Constants.PPL),
            39431 => (55108, Constants.PPL),
            48210 => (34899, Constants.CPCT),
            48330 => (48545, Constants.CPCT),
            47699 => (48846, Constants.CPCT),
            > 33162 => (urlId, Constants.CPCT),
            _ => (urlId, Constants.PPL),
        };
    }

}
