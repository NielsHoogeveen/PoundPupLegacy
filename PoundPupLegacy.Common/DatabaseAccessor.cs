using Npgsql;

namespace PoundPupLegacy.Common;

public interface IDatabaseAccessorFactory
{
    IEnumerable<DatabaseParameter> DatabaseParameters { get; }
}
public abstract class DatabaseAccessorFactory : IDatabaseAccessorFactory
{
    public IEnumerable<DatabaseParameter> DatabaseParameters => GetDatabaseParameters();
    private List<DatabaseParameter> GetDatabaseParameters()
    {
        var t = GetType();
        var fields = t.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return fields.Select(x => x.GetValue(null) as DatabaseParameter).Where(x => x is not null).Select(x => (DatabaseParameter)x!).ToList();
    }

}

public interface IDatabaseAccessor : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }
}

public abstract class DatabaseAccessor: IDatabaseAccessor
{
    protected readonly NpgsqlCommand _command;
    public string Sql => _command.CommandText;
    public bool HasBeenPrepared => _command.IsPrepared;

    protected DatabaseAccessor(NpgsqlCommand command)
    {
        _command = command;
    }
    private static void SetTimeStampRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[parameterDateRange].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    throw new ArgumentException("A date range should have a start that is below the end");

                }
                else {
                    var value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")})";
                    command.Parameters[parameterDateRange].Value = value;
                }
            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")})";

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")},)";

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(,)";

            }
        }
    }

    private static void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[parameterDateRange].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    throw new ArgumentException("A date range should have a start that is below the end");

                }
                else {
                    command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                }

            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")},)";

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(,)";

            }
        }
    }


    private static void SetNullableParameter<T2>(T2? value, string parameter, NpgsqlCommand command)
    {
        if (value is not null) {
            command.Parameters[parameter].Value = value;
        }
        else {
            command.Parameters[parameter].Value = DBNull.Value;
        }
    }

    private static void SetParameter<T2>(T2 value, string parameter, NpgsqlCommand command)
    {
        command.Parameters[parameter].Value = value;
    }
    public async virtual ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }

    private static void SetUnchecked(DatabaseParameter parameter, object? value, NpgsqlCommand command)
    {
        if (parameter is NullableDateRangeDatabaseParameter) {
            SetDateTimeRangeParameter(value as DateTimeRange, parameter.Name, command);
        }
        else if (parameter is NonNullableDateRangeDatabaseParameter) {
            SetDateTimeRangeParameter(value as DateTimeRange, parameter.Name, command);
        }
        else if (parameter is NullableTimeStampRangeDatabaseParameter) {
            SetTimeStampRangeParameter(value as DateTimeRange, parameter.Name, command);
        }
        else if (parameter is NonNullableTimeStampRangeDatabaseParameter) {
            SetTimeStampRangeParameter(value as DateTimeRange, parameter.Name, command);
        }
        else if (parameter is NullableFuzzyDateDatabaseParameter) {
            var fuzzyDate = value as FuzzyDate;
            SetTimeStampRangeParameter(fuzzyDate?.ToDateTimeRange(), parameter.Name, command);
        }
        else if (parameter is NonNullableFuzzyDateDatabaseParameter) {
            var fuzzyDate = value as FuzzyDate;
            SetTimeStampRangeParameter(fuzzyDate!.ToDateTimeRange(), parameter.Name, command);
        }
        else {
            if (parameter.IsNullable) {
                SetNullableParameter(value, parameter.Name, command);
            }
            else {
                SetParameter(value, parameter.Name, command);
            }
        }
    }
    protected void Set(IEnumerable<ParameterValue> parameterValues)
    {
        Set(parameterValues, _command);
    }
    protected static void Set(IEnumerable<ParameterValue> parameterValues, NpgsqlCommand command)
    {
        foreach (var parameterValue in parameterValues) {
            SetUnchecked(parameterValue.DatabaseParameter, parameterValue.Value, command);
        }
    }

}
