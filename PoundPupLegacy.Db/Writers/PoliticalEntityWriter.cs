using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class PoliticalEntityWriter : DatabaseWriter<PoliticalEntity>, IDatabaseWriter<PoliticalEntity>
{
    private const string ID = "id";
    private const string FILE_ID_FLAG = "file_id_flag";
    public static DatabaseWriter<PoliticalEntity> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "political_entity",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FILE_ID_FLAG,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PoliticalEntityWriter(command);
    }

    internal PoliticalEntityWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(PoliticalEntity entity)
    {
        WriteValue(entity.Id, ID);
        WriteNullableValue(entity.FileIdFlag, FILE_ID_FLAG);
        _command.ExecuteNonQuery();
    }
}
