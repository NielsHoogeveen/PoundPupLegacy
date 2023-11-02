using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class UserProfileService(
    NpgsqlDataSource dataSource,
    ILogger<UserProfileService> logger,
    ISingleItemDatabaseReaderFactory<UserProfileDocumentReaderRequest, UserProfile> userProfileReaderFactory
) : DatabaseService(dataSource, logger), IUserProfileService
{
    public async Task<UserProfile?> GetUserProfile(int userId)
    {
        return await WithConnection(async (connection) => { 
            var request = new UserProfileDocumentReaderRequest { UserId = userId };
            var reader = await userProfileReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(request);
        });
    }
    public async Task Store(UserProfile userProfile) {
        await WithConnection(async (connection) => {
            var command = connection.CreateCommand();
            command.CommandTimeout = 300;
            command.CommandType = CommandType.Text;
            command.CommandText = """
                update "user"
                set 
                    avatar = @avatar,
                    animal_within = @animal_within,
                    about_me = @about_me,
                    relation_to_child_placement_id = @relation_to_child_placement_id
                where id = @id
                """;
            command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("avatar", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("animal_within", NpgsqlTypes.NpgsqlDbType.Text);
            command.Parameters.Add("about_me", NpgsqlTypes.NpgsqlDbType.Text);
            command.Parameters.Add("relation_to_child_placement_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["id"].Value = userProfile.Id;
            if (userProfile.Avatar is not null) {

                command.Parameters["avatar"].Value = userProfile.Avatar;
            }
            else {
                command.Parameters["avatar"].Value = DBNull.Value;
            }
            if (userProfile.Avatar is not null) {
                command.Parameters["animal_within"].Value = userProfile.AnimalWithin;
            }
            else {
                command.Parameters["animal_within"].Value = DBNull.Value;
            }
            if (userProfile.AboutMe is not null) {
                command.Parameters["about_me"].Value = userProfile.AboutMe;
            }
            else {
                command.Parameters["about_me"].Value = DBNull.Value;
            }
            if (userProfile.RelationToChildPlacementId.HasValue) {
                command.Parameters["relation_to_child_placement_id"].Value = userProfile.RelationToChildPlacementId;
            }
            else {
                command.Parameters["relation_to_child_placement_id"].Value = DBNull.Value;
            }
            await command.ExecuteNonQueryAsync();
            return Unit.Instance;
        });
    }
}
