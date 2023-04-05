using MySqlConnector;
using Npgsql;
using System.Diagnostics;

namespace PoundPupLegacy.Convert;

internal partial class MySqlToPostgresConverter
{
    private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public async Task Convert()
    {
        var mysqlConnectionPPL = new MySqlConnection(ConnectionStringMariaDbPPL);
        var mysqlConnectionCPCT = new MySqlConnection(ConnectionStringMariaDbCPCT);
        var postgresConnection = new NpgsqlConnection(ConnectStringPostgresql);
        await mysqlConnectionPPL.OpenAsync();
        await mysqlConnectionCPCT.OpenAsync();
        await postgresConnection.OpenAsync();

        var sc = new ServiceCollection();
        sc.AddCreateModelAccessors();
        var connections = new DatabaseConnections {
            MysqlConnectionCPCT = mysqlConnectionCPCT,
            MysqlConnectionPPL = mysqlConnectionPPL,
            PostgressConnection = postgresConnection
        };
        sc.AddSingleton<IDatabaseConnections>((sp) => connections);
        sc.AddMigrators();
        var sp = sc.BuildServiceProvider();

        await TruncateDatabase(postgresConnection);

                
        await sp.Migrate<PollStatusMigrator>();
        await sp.Migrate<PublicationStatusMigrator>();
        await sp.Migrate<NodeTypeMigrator>();
        await sp.Migrate<ActionMigrator>();
        await sp.Migrate<UserMigrator>();
        await sp.Migrate<PollMigrator>();
        await sp.Migrate<FileMigratorPPL>();
        await sp.Migrate<FileMigratorCPCT>();
        await sp.Migrate<VocabularyMigrator>();
        await sp.Migrate<CasePartyTypeMigrator>();
        await sp.Migrate<CaseTypeMigrator>();
        await sp.Migrate<AccessRolePrivilegeMigrator>();
        await sp.Migrate<SubdivisionTypeMigrator>();
        await sp.Migrate<BasicNameableMigrator>();
        await sp.Migrate<ChildPlacementTypeMigrator>();
        await sp.Migrate<OrganizationTypeMigrator>();
        await sp.Migrate<InterCountryRelationTypeMigrator>();
        await sp.Migrate<InterOrganizationalRelationTypeMigrator>();
        await sp.Migrate<InterPersonalRelationTypeMigrator>();
        await sp.Migrate<PartyPoliticalEntityRelationTypeMigrator>();
        await sp.Migrate<PersonOrganizationRelationTypeMigrator>();
        await sp.Migrate<BillActionTypeMigrator>();
        await sp.Migrate<TypeOfAbuseMigrator>();
        await sp.Migrate<TypeOfAbuserMigrator>();
        await sp.Migrate<FamilySizeMigrator>();
        await sp.Migrate<ProfessionMigrator>();
        await sp.Migrate<DenominationMigrator>();
        await sp.Migrate<HagueStatusMigrator>();
        await sp.Migrate<DocumentTypeMigrator>();

        await sp.Migrate<UnitedStatesCongressionalMeetingMigrator>();

        await sp.Migrate<FirstLevelGlobalRegionMigrator>();
        await sp.Migrate<SecondLevelGlobalRegionMigrator>();
        await sp.Migrate<BasicCountryMigrator>();
        await sp.Migrate<BindingCountryMigrator>();
        await AddTenantDefaultCountry(postgresConnection);
        await sp.Migrate<CountrySubdivisionTypeMigratorPartOne>();
        await sp.Migrate<BoundCountryMigrator>();
        await sp.Migrate<CountrySubdivisionTypeMigratorPartTwo>();
        await sp.Migrate<CountryAndFirstLevelSubDivisionMigrator>();
        await sp.Migrate<CountryAndFirstAndSecondLevelSubdivisionMigrator>();
        await sp.Migrate<CountrySubdivisionTypeMigratorPartThree>();
        await sp.Migrate<FirstAndBottomLevelSubdivisionMigrator>();
        await sp.Migrate<InformalIntermediateLevelSubdivisionMigrator>();
        await sp.Migrate<FormalIntermediateLevelSubdivisionMigrator>();
        await sp.Migrate<BasicSecondLevelSubdivisionMigrator>();
        await sp.Migrate<BlogPostMigrator>();
        await sp.Migrate<ArticleMigrator>();
        await sp.Migrate<DiscussionMigrator>();
        await sp.Migrate<AdoptionImportMigrator>();
        await sp.Migrate<DocumentMigratorPPL>();
        await sp.Migrate<OrganizationMigratorPPL>();
        await sp.Migrate<UnitedStatesPoliticalPartyAffliationMigrator>();
        await sp.Migrate<PersonMigratorPPL>();
        await sp.Migrate<AbuseCaseMigrator>();
        await sp.Migrate<ChildTraffickingCaseMigrator>();
        await sp.Migrate<CoercedAdoptionCaseMigrator>();
        await sp.Migrate<DisruptedPlacementCaseMigrator>();
        await sp.Migrate<DeportationCaseMigrator>();
        await sp.Migrate<FathersRightsViolationsCaseMigrator>();
        await sp.Migrate<WrongfulMedicationCaseMigrator>();
        await sp.Migrate<WrongfulRemovalCaseMigrator>();
        await sp.Migrate<LocationMigratorPPL>();
        await sp.Migrate<PageMigrator>();
        await sp.Migrate<ReviewMigrator>();
        await sp.Migrate<ActMigrator>();
        await sp.Migrate<BillMigrator>();
        await sp.Migrate<NodeTermMigrator>();
        await sp.Migrate<MenuMigrator>();
        await sp.Migrate<DocumentableDocumentMigrator>();
        await sp.Migrate<TermHierarchyMigrator>();
        await sp.Migrate<PartyPoliticalEntityRelationMigratorPPL>();
        await sp.Migrate<PersonOrganizationRelationMigratorPPL>();
        await sp.Migrate<InterOrganizationalRelationMigratorPPL>();
        await sp.Migrate<InterPersonalRelationMigratorPPL>();
        await sp.Migrate<MemberOfCongressMigrator>();
        await sp.Migrate<RepresentativeHouseBillActionMigrator>();
        await sp.Migrate<SenatorSenateBillActionMigrator>();
        await sp.Migrate<OrganizationMigratorCPCT>();
        await sp.Migrate<PersonMigratorCPCT>();
        await sp.Migrate<PersonOrganizationRelationMigratorCPCT>();
        await sp.Migrate<DocumentMigratorCPCT>();
        await sp.Migrate<InterOrganizationalRelationMigratorCPCT>();
        await sp.Migrate<InterPersonalRelationMigratorCPCT>();
        await sp.Migrate<PartyPoliticalEntityRelationMigratorCPCT>();
        await sp.Migrate<LocationMigratorCPCT>();
        await sp.Migrate<SearchableMigrator>();
        await sp.Migrate<CaseCaseRelationsMigrator>();
        await sp.Migrate<NodeFileMigratorPPL>();
        await sp.Migrate<NodeFileMigratorCPCT>();
        await sp.Migrate<CommentMigrator>();
        await sp.Migrate<AdultAftermathMigrator>();
        await PrepareFiles(postgresConnection);
    }

