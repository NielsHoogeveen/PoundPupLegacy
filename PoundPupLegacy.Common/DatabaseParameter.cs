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
public abstract record DatabaseParameter<T>: DatabaseParameter
{
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
