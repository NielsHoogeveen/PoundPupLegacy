﻿namespace PoundPupLegacy.Db.Writers;

internal sealed class VocabularyWriter : DatabaseWriter<Vocabulary>, IDatabaseWriter<Vocabulary>
{
    private const string ID = "id";
    private const string OWNER_ID = "owner_id";
    private const string NAME = "name";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseWriter<Vocabulary>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "vocabulary",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = OWNER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new VocabularyWriter(command);

    }

    internal VocabularyWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Vocabulary vocabulary)
    {
        if (vocabulary.Id is null)
            throw new NullReferenceException();
        WriteValue(vocabulary.Id, ID);
        WriteValue(vocabulary.OwnerId, OWNER_ID);
        WriteValue(vocabulary.Name, NAME);
        WriteValue(vocabulary.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
