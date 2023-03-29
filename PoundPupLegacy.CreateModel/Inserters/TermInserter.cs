﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TermInserter : DatabaseInserter<Term>, IDatabaseInserter<Term>
{
    private const string VOCABULARY_ID = "vocabulary_id";
    private const string NAME = "name";
    private const string NAMEABLE_ID = "nameable_id";
    public static async Task<DatabaseInserter<Term>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateIdentityInsertStatementAsync(
            connection,
            "term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = VOCABULARY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAMEABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TermInserter(command);

    }

    internal TermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Term term)
    {
        WriteValue(term.VocabularyId, VOCABULARY_ID);
        WriteValue(term.Name.Trim(), NAME);
        WriteValue(term.NameableId, NAMEABLE_ID);
        var retval = await _command.ExecuteScalarAsync();
        term.Id = retval switch {
            int i => i,
            long i => (int)i,
            _ => throw new Exception($"Id could not be set for term {term.Name} in vocabulary {term.VocabularyId}")
        };
    }
}
