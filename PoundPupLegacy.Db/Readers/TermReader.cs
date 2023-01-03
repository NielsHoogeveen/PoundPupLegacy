using System.Data;

namespace PoundPupLegacy.Db.Readers;

internal class TermReader : DatabaseReader<Term>, IDatabaseReader<TermReader>
{
    public static TermReader Create(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id, nameable_id FROM term WHERE vocabulary_id = @vocabulary_id AND name = @name
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("vocabulary_id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        command.Prepare();

        return new TermReader(command);

    }

    internal TermReader(NpgsqlCommand command) : base(command) { }

    internal Term Read(int vocabularyId, string name)
    {
        _command.Parameters["vocabulary_id"].Value = vocabularyId;
        _command.Parameters["name"].Value = name;

        var reader = _command.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            var term = new Term
            {
                Id = reader.GetInt32("id"),
                Name = name,
                VocabularyId = vocabularyId,
                NameableId = reader.GetInt32("nameable_id")
            };
            reader.Close();
            return term;
        }
        reader.Close();
        throw new Exception("term cannot be found");
    }
}
