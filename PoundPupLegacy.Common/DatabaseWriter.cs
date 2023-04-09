using Npgsql;

namespace PoundPupLegacy.Common;

public abstract class DatabaseWriter
{
    protected readonly NpgsqlCommand _command;

    protected DatabaseWriter(NpgsqlCommand command)
    {
        _command = command;
    }
    protected void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange)
    {
        SetDateTimeRangeParameter(dateTimeRange, parameterDateRange, _command);
    }
    protected void SetTimeStampRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange)
    {
        SetTimeStampRangeParameter(dateTimeRange, parameterDateRange, _command);
    }
    protected void SetTimeStampRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
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
                    command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")})";
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

    protected void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
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

    protected void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange)
    {
        SetDateTimeRange(dateTimeRange, parameterDate, parameterDateRange, _command);
    }
    protected void SetDateTimeRange(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange, NpgsqlCommand command)
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

    protected void SetNullableParameter<T2>(T2? value, string parameter)
    {
        SetNullableParameter(value, parameter, _command);
    }
    protected void SetNullableParameter<T2>(T2? value, string parameter, NpgsqlCommand command)
    {
        if (value is not null) {
            command.Parameters[parameter].Value = value;
        }
        else {
            command.Parameters[parameter].Value = DBNull.Value;
        }
    }
    protected void SetParameter<T2>(T2 value, string parameter)
    {
        SetParameter(value, parameter, _command);
    }

    protected void SetParameter<T2>(T2 value, string parameter, NpgsqlCommand command)
    {
        command.Parameters[parameter].Value = value;
    }
    public async virtual ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }

    protected void Set<T2>(DatabaseParameter<T2> parameter, T2 value)
    {
        if (parameter.IsNullable) 
        {
            SetNullableParameter(value, parameter.Name);
        }
        else 
        {
            SetParameter(value, parameter.Name);
        }
    }

    protected void Set(IEnumerable<ParameterValue> parameterValues)
    {
        foreach (var value in parameterValues) 
        {
            if (value.DatabaseParameter.IsNullable) 
            {
                SetNullableParameter(value.Value, value.DatabaseParameter.Name);
            }
            else 
            {
                SetParameter(value.Value, value.DatabaseParameter.Name);
            }
        }
    }
}
