using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

internal class TermHierarchyWriter : DatabaseWriter<TermHierarchy>
{

    internal TermHierarchyWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(TermHierarchy termHierarchy)
    {
        _command.Parameters["term_id_parent"].Value = termHierarchy.ParentId;
        _command.Parameters["term_id_child"].Value = termHierarchy.ChildId;
        _command.ExecuteNonQuery();
    }
}
