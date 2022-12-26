using Npgsql;
using NpgsqlTypes;
namespace PoundPupLegacy.Db.Writers;

internal class FileWriter : DatabaseWriter<Model.File>, IDatabaseWriter<Model.File>
{
    private const string ID = "id";
    private const string PATH = "path";
    private const string NAME = "name";
    private const string MIME_TYPE = "mime_type";
    private const string SIZE = "size";

    public static DatabaseWriter<Model.File> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "file",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PATH,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = MIME_TYPE,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = SIZE,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },

            }
        );
        return new FileWriter(command);

    }

    internal FileWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Model.File subdivision)
    {
        WriteValue(subdivision.Id, ID);
        WriteValue(subdivision.Path, PATH);
        WriteValue(subdivision.Name, NAME);
        WriteValue(subdivision.MimeType, MIME_TYPE);
        WriteValue(subdivision.Size, SIZE);
        _command.ExecuteNonQuery();
    }
}
