﻿using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;
public interface IDatabaseInserter : IDatabaseAccessor 
{ 
}
public interface IDatabaseInserter<T> : IDatabaseInserter
{
    Task InsertAsync(T item);
}

public interface IDatabaseInserterFactory: IDatabaseAccessorFactory
{
}
public interface IDatabaseInserterFactory<T> : IDatabaseInserterFactory
{
    Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection);
}
public abstract class DatabaseInserterFactoryBase<T, T2> : DatabaseAccessorFactory, IDatabaseInserterFactory<T>
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

public abstract class ConditionalAutoGenerateIdDatabaseInserterFactory<T, T2> : IDatabaseInserterFactory<T>
    where T : Identifiable
    where T2 : ConditionalAutoGenerateIdDatabaseInserter<T>
{
    public abstract string TableName { get; }

    public IEnumerable<DatabaseParameter> DatabaseParameters => GetDatabaseParameters();
    private List<DatabaseParameter> GetDatabaseParameters()
    {
        var t = GetType();
        var fields = t.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return fields.Select(x => x.GetValue(null) as DatabaseParameter).Where(x => x is not null).Select(x => (DatabaseParameter)x!).ToList();
    }

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

public abstract class DatabaseInserter<T> : DatabaseAccessor, IDatabaseInserter<T>
{
    public abstract IEnumerable<ParameterValue> GetParameterValues(T item);
    protected DatabaseInserter(NpgsqlCommand command) : base(command)
    {
    }
    public async Task InsertAsync(T item)
    {
        Set(GetParameterValues(item));
        await _command.ExecuteNonQueryAsync();
    }
}
public abstract class AutoGenerateIdDatabaseInserter<T> : DatabaseAccessor, IDatabaseInserter<T>
    where T : Identifiable
{
    public abstract IEnumerable<ParameterValue> GetParameterValues(T item);
    protected AutoGenerateIdDatabaseInserter(NpgsqlCommand command) : base(command)
    {
    }
    public async Task InsertAsync(T item)
    {
        Set(GetParameterValues(item));
        item.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception($"Insert action did not return an id.")
        };
    }
}
public abstract class ConditionalAutoGenerateIdDatabaseInserter<T> : DatabaseAccessor, IDatabaseInserter<T>
    where T : Identifiable
{
    public abstract IEnumerable<ParameterValue> GetParameterValues(T item);

    private readonly NpgsqlCommand _commandAutoGenerate;
    protected ConditionalAutoGenerateIdDatabaseInserter(NpgsqlCommand command, NpgsqlCommand commandAutoGenerate) : base(command)
    {
        _commandAutoGenerate = commandAutoGenerate;
    }
    public async Task InsertAsync(T item)
    {
        if (item.Id is null) {
            Set(GetParameterValues(item).Where(x => x.DatabaseParameter is not AutoGenerateIntegerDatabaseParameter), _commandAutoGenerate);
            item.Id = await _commandAutoGenerate.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception($"Insert action did not return an id.")
            };
        }
        else {
            Set(GetParameterValues(item), _command);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _commandAutoGenerate.DisposeAsync();
    }
}

