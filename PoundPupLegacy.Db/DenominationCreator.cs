namespace PoundPupLegacy.Db;

public class DenominationCreator : IEntityCreator<Denomination>
{
    public static void Create(IEnumerable<Denomination> denominations, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var denominationWriter = DenominationWriter.Create(connection);

        foreach (var denomination in denominations)
        {
            nodeWriter.Write(denomination);
            nameableWriter.Write(denomination);
            denominationWriter.Write(denomination);
        }
    }
}
