using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Db.Writers;

internal class TermHierarchyWriter : DatabaseWriter<TermHierarchy>, IDatabaseWriter<TermHierarchy>
{
    public static DatabaseWriter<TermHierarchy> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."term_hierarchy" (term_id_parent, term_id_child) VALUES(@term_id_parent,@term_id_child)""";
        command.Parameters.Add("term_id_parent", NpgsqlDbType.Integer);
        command.Parameters.Add("term_id_child", NpgsqlDbType.Integer);
        return new TermHierarchyWriter(command);

    }
    private TermHierarchyWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(TermHierarchy termHierarchy)
    {
        _command.Parameters["term_id_parent"].Value = termHierarchy.ParentId;
        _command.Parameters["term_id_child"].Value = termHierarchy.ChildId;
        _command.ExecuteNonQuery();
    }
}
