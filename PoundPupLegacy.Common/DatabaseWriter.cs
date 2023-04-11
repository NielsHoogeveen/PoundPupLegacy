using Npgsql;

namespace PoundPupLegacy.Common;

public abstract class DatabaseWriter
{
    protected readonly NpgsqlCommand _command;

    protected DatabaseWriter(NpgsqlCommand command)
    {
        _command = command;
    }
    private void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange)
    {
        SetDateTimeRangeParameter(dateTimeRange, parameterDateRange, _command);
    }
    private void SetTimeStampRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange)
    {
        SetTimeStampRangeParameter(dateTimeRange, parameterDateRange, _command);
    }
    private void SetTimeStampRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
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

    private void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
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

    private void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange)
    {
        SetDateTimeRange(dateTimeRange, parameterDate, parameterDateRange, _command);
    }
    private void SetDateTimeRange(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[parameterDateRange].Value = DBNull.Value;
            command.Parameters[parameterDate].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    command.Parameters[parameterDateRange].Value = DBNull.Value;
                    command.Parameters[parameterDate].Value = dateTimeRange.Start;

                }
                else {
                    command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                    command.Parameters[parameterDate].Value = DBNull.Value;
                }

            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                command.Parameters[parameterDate].Value = DBNull.Value;

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")},)";
                command.Parameters[parameterDate].Value = DBNull.Value;

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(,)";
                command.Parameters[parameterDate].Value = DBNull.Value;

            }
        }
    }

    private void SetNullableParameter<T2>(T2? value, string parameter, NpgsqlCommand command)
    {
        if (value is not null) {
            command.Parameters[parameter].Value = value;
        }
        else {
            command.Parameters[parameter].Value = DBNull.Value;
        }
    }

    private void SetParameter<T2>(T2 value, string parameter, NpgsqlCommand command)
    {
        command.Parameters[parameter].Value = value;
    }
    public async virtual ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }

    private void SetUnchecked(DatabaseParameter parameter, object? value, NpgsqlCommand command)
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
            SetTimeStampRangeParameter(value as DateTimeRange, parameter.Name, command);
        }
        else if (parameter is NonNullableFuzzyDateDatabaseParameter) {
            SetTimeStampRangeParameter(value as DateTimeRange, parameter.Name, command);
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
    protected void Set(IEnumerable<ParameterValue> parameterValues, NpgsqlCommand command)
    {
        foreach (var parameterValue in parameterValues) {
            SetUnchecked(parameterValue.DatabaseParameter, parameterValue.Value, command);
        }
    }

}
