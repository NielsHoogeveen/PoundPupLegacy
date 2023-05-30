using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;
public interface IDatabaseInserter : IDatabaseAccessor
{
}
public interface IDatabaseInserter< in T> : IDatabaseInserter
    where T : IRequest
{
    Task InsertAsync(T item);
}

public interface IDatabaseInserterFactory : IDatabaseAccessorFactory
{
}
public interface IDatabaseInserterFactory<T> : IDatabaseInserterFactory
    where T : IRequest
{
    Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection);
}
public abstract class DatabaseInserterFactoryBase<T> : DatabaseAccessorFactory, IDatabaseInserterFactory<T>
    where T : IRequest
{

    protected abstract string Sql { get; }

    protected abstract IDatabaseInserter<T> CreateInstance(NpgsqlCommand command);

    public async Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = Sql;

        foreach (var parameter in DatabaseParameters) {
            command.AddParameter(parameter);
        }
        await command.PrepareAsync();
        return CreateInstance(command);
    }
}
public abstract class DatabaseInserterFactory<T> : DatabaseInserterFactoryBase<T>
    where T : IRequest
{
    public abstract string TableName { get; }

    protected override string Sql => $"""
        INSERT INTO public."{TableName}"(
            {string.Join(',', DatabaseParameters.Select(x => x.Name))}
        ) 
        VALUES(
            {string.Join(',', DatabaseParameters.Select(x => $"@{x.Name}"))}
        )
        """;
}

public abstract class BasicDatabaseInserterFactory<T> : DatabaseInserterFactoryBase<T>
    where T : IRequest
{
    protected abstract IEnumerable<ParameterValue> GetParameterValues(T request);
    protected override IDatabaseInserter<T> CreateInstance(NpgsqlCommand command)
    {
        return new BasicDatabaseInserter<T>(command, GetParameterValues);
    }
    public abstract string TableName { get; }

    protected override string Sql => $"""
        INSERT INTO public."{TableName}"(
            {string.Join(',', DatabaseParameters.Select(x => x.Name))}
        ) 
        VALUES(
            {string.Join(',', DatabaseParameters.Select(x => $"@{x.Name}"))}
        )
        """;

}

internal static class IdentifiableDatabaseInserterFactory
{
    internal static readonly NullCheckingIntegerDatabaseParameter Id = new() { Name = "id" };

}

public abstract class IdentifiableDatabaseInserterFactory<T> : DatabaseInserterFactoryBase<T>
    where T : PossiblyIdentifiable
{

    protected abstract IEnumerable<ParameterValue> GetNonIdParameterValues(T request);

    protected override IDatabaseInserter<T> CreateInstance(NpgsqlCommand command)
    {
        return new IdentifiableDatabaseInserter<T>(command, GetNonIdParameterValues);
    }


    internal static readonly NullCheckingIntegerDatabaseParameter Id = IdentifiableDatabaseInserterFactory.Id;

    public abstract string TableName { get; }

    protected override string Sql => $"""
        INSERT INTO public."{TableName}"(
            {string.Join(',', DatabaseParameters.Select(x => x.Name))}
        ) 
        VALUES(
            {string.Join(',', DatabaseParameters.Select(x => $"@{x.Name}"))}
        )
        """;
}

internal static class ConditionalAutoGenerateIdDatabaseInserterFactory
{
    internal static readonly AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
}
public abstract class ConditionalAutoGenerateIdDatabaseInserterFactory<T> : DatabaseAccessorFactory, IDatabaseInserterFactory<T>
    where T : PossiblyIdentifiable
{
    protected abstract IEnumerable<ParameterValue> GetNonIdParameterValues(T request);

    internal static readonly AutoGenerateIntegerDatabaseParameter Id = ConditionalAutoGenerateIdDatabaseInserterFactory.Id;
    public abstract string TableName { get; }

    public async Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""
            INSERT INTO public."{TableName}"(
                {string.Join(',', DatabaseParameters.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', DatabaseParameters.Select(x => $"@{x.Name}"))}
            )
            """;

        foreach (var parameter in DatabaseParameters) {
            command.AddParameter(parameter);
        }

        await command.PrepareAsync();

        var autoGenerateCommand = postgresConnection.CreateCommand();

        autoGenerateCommand.CommandType = CommandType.Text;
        autoGenerateCommand.CommandTimeout = 300;
        autoGenerateCommand.CommandText = DatabaseParameters.Where(x => x is not AutoGenerateIntegerDatabaseParameter).Any()
            ? $"""
                INSERT INTO public."{TableName}"(
                    {string.Join(',', DatabaseParameters.Where(x => x is not AutoGenerateIntegerDatabaseParameter).Select(x => x.Name))}
                ) 
                VALUES(
                    {string.Join(',', DatabaseParameters.Where(x => x is not AutoGenerateIntegerDatabaseParameter).Select(x => $"@{x.Name}"))}
                );
                SELECT lastval();
                """
            : $"""
                INSERT INTO public."{TableName}" DEFAULT VALUES;
                SELECT lastval();
                """;

        foreach (var parameter in DatabaseParameters.Where(x => x is not AutoGenerateIntegerDatabaseParameter)) {
            autoGenerateCommand.AddParameter(parameter);
        }
        await autoGenerateCommand.PrepareAsync();
        return new ConditionalAutoGenerateIdDatabaseInserter<T>(command, autoGenerateCommand, GetNonIdParameterValues);
    }
}

public abstract class AutoGenerateIdDatabaseInserterFactory<T> : DatabaseInserterFactoryBase<T>
    where T : PossiblyIdentifiable
{
    public abstract string TableName { get; }

    protected abstract IEnumerable<ParameterValue> GetParameterValues(T request);

    protected override IDatabaseInserter<T> CreateInstance(NpgsqlCommand command)
    {
        return new AutoGenerateIdDatabaseInserter<T>(command, GetParameterValues);
    }

    protected override string Sql => DatabaseParameters.Any()
        ? $"""
            INSERT INTO public."{TableName}"(
                {string.Join(',', DatabaseParameters.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', DatabaseParameters.Select(x => $"@{x.Name}"))}
            );
            SELECT lastval();
            """
        : $"""
            INSERT INTO public."{TableName}" DEFAULT VALUES;
            SELECT lastval();
            """;
}

public abstract class DatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : IRequest
{
    protected DatabaseInserter(NpgsqlCommand command) : base(command)
    {
    }
    public async Task InsertAsync(T item)
    {
        foreach (var parameter in GetParameterValues(item)) {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }
}


public class IdentifiableDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : PossiblyIdentifiable
{
    private Func<T, IEnumerable<ParameterValue>> _parameterMapper;
    public IdentifiableDatabaseInserter(NpgsqlCommand command, Func<T, IEnumerable<ParameterValue>> parameterMapper) : base(command)
    {
        _parameterMapper = parameterMapper;
    }

    protected sealed override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        yield return ParameterValue.Create(IdentifiableDatabaseInserterFactory.Id, request.Identification.Id);

        foreach (var parameter in _parameterMapper(request)) {
            yield return parameter;
        }
    }

    public async Task InsertAsync(T item)
    {
        foreach (var parameter in GetParameterValues(item)) {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }
}

public class BasicDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : IRequest
{
    private readonly Func<T, IEnumerable<ParameterValue>> _parameterMapper;
    public BasicDatabaseInserter(NpgsqlCommand command, Func<T, IEnumerable<ParameterValue>> parameterMapper) : base(command)
    {
        _parameterMapper = parameterMapper;
    }

    protected sealed override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        foreach (var parameter in _parameterMapper(request)) {
            yield return parameter;
        }
    }

    public async Task InsertAsync(T item)
    {
        foreach (var parameter in GetParameterValues(item)) {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }
}

public class AutoGenerateIdDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : PossiblyIdentifiable
{
    private readonly Func<T, IEnumerable<ParameterValue>> _parameterMapper;
    public AutoGenerateIdDatabaseInserter(NpgsqlCommand command, Func<T, IEnumerable<ParameterValue>> parameterMapper) : base(command)
    {
        _parameterMapper = parameterMapper;
    }

    protected sealed override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        foreach (var parameter in _parameterMapper(request)) {
            yield return parameter;
        }
    }

    public async Task InsertAsync(T item)
    {
        if (item.Identification.Id is not null)
            throw new ArgumentException($"The Id property of {item} should be null");
        foreach (var parameter in GetParameterValues(item)) {
            parameter.Set(_command);
        }
        item.Identification.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception($"Insert action did not return an id.")
        };
    }
}
public class ConditionalAutoGenerateIdDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : PossiblyIdentifiable
{

    private readonly NpgsqlCommand _commandAutoGenerate;
    private readonly Func<T, IEnumerable<ParameterValue>> _parameterMapper;
    public ConditionalAutoGenerateIdDatabaseInserter(NpgsqlCommand command, NpgsqlCommand commandAutoGenerate, Func<T, IEnumerable<ParameterValue>> parameterMapper) : base(command)
    {
        _commandAutoGenerate = commandAutoGenerate;
        _parameterMapper = parameterMapper;
    }

    protected IEnumerable<ParameterValue> GetNonIdParameterValues(T request)
    {
        foreach (var parameter in _parameterMapper(request)) {
            yield return parameter;
        }
    }

    protected sealed override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        yield return ParameterValue.Create(ConditionalAutoGenerateIdDatabaseInserterFactory.Id, request.Identification.Id);
        foreach (var parameter in GetNonIdParameterValues(request)) {
            yield return parameter;
        }
    }

    public async Task InsertAsync(T request)
    {
        if (request.Identification.Id is null) {
            foreach (var parameter in GetNonIdParameterValues(request)) {
                parameter.Set(_commandAutoGenerate);
            };
            request.Identification.Id = await _commandAutoGenerate.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception($"Insert action did not return an id.")
            };
        }
        else {
            foreach (var parameter in GetParameterValues(request)) {
                parameter.Set(_command);
            }
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _commandAutoGenerate.DisposeAsync();
    }
}

