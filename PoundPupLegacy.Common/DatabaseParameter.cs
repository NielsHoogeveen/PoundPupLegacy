using NpgsqlTypes;

namespace PoundPupLegacy.Common;

public record ParameterValue
{
    public static ParameterValue Create<T>(DatabaseParameter<T> databaseParameter, T value)
    {
        return new ParameterValue(databaseParameter, value);
    }
    private ParameterValue(DatabaseParameter databaseParameter, object? value)
    {
        DatabaseParameter = databaseParameter;
        Value = value;
    }
    public DatabaseParameter DatabaseParameter { get; }
    public object? Value { get; }
}


public abstract record DatabaseParameter
{
    public required string Name { get; init; }

    public abstract bool IsNullable { get; }

    public abstract NpgsqlDbType ParameterType { get; }

}
public abstract record DatabaseParameter<T> : DatabaseParameter
{
}
public record NullableBooleanDatabaseParameter : DatabaseParameter<bool?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Boolean;
}
public record NonNullableBooleanDatabaseParameter : DatabaseParameter<bool>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Boolean;
}


public record NullableIntegerDatabaseParameter : DatabaseParameter<int?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;
}
public record NonNullableIntegerDatabaseParameter : DatabaseParameter<int>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;
}

public record NullableIntegerArrayDatabaseParameter : DatabaseParameter<int?[]>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Array | NpgsqlDbType.Integer;
}
public record NonNullableIntegerArrayDatabaseParameter : DatabaseParameter<int[]>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Array | NpgsqlDbType.Integer;
}
public record NullableLongDatabaseParameter : DatabaseParameter<long?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;
}
public record NonNullableLongDatabaseParameter : DatabaseParameter<long>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;
}
public record AutoGenerateIntegerDatabaseParameter : DatabaseParameter<int?>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Integer;
}

public record NullableDoubleDatabaseParameter : DatabaseParameter<double?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Double;
}
public record NonNullableDoubleDatabaseParameter : DatabaseParameter<double>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Double;
}

public record NullableStringDatabaseParameter : DatabaseParameter<string?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Varchar;
}

public record NonNullableStringDatabaseParameter : DatabaseParameter<string>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Varchar;
}
public record NullableFixedStringDatabaseParameter : DatabaseParameter<string?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Char;
}

public record NonNullableFixedStringDatabaseParameter : DatabaseParameter<string>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Char;
}
public record NullableDecimalDatabaseParameter : DatabaseParameter<decimal?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Numeric;
}

public record NonNullableDecimalDatabaseParameter : DatabaseParameter<decimal>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Numeric;
}
public record NullableDateTimeDatabaseParameter : DatabaseParameter<DateTime?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Timestamp;
}

public record NonNullableDateTimeDatabaseParameter : DatabaseParameter<DateTime>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Timestamp;
}
public record NullableDateRangeDatabaseParameter : DatabaseParameter<DateTimeRange?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
}

public record NonNullableDateRangeDatabaseParameter : DatabaseParameter<DateTimeRange>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
}
public record NullableTimeStampRangeDatabaseParameter : DatabaseParameter<DateTimeRange?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
}

public record NonNullableTimeStampRangeDatabaseParameter : DatabaseParameter<DateTimeRange>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
}
public record NullableFuzzyDateDatabaseParameter : DatabaseParameter<FuzzyDate?>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
}

public record NonNullableFuzzyDateDatabaseParameter : DatabaseParameter<FuzzyDate>
{
    public override bool IsNullable => false;

    public override NpgsqlDbType ParameterType => NpgsqlDbType.Unknown;
}
