using System.Diagnostics;

namespace PoundPupLegacy.Convert;

internal partial class MySqlToPostgresConverter
{
    private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public async Task Convert()
    {

        await TruncateDatabase();
        await _serviceProvider.Migrate<AuthoringStatusMigrator>();
        await _serviceProvider.Migrate<PollStatusMigrator>();
        await _serviceProvider.Migrate<PublicationStatusMigrator>();
        await _serviceProvider.Migrate<NodeTypeMigrator>();
        await _serviceProvider.Migrate<ActionMigrator>();
        await _serviceProvider.Migrate<UserMigrator>();
        await _serviceProvider.Migrate<PollMigrator>();
        await _serviceProvider.Migrate<FileMigratorPPL>();
        await _serviceProvider.Migrate<FileMigratorCPCT>();
        await _serviceProvider.Migrate<VocabularyMigrator>();
        await _serviceProvider.Migrate<CasePartyTypeMigrator>();
        await _serviceProvider.Migrate<CaseTypeMigrator>();
        await _serviceProvider.Migrate<AccessRolePrivilegeMigrator>();
        await _serviceProvider.Migrate<SubdivisionTypeMigrator>();
        await _serviceProvider.Migrate<BasicNameableMigrator>();
        await _serviceProvider.Migrate<ChildPlacementTypeMigrator>();
        await _serviceProvider.Migrate<OrganizationTypeMigrator>();
        await _serviceProvider.Migrate<InterCountryRelationTypeMigrator>();
        await _serviceProvider.Migrate<InterOrganizationalRelationTypeMigrator>();
        await _serviceProvider.Migrate<InterPersonalRelationTypeMigrator>();
        await _serviceProvider.Migrate<PartyPoliticalEntityRelationTypeMigrator>();
        await _serviceProvider.Migrate<PersonOrganizationRelationTypeMigrator>();
        await _serviceProvider.Migrate<BillActionTypeMigrator>();
        await _serviceProvider.Migrate<TypeOfAbuseMigrator>();
        await _serviceProvider.Migrate<TypeOfAbuserMigrator>();
        await _serviceProvider.Migrate<FamilySizeMigrator>();
        await _serviceProvider.Migrate<ProfessionMigrator>();
        await _serviceProvider.Migrate<DenominationMigrator>();
        await _serviceProvider.Migrate<HagueStatusMigrator>();
        await _serviceProvider.Migrate<DocumentTypeMigrator>();

        await _serviceProvider.Migrate<UnitedStatesCongressionalMeetingMigrator>();

        await _serviceProvider.Migrate<FirstLevelGlobalRegionMigrator>();
        await _serviceProvider.Migrate<SecondLevelGlobalRegionMigrator>();
        await _serviceProvider.Migrate<BasicCountryMigrator>();
        await _serviceProvider.Migrate<BindingCountryMigrator>();
        await AddTenantDefaultCountry();
        await _serviceProvider.Migrate<CountrySubdivisionTypeMigratorPartOne>();
        await _serviceProvider.Migrate<BoundCountryMigrator>();
        await _serviceProvider.Migrate<CountrySubdivisionTypeMigratorPartTwo>();
        await _serviceProvider.Migrate<CountryAndFirstLevelSubDivisionMigrator>();
        await _serviceProvider.Migrate<CountryAndFirstAndSecondLevelSubdivisionMigrator>();
        await _serviceProvider.Migrate<CountrySubdivisionTypeMigratorPartThree>();
        await _serviceProvider.Migrate<FirstAndBottomLevelSubdivisionMigrator>();
        await _serviceProvider.Migrate<InformalIntermediateLevelSubdivisionMigrator>();
        await _serviceProvider.Migrate<FormalIntermediateLevelSubdivisionMigrator>();
        await _serviceProvider.Migrate<BasicSecondLevelSubdivisionMigrator>();
        await _serviceProvider.Migrate<BlogPostMigrator>();
        await _serviceProvider.Migrate<ArticleMigrator>();
        await _serviceProvider.Migrate<DiscussionMigrator>();
        await _serviceProvider.Migrate<AdoptionImportMigrator>();
        await _serviceProvider.Migrate<DocumentMigratorPPL>();
        await _serviceProvider.Migrate<OrganizationMigratorPPL>();
        await _serviceProvider.Migrate<UnitedStatesPoliticalPartyAffliationMigrator>();
        await _serviceProvider.Migrate<PersonMigratorPPL>();
        await _serviceProvider.Migrate<AbuseCaseMigrator>();
        await _serviceProvider.Migrate<AbuseCaseTypeOfAbuseMigrator>();
        await _serviceProvider.Migrate<AbuseCaseTypeOfAbuserMigrator>();
        await _serviceProvider.Migrate<ChildTraffickingCaseMigrator>();
        await _serviceProvider.Migrate<CoercedAdoptionCaseMigrator>();
        await _serviceProvider.Migrate<DisruptedPlacementCaseMigrator>();
        await _serviceProvider.Migrate<DeportationCaseMigrator>();
        await _serviceProvider.Migrate<FathersRightsViolationsCaseMigrator>();
        await _serviceProvider.Migrate<WrongfulMedicationCaseMigrator>();
        await _serviceProvider.Migrate<WrongfulRemovalCaseMigrator>();
        await _serviceProvider.Migrate<LocationMigratorPPL>();
        await _serviceProvider.Migrate<PageMigrator>();
        await _serviceProvider.Migrate<ReviewMigrator>();
        await _serviceProvider.Migrate<ActMigrator>();
        await _serviceProvider.Migrate<BillMigrator>();

        await _serviceProvider.Migrate<MenuMigrator>();

        await _serviceProvider.Migrate<PartyPoliticalEntityRelationMigratorPPL>();
        await _serviceProvider.Migrate<PersonOrganizationRelationMigratorPPL>();
        await _serviceProvider.Migrate<InterOrganizationalRelationMigratorPPL>();
        await _serviceProvider.Migrate<InterPersonalRelationMigratorPPL>();
        await _serviceProvider.Migrate<MemberOfCongressMigrator>();
        await _serviceProvider.Migrate<RepresentativeHouseBillActionMigrator>();
        await _serviceProvider.Migrate<SenatorSenateBillActionMigrator>();
        await _serviceProvider.Migrate<DocumentableDocumentMigrator>();

        await _serviceProvider.Migrate<OrganizationMigratorCPCT>();
        await _serviceProvider.Migrate<PersonMigratorCPCT>();
        await _serviceProvider.Migrate<PersonOrganizationRelationMigratorCPCT>();
        await _serviceProvider.Migrate<DocumentMigratorCPCT>();
        await _serviceProvider.Migrate<InterOrganizationalRelationMigratorCPCT>();
        await _serviceProvider.Migrate<InterPersonalRelationMigratorCPCT>();
        await _serviceProvider.Migrate<PartyPoliticalEntityRelationMigratorCPCT>();
        await _serviceProvider.Migrate<LocationMigratorCPCT>();
        await _serviceProvider.Migrate<CaseCaseRelationsMigrator>();
        await _serviceProvider.Migrate<NodeFileMigratorPPL>();
        await _serviceProvider.Migrate<NodeFileMigratorCPCT>();
        await _serviceProvider.Migrate<CommentMigrator>();
        await _serviceProvider.Migrate<AdultAftermathMigrator>();
        await _serviceProvider.Migrate<SearchableMigrator>();
        await _serviceProvider.Migrate<NodeTermMigrator>();
        await _serviceProvider.Migrate<TermHierarchyMigrator>();
        await AddSubdivisionTermsToCases();
        await AddPartyPoliticalEntityRelationTypes();
        await PrepareFiles();

    }

