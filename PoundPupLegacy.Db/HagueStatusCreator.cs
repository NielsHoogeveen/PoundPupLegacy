﻿namespace PoundPupLegacy.Db;

public class HagueStatusCreator : IEntityCreator<HagueStatus>
{
    public static void Create(IEnumerable<HagueStatus> hagueStatuss, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var hagueStatusWriter = HagueStatusWriter.Create(connection);

        foreach (var hagueStatus in hagueStatuss)
        {
            nodeWriter.Write(hagueStatus);
            nameableWriter.Write(hagueStatus);
            hagueStatusWriter.Write(hagueStatus);
        }
    }
}