    internal const string ConnectionStringMariaDbPPL = "server=localhost;userid=root;Password=root;database=ppl";
    internal const string ConnectionStringMariaDbCPCT = "server=localhost;userid=root;Password=root;database=cpct";
    internal const string ConnectStringPostgresql = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=True";

    public MySqlToPostgresConverter()
    {
    }

    private async Task TruncateDatabase(NpgsqlConnection postgresConnection)
    {
        _stopwatch.Restart();
        Console.Write("Cleaning database");
        var sql = """
            alter table tenant alter column country_id_default DROP NOT NULL;	
            TRUNCATE case_parties
            RESTART IDENTITY
            CASCADE;
            TRUNCATE poll_status
            RESTART IDENTITY
            CASCADE;
            TRUNCATE publication_status
            RESTART IDENTITY
            CASCADE;
            TRUNCATE case_party_type
            RESTART IDENTITY
            CASCADE;
            TRUNCATE file
            RESTART IDENTITY
            CASCADE;
            TRUNCATE node 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE principal 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE user_group 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE node_type 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE action 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE menu_item 
            RESTART IDENTITY
            CASCADE;
            """;
        using var command = postgresConnection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($" took {_stopwatch.ElapsedMilliseconds} ms");
    }


    private async Task PrepareFiles(NpgsqlConnection postgresConnection)
    {
        var cpctDirectory = new DirectoryInfo("\\\\wsl.localhost\\Ubuntu\\home\\niels\\cpct");
        var pplDirectory = new DirectoryInfo("\\\\wsl.localhost\\Ubuntu\\home\\niels\\ppl");
        var combinedDirectory = new DirectoryInfo("\\\\wsl.localhost\\Ubuntu\\home\\niels\\files");
        var attachmentDirectory = new DirectoryInfo("\\\\wsl.localhost\\Ubuntu\\home\\niels\\attachments");
        foreach (FileInfo file in combinedDirectory.GetFiles()) {
            file.Delete();
        }
        foreach (DirectoryInfo dir in combinedDirectory.GetDirectories()) {
            dir.Delete(true);
        }
        foreach (FileInfo file in attachmentDirectory.GetFiles()) {
            file.Delete();
        }
        foreach (DirectoryInfo dir in attachmentDirectory.GetDirectories()) {
            dir.Delete(true);
        }
        combinedDirectory.CreateSubdirectory("files");
        List<(int, string)> filePaths = new();
        using (var command = postgresConnection.CreateCommand()) {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = """
                select 
                f.id,
                f.path,	
                case 
                    when tf1.tenant_file_id = tf6.tenant_file_id then  1
                    when tf6.file_id = tf6.tenant_file_id and tf1.file_id <> tf1.tenant_file_id then 1
                    when tf6.file_id <> tf6.tenant_file_id and tf1.file_id = tf1.tenant_file_id then 6
                    else -1
                end	tenant_id
                from "file" f
                left join tenant_file tf1 on tf1.file_id = f.id and tf1.tenant_id = 1
                left join tenant_file tf6 on tf6.file_id = f.id and tf6.tenant_id = 6
                """;

            var reader = await command.ExecuteReaderAsync();
            while (reader.Read()) {
                var id = reader.GetInt32(0);
                var path = reader.GetString(1);
                var tenant = reader.GetInt32(2);
                var newFileName = Guid.NewGuid();
                var tagetPath = $"{attachmentDirectory.FullName}\\{newFileName}";
                Directory.CreateDirectory(Path.GetDirectoryName(tagetPath)!);
                var sourcePath = tenant switch {
                    1 => $"{pplDirectory.FullName}\\{path.Replace("/", "\\")}",
                    6 => $"{cpctDirectory.FullName}\\{path.Replace("/", "\\")}",
                    _ => throw new Exception($"Tenant {tenant} is unknown"),
                };
                if (System.IO.File.Exists(sourcePath)) {
                    System.IO.File.Copy(sourcePath, tagetPath);
                    filePaths.Add((id, newFileName.ToString()));
                }
                else {
                    Console.WriteLine($"file {sourcePath} not found");
                }
            }
            await reader.CloseAsync();
        }
        using (var command = postgresConnection.CreateCommand()) {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = """
                UPDATE file set path = @path where id = @id
                """;
            command.Parameters.Add("path", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            foreach (var (id, path) in filePaths) {
                command.Parameters["id"].Value = id;
                command.Parameters["path"].Value = path;
                command.ExecuteNonQuery();
            }
        }
        foreach (var file in (new DirectoryInfo($"{pplDirectory}\\files\\flags")).GetFiles()) {

            var tagetPath = $"{combinedDirectory.FullName}\\files\\flags\\{file.Name}";
            Directory.CreateDirectory(Path.GetDirectoryName(tagetPath)!);
            System.IO.File.Copy(file.FullName, tagetPath);
        }
        foreach (var file in (new DirectoryInfo($"{pplDirectory}\\files\\images")).GetFiles()) {

            var tagetPath = $"{combinedDirectory.FullName}\\files\\images\\{file.Name}";
            Directory.CreateDirectory(Path.GetDirectoryName(tagetPath)!);
            System.IO.File.Copy(file.FullName, tagetPath);
        }
        foreach (var file in (new DirectoryInfo($"{pplDirectory}\\files\\jcics")).GetFiles()) {

            var tagetPath = $"{combinedDirectory.FullName}\\files\\jcics\\{file.Name}";
            Directory.CreateDirectory(Path.GetDirectoryName(tagetPath)!);
            System.IO.File.Copy(file.FullName, tagetPath);
        }
        foreach (var file in (new DirectoryInfo($"{pplDirectory}\\files\\my_own_baby_pages")).GetFiles()) {

            var tagetPath = $"{combinedDirectory.FullName}\\files\\my_own_baby_pages\\{file.Name}";
            Directory.CreateDirectory(Path.GetDirectoryName(tagetPath)!);
            System.IO.File.Copy(file.FullName, tagetPath);
        }
        foreach (var file in (new DirectoryInfo($"{pplDirectory}\\files\\pictures")).GetFiles()) {

            var tagetPath = $"{combinedDirectory.FullName}\\files\\pictures\\{file.Name}";
            Directory.CreateDirectory(Path.GetDirectoryName(tagetPath)!);
            System.IO.File.Copy(file.FullName, tagetPath);
        }

        Directory.CreateDirectory(Path.GetDirectoryName($"{combinedDirectory}\\files\\userimages\\Image")!);
        CopyFilesRecursively(new DirectoryInfo($"{pplDirectory}\\files\\userimages\\Image"), new DirectoryInfo($"{combinedDirectory}\\files\\userimages\\Image"));
    }

    private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (DirectoryInfo dir in source.GetDirectories()){
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
        }
        foreach (FileInfo file in source.GetFiles()) {
            file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
    }

    private async Task AddTenantDefaultCountry(NpgsqlConnection postgresConnection)
    {
        using (var command = postgresConnection.CreateCommand()) {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = """
            update tenant
                set country_id_default = subquery.country_id
                from(
                    select
                    6 tenant_id,
                    c.id country_id
                    from country c
                    join tenant_node tn on tn.tenant_id = 1 and tn.node_id = c.id
                    where tn.url_id = 4023
                    union
                    select
                    1 tenant_id,
                    c.id country_id
                    from country c
                    join node n on c.id = n.id and n.title ilike 'United State%'
                    join tenant_node tn on tn.tenant_id = 1 and tn.node_id = c.id
                    where tn.url_id = 3805
            	) subquery
            	where tenant.id = subquery.tenant_id;
            alter table tenant alter column country_id_default SET NOT NULL;	
            """;
            await command.ExecuteNonQueryAsync();
        }
    }
}