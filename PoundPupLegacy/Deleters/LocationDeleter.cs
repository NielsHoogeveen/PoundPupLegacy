using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Deleters;
internal sealed class LocationDeleterFactory : IDatabaseDeleterFactory<LocationDeleter>
{
    public async Task<LocationDeleter> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = postgresConnection.CreateCommand();

        var sql = $"""
                    delete from location_locatable
                    where location_id = @location_id and locatable_id = @locatable_id;
                    delete from location
                    where id in (
                        select
                        l.id
                        from location l 
                        left join location_locatable ll on l.id = ll.location_id
                        where ll.location_id is null
                    )
                    """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("location_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("locatable_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new LocationDeleter(command);
    }
}
internal sealed class LocationDeleter : DatabaseDeleter<LocationDeleter.Request>
{
    public record Request
    {
        public required int LocationId { get; init; }
        public required int LocatableId { get; init; }
    }
    public LocationDeleter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task DeleteAsync(Request request)
    {
        _command.Parameters["location_id"].Value = request.LocationId;
        _command.Parameters["locatable_id"].Value = request.LocatableId;
        await _command.ExecuteNonQueryAsync();
    }
}
