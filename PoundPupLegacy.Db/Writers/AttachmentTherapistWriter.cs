using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class AttachmentTherapistWriter : DatabaseWriter<AttachmentTherapist>, IDatabaseWriter<AttachmentTherapist>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    public static DatabaseWriter<AttachmentTherapist> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "attachment_therapist",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new AttachmentTherapistWriter(command);

    }

    internal AttachmentTherapistWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(AttachmentTherapist therapist)
    {
        WriteValue(therapist.Id, ID);
        WriteNullableValue(therapist.Description, DESCRIPTION);
        _command.ExecuteNonQuery();
    }
}
