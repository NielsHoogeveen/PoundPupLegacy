﻿using PoundPupLegacy.Db;
using System.Data;
using File = PoundPupLegacy.Model.File;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class FileMigratorCPCT : CPCTMigrator
{

    public FileMigratorCPCT(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    protected override string Name => "files";

    protected override async Task MigrateImpl()
    {
        await FileCreator.CreateAsync(ReadFiles(), _postgresConnection);
    }
    private async IAsyncEnumerable<File> ReadFiles()
    {

        var sql = $"""
                SELECT
                f.fid id,
                f.filepath path,
                f.filename `name`,
                f.filemime mime_type,
                f.filesize size
                FROM `files` f
                WHERE fileName NOT IN ('preview', 'thumbnail', '_original')
                and f.fid > 1185
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            
            yield return new File
            {
                Id = null,
                Path = reader.GetString("path"),
                Name = reader.GetString("name"),
                MimeType = reader.GetString("mime_type"),
                Size = reader.GetInt32("size"),
                TenantFiles = new List<TenantFile>{
                    new TenantFile
                    {
                        TenantId = Constants.CPCT,
                        FileId = null,
                        TenantFileId = id
                    },
                    new TenantFile
                    {
                        TenantId = Constants.PPL,
                        FileId = null,
                        TenantFileId = null
                    },
                }
            };

        }
        await reader.CloseAsync();
    }
}