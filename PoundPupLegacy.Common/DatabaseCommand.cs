using Npgsql;

namespace PoundPupLegacy.Common;

public static class DatabaseCommand
{
    public static void AddParameter(this NpgsqlCommand command, DatabaseParameter parameter)
    {
        command.Parameters.Add(parameter.Name, parameter.ParameterType);
    }
}