    private readonly IDatabaseConnections _databaseConnections;
    private readonly IServiceProvider _serviceProvider;

    public MySqlToPostgresConverter(IDatabaseConnections databaseConnections, IServiceProvider serviceProvider)
    {
        _databaseConnections = databaseConnections;
        _serviceProvider = serviceProvider;
    }

    private async Task TruncateDatabase()
    {
        _stopwatch.Restart();
        Console.Write("Cleaning database");
        var sql = """
            alter table tenant alter column country_id_default DROP NOT NULL;	
            TRUNCATE authorization_status
            RESTART IDENTITY
            CASCADE;
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
        using var command = _databaseConnections.PostgressConnection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($" took {_stopwatch.ElapsedMilliseconds} ms");
    }


    private async Task PrepareFiles()
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
        using (var command = _databaseConnections.PostgressConnection.CreateCommand()) {
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
        using (var command = _databaseConnections.PostgressConnection.CreateCommand()) {
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
        foreach (DirectoryInfo dir in source.GetDirectories()) {
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
        }
        foreach (FileInfo file in source.GetFiles()) {
            file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
    }

    private async Task AddSubdivisionTermsToCases()
    {
        using var command = _databaseConnections.PostgressConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = """
        INSERT INTO node_term(node_id, term_id)
        select
        n.id node_id,
        t.id term_id
        from "case" c
        join node n on n.id = c.id
        join location_locatable ll on ll.locatable_id = c.id
        join location l on l.id = ll.location_id
        join subdivision s on s.id = l.subdivision_id
        join term t on t.nameable_id = s.id
        join vocabulary v on v.id = t.vocabulary_id
        left join node_term nt on nt.node_id = c.id and nt.term_id = t.id
        where v.name = 'Topics'
        and nt.node_id is null;
        """;
        await command.ExecuteNonQueryAsync();
    }
    private async Task AddTenantDefaultCountry()
    {
        using var command = _databaseConnections.PostgressConnection.CreateCommand();
        command.CommandType = CommandType.Text;
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

    private async Task AddPartyPoliticalEntityRelationTypes()
    {
        using var command = _databaseConnections.PostgressConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = """
        insert into organization_political_entity_relation_type
        select
        distinct
        prt.id
        from party_political_entity_relation pr
        join organization p on p.id = pr.party_id
        join party_political_entity_relation_type prt on prt.id = pr.party_political_entity_relation_type_id;
        insert into person_political_entity_relation_type
        select
        distinct
        prt.id
        from party_political_entity_relation pr
        join person p on p.id = pr.party_id
        join party_political_entity_relation_type prt on prt.id = pr.party_political_entity_relation_type_id;
        """;
        await command.ExecuteNonQueryAsync();

    }

}