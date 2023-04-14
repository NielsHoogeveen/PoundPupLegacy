using Npgsql;
using System.Data;
using System.Diagnostics;

namespace PoundPupLegacy.Common;
public interface IDatabaseInserter : IDatabaseAccessor 
{ 
}
public interface IDatabaseInserter<T> : IDatabaseInserter
    where T: IRequest
{
    Task InsertAsync(T item);
}

public interface IDatabaseInserterFactory: IDatabaseAccessorFactory
{
}
public interface IDatabaseInserterFactory<T> : IDatabaseInserterFactory
    where T : IRequest
{
    Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection);
}
public abstract class DatabaseInserterFactoryBase<T, T2> : DatabaseAccessorFactory, IDatabaseInserterFactory<T>
    where T : IRequest
    where T2 : IDatabaseInserter<T>
{
    protected abstract string Sql { get; }

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
        return (IDatabaseInserter<T>)Activator.CreateInstance(typeof(T2), new object[] { command })!;
    }
}
public abstract class DatabaseInserterFactory<T, T2> : DatabaseInserterFactoryBase<T, T2>
    where T : IRequest
    where T2 : DatabaseInserter<T>
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

internal static class IdentifiableDatabaseInserterFactory
{
    internal static readonly NullCheckingIntegerDatabaseParameter Id = new() { Name = "id" };
}

public abstract class IdentifiableDatabaseInserterFactory<T, T2> : DatabaseInserterFactoryBase<T, T2>
    where T : Identifiable
    where T2 : IdentifiableDatabaseInserter<T>
{

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
    internal static readonly NullableIntegerDatabaseParameter Id = new() { Name = "id" };
}
public abstract class ConditionalAutoGenerateIdDatabaseInserterFactory<T, T2> : DatabaseAccessorFactory, IDatabaseInserterFactory<T>
    where T : Identifiable
    where T2 : ConditionalAutoGenerateIdDatabaseInserter<T>
{

    internal static readonly NullableIntegerDatabaseParameter Id = ConditionalAutoGenerateIdDatabaseInserterFactory.Id;
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
        return (IDatabaseInserter<T>)Activator.CreateInstance(typeof(T2), new object[] { command, autoGenerateCommand })!;
    }
}

public abstract class AutoGenerateIdDatabaseInserterFactory<T, T2> : DatabaseInserterFactoryBase<T, T2>
    where T : Identifiable
    where T2 : AutoGenerateIdDatabaseInserter<T>
{
    public abstract string TableName { get; }

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


public abstract class IdentifiableDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : Identifiable
{
    protected IdentifiableDatabaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected abstract IEnumerable<ParameterValue> GetNonIdParameterValues(T request);

    protected sealed override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        yield return ParameterValue.Create(IdentifiableDatabaseInserterFactory.Id , request.Id);
        foreach(var parameter in GetNonIdParameterValues(request)) 
        {
            yield return parameter;
        }
    }

    public async Task InsertAsync(T item)
    {
        foreach(var parameter in GetParameterValues(item)) 
        {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }
}

public abstract class AutoGenerateIdDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : Identifiable
{
    protected AutoGenerateIdDatabaseInserter(NpgsqlCommand command) : base(command)
    {
    }
    public async Task InsertAsync(T item)
    {
        if(item.Id is not null) 
            throw new ArgumentException("The Id property should be null");
        foreach (var parameter in GetParameterValues(item)) 
        {
            parameter.Set(_command);
        }
        item.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception($"Insert action did not return an id.")
        };
    }
}
public abstract class ConditionalAutoGenerateIdDatabaseInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T>
    where T : Identifiable
{

    private readonly NpgsqlCommand _commandAutoGenerate;
    protected ConditionalAutoGenerateIdDatabaseInserter(NpgsqlCommand command, NpgsqlCommand commandAutoGenerate) : base(command)
    {
        _commandAutoGenerate = commandAutoGenerate;
    }

    protected abstract IEnumerable<ParameterValue> GetNonIdParameterValues(T request);

    protected sealed override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        yield return ParameterValue.Create(ConditionalAutoGenerateIdDatabaseInserterFactory.Id, request.Id);
        foreach (var parameter in GetNonIdParameterValues(request)) {
            yield return parameter;
        }
    }

    public async Task InsertAsync(T request)
    {
        if (request.Id is null) {
            foreach(var parameter in GetNonIdParameterValues(request))
            {
                parameter.Set(_commandAutoGenerate);
            };
            request.Id = await _commandAutoGenerate.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception($"Insert action did not return an id.")
            };
        }
        else {
            foreach(var parameter in GetParameterValues(request)) 
            {
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

