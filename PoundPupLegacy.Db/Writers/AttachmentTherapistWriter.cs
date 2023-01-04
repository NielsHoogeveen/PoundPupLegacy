namespace PoundPupLegacy.Db.Writers;

internal class AttachmentTherapistWriter : DatabaseWriter<AttachmentTherapist>, IDatabaseWriter<AttachmentTherapist>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseWriter<AttachmentTherapist>>  CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
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

    internal override async Task WriteAsync(AttachmentTherapist therapist)
    {
        if (therapist.Id is null)
            throw new NullReferenceException();

        WriteValue(therapist.Id, ID);
        WriteNullableValue(therapist.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
