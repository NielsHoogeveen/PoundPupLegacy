using Npgsql;
using NpgsqlTypes;

namespace PoundPupLegacy.Common;

public record ParameterValue
{
    public static ParameterValue Create<T>(DatabaseParameter<T> databaseParameter, T value)
    {
        return new ParameterValue(databaseParameter, (command) => databaseParameter.Set(value, command));
    }
    private readonly Action<NpgsqlCommand> _action;
    private ParameterValue(DatabaseParameter databaseParameter, Action<NpgsqlCommand> action)
    {
        _action = action;
        DatabaseParameter = databaseParameter;
    }
    public DatabaseParameter DatabaseParameter { get; }

    public void Set(NpgsqlCommand command)
    {
        _action(command);
    }
}


public abstract record DatabaseParameter
{
    public required string Name { get; init; }

    public abstract bool IsNullable { get; }

    public abstract NpgsqlDbType ParameterType { get; }

}
public abstract record DatabaseParameter<T> : DatabaseParameter
{
    public abstract void Set(T value, NpgsqlCommand command);
    protected void SetNullableParameter<T2>(T2? value, NpgsqlCommand command)
    {
        if (value is not null) {
            command.Parameters[Name].Value = value;
        }
        else {
            command.Parameters[Name].Value = DBNull.Value;
        }
    }

    protected void SetParameter<T2>(T2 value, NpgsqlCommand command)
    {
        command.Parameters[Name].Value = value;
    }

    protected void SetTimeStampRangeParameter(DateTimeRange? dateTimeRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[Name].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    throw new ArgumentException("A date range should have a start that is below the end");

                }
                else {
                    var value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")})";
                    command.Parameters[Name].Value = value;
                }
            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[Name].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")})";

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[Name].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")},)";

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[Name].Value = $"(,)";

            }
        }
    }

    protected void SetDateTimeRangeParameter(DateTimeRange? dateTimeRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[Name].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    throw new ArgumentException("A date range should have a start that is below the end");

                }
                else {
                    command.Parameters[Name].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                }

            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[Name].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[Name].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")},)";

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[Name].Value = $"(,)";

            }
        }
    }
}
public record NullableBooleanDatabaseParameter : DatabaseParameter<bool?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Boolean;

    public override void Set(bool? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}
public record NonNullableBooleanDatabaseParameter : DatabaseParameter<bool>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Boolean;

    public override void Set(bool value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }
}


public record NullableIntegerDatabaseParameter : DatabaseParameter<int?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;

    public override void Set(int? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}
public record NonNullableIntegerDatabaseParameter : DatabaseParameter<int>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;

    public override void Set(int value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}

public record NullableIntegerArrayDatabaseParameter : DatabaseParameter<int[]?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Array | NpgsqlDbType.Integer;

    public override void Set(int[]? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}
public record NonNullableIntegerArrayDatabaseParameter : DatabaseParameter<int[]>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Array | NpgsqlDbType.Integer;

    public override void Set(int[] value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}
public record NullableLongDatabaseParameter : DatabaseParameter<long?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;

    public override void Set(long? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}
public record NonNullableLongDatabaseParameter : DatabaseParameter<long>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;

    public override void Set(long value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}
public record AutoGenerateIntegerDatabaseParameter : DatabaseParameter<int?>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;
    public override void Set(int? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }

}

public record NullableDoubleDatabaseParameter : DatabaseParameter<double?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Double;

    public override void Set(double? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}
public record NonNullableDoubleDatabaseParameter : DatabaseParameter<double>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Double;

    public override void Set(double value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}

public record NullableStringDatabaseParameter : DatabaseParameter<string?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Varchar;

    public override void Set(string? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}

public record NonNullableStringDatabaseParameter : DatabaseParameter<string>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Varchar;

    public override void Set(string value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}
public record NullableFixedStringDatabaseParameter : DatabaseParameter<string?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Char;

    public override void Set(string? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}

public record NonNullableFixedStringDatabaseParameter : DatabaseParameter<string>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Char;

    public override void Set(string value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}
public record NullableDecimalDatabaseParameter : DatabaseParameter<decimal?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Numeric;

    public override void Set(decimal? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}

public record NonNullableDecimalDatabaseParameter : DatabaseParameter<decimal>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Numeric;

    public override void Set(decimal value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}
public record NullableDateTimeDatabaseParameter : DatabaseParameter<DateTime?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Timestamp;

    public override void Set(DateTime? value, NpgsqlCommand command)
    {
        SetNullableParameter(value, command);
    }
}

public record NonNullableDateTimeDatabaseParameter : DatabaseParameter<DateTime>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Timestamp;

    public override void Set(DateTime value, NpgsqlCommand command)
    {
        SetParameter(value, command);
    }

}
public record NullableDateRangeDatabaseParameter : DatabaseParameter<DateTimeRange?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
    public override void Set(DateTimeRange? value, NpgsqlCommand command)
    {
        SetDateTimeRangeParameter(value, command);
    }

}

public record NonNullableDateRangeDatabaseParameter : DatabaseParameter<DateTimeRange>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
    public override void Set(DateTimeRange value, NpgsqlCommand command)
    {
        SetDateTimeRangeParameter(value, command);
    }

}
public record NullableTimeStampRangeDatabaseParameter : DatabaseParameter<DateTimeRange?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
    public override void Set(DateTimeRange? value, NpgsqlCommand command)
    {
        SetTimeStampRangeParameter(value, command);
    }

}

public record NonNullableTimeStampRangeDatabaseParameter : DatabaseParameter<DateTimeRange>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;

    public override void Set(DateTimeRange value, NpgsqlCommand command)
    {
        SetTimeStampRangeParameter(value, command);
    }

}
public record NullableFuzzyDateDatabaseParameter : DatabaseParameter<FuzzyDate?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;

    public override void Set(FuzzyDate? value, NpgsqlCommand command)
    {
        SetTimeStampRangeParameter(value?.ToDateTimeRange(), command);
    }

}

public record NonNullableFuzzyDateDatabaseParameter : DatabaseParameter<FuzzyDate>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;

    public override void Set(FuzzyDate value, NpgsqlCommand command)
    {
        SetTimeStampRangeParameter(value.ToDateTimeRange(), command);
    }
}
