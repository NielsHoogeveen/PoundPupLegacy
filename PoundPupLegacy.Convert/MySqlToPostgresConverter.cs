using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db.Readers;
using System.Diagnostics;

namespace PoundPupLegacy.Convert;

internal partial class MySqlToPostgresConverter : IAsyncDisposable
{
    private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public async Task Convert()
    {
        //await TruncateDatabase();
        //await (new PublicationStatusMigrator(this)).Migrate();
        //await (new CaseRelationTypeMigrator(this)).Migrate();
        //await (new NodeTypeMigrator(this)).Migrate();
        //await (new ActionMigrator(this)).Migrate();
        //await (new UserMigrator(this)).Migrate();
        //await (new FileMigratorPPL(this)).Migrate();
        //await (new FileMigratorCPCT(this)).Migrate();
        //await (new VocabularyMigrator(this)).Migrate();
        //await (new SubdivisionTypeMigrator(this)).Migrate();
        //await (new BasicNameableMigrator(this)).Migrate();
        //await (new ChildPlacementTypeMigrator(this)).Migrate();
        //await (new OrganizationTypeMigrator(this)).Migrate();
        //await (new InterCountryRelationTypeMigrator(this)).Migrate();
        //await (new InterOrganizationalRelationTypeMigrator(this)).Migrate();
        //await (new InterPersonalRelationTypeMigrator(this)).Migrate();
        //await (new PartyPoliticalEntityRelationTypeMigrator(this)).Migrate();
        //await (new PersonOrganizationRelationTypeMigrator(this)).Migrate();
        //await (new TypeOfAbuseMigrator(this)).Migrate();
        //await (new TypeOfAbuserMigrator(this)).Migrate();
        //await (new FamilySizeMigrator(this)).Migrate();
        //await (new ProfessionMigrator(this)).Migrate();
        //await (new DenominationMigrator(this)).Migrate();
        //await (new HagueStatusMigrator(this)).Migrate();
        //await (new DocumentTypeMigrator(this)).Migrate();
        //await (new FirstLevelGlobalRegionMigrator(this)).Migrate();
        //await (new SecondLevelGlobalRegionMigrator(this)).Migrate();
        //await (new BasicCountryMigrator(this)).Migrate();
        //await (new BindingCountryMigrator(this)).Migrate();
        //await (new CountrySubdivisionTypeMigratorPartOne(this)).Migrate();
        //await (new BoundCountryMigrator(this)).Migrate();
        //await (new CountrySubdivisionTypeMigratorPartTwo(this)).Migrate();
        //await (new CountryAndFirstLevelSubDivisionMigrator(this)).Migrate();
        //await (new CountryAndFirstAndSecondLevelSubdivisionMigrator(this)).Migrate();
        //await (new CountrySubdivisionTypeMigratorPartThree(this)).Migrate();
        //await (new FirstAndBottomLevelSubdivisionMigrator(this)).Migrate();
        //await (new InformalIntermediateLevelSubdivisionMigrator(this)).Migrate();
        //await (new FormalIntermediateLevelSubdivisionMigrator(this)).Migrate();
        //await (new BasicSecondLevelSubdivisionMigrator(this)).Migrate();
        //await (new BlogPostMigrator(this)).Migrate();
        //await (new ArticleMigrator(this)).Migrate();
        //await (new DiscussionMigrator(this)).Migrate();
        //await (new AdoptionImportMigrator(this)).Migrate();
        //await (new DocumentMigratorPPL(this)).Migrate();
        //await (new OrganizationMigratorPPL(this)).Migrate();
        //await (new PersonMigratorPPL(this)).Migrate();
        //await (new AbuseCaseMigrator(this)).Migrate();
        //await (new ChildTraffickingCaseMigrator(this)).Migrate();
        //await (new CoercedAdoptionCaseMigrator(this)).Migrate();
        //await (new DisruptedPlacementCaseMigrator(this)).Migrate();
        //await (new DeportationCaseMigrator(this)).Migrate();
        //await (new FathersRightsViolationsCaseMigrator(this)).Migrate();
        //await (new WrongfulMedicationCaseMigrator(this)).Migrate();
        //await (new WrongfulRemovalCaseMigrator(this)).Migrate();
        //await (new LocationMigrator(this)).Migrate();
        //await (new PageMigrator(this)).Migrate();
        //await (new ReviewMigrator(this)).Migrate();
        //await (new ActMigrator(this)).Migrate();
        //await (new BillMigrator(this)).Migrate();
        //await (new NodeTermMigrator(this)).Migrate();
        //await (new MenuMigrator(this)).Migrate();
        //await (new DocumentableDocumentMigrator(this)).Migrate();
        //await (new CommentMigrator(this)).Migrate();
        //await (new TermHierarchyMigrator(this)).Migrate();
        //await (new UnitedStatesCongressionalMeetingMigrator(this)).Migrate();
        //await (new PartyPoliticalEntityRelationMigratorPPL(this)).Migrate();
        //await (new PersonOrganizationRelationMigratorPPL(this)).Migrate();
        //await (new InterOrganizationalRelationMigratorPPL(this)).Migrate();
        //await (new InterPersonalRelationMigratorPPL(this)).Migrate();
        //await (new MemberOfCongressMigrator(this)).Migrate();
        //await (new OrganizationMigratorCPCT(this)).Migrate();
        //await (new PersonMigratorCPCT(this)).Migrate();
        //await (new PersonOrganizationRelationMigratorCPCT(this)).Migrate();
        //await (new DocumentMigratorCPCT(this)).Migrate();
        //await (new InterOrganizationalRelationMigratorCPCT(this)).Migrate();
        //await (new InterPersonalRelationMigratorCPCT(this)).Migrate();
        //await (new PartyPoliticalEntityRelationMigratorCPCT(this)).Migrate();
        //await (new SearchableMigrator(this)).Migrate();
        //await (new CaseCaseRelationsMigrator(this)).Migrate();
        await (new NodeFileMigratorPPL(this)).Migrate();
        await (new NodeFileMigratorCPCT(this)).Migrate();
    }

    internal const string ConnectionStringMariaDbPPL = "server=localhost;userid=root;Password=niels;database=ppl";
    internal const string ConnectionStringMariaDbCPCT = "server=localhost;userid=root;Password=niels;database=cpct";

    internal const string ConnectStringPostgresql = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

    public static async Task<MySqlToPostgresConverter> GetInstance()
    {

        Console.Write("Setting up connections and opening readers");
        _stopwatch.Restart();
        var mysqlConnectionPPL = new MySqlConnection(ConnectionStringMariaDbPPL);
        var mysqlConnectionCPCT = new MySqlConnection(ConnectionStringMariaDbCPCT);
        var postgresConnection = new NpgsqlConnection(ConnectStringPostgresql);
        await mysqlConnectionPPL.OpenAsync();
        await mysqlConnectionCPCT.OpenAsync();
        await postgresConnection.OpenAsync();
        var nodeIdReader = await NodeIdReaderByUrlId.CreateAsync(postgresConnection);
        var termByNameableIdReader = await TermReaderByNameableId.CreateAsync(postgresConnection);
        var subdivisionReader = await SubdivisionIdReaderByName.CreateAsync(postgresConnection);
        var subdivisionReaderByIsoCode = await SubdivisionIdReaderByIso3166Code.CreateAsync(postgresConnection);
        var createNodeActionIdReaderByNodeTypeId = await CreateNodeActionIdReaderByNodeTypeId.CreateAsync(postgresConnection);
        var deleteNodeActionIdReaderByNodeTypeId = await DeleteNodeActionIdReaderByNodeTypeId.CreateAsync(postgresConnection);
        var editNodeActionIdReaderByNodeTypeId = await EditNodeActionIdReaderByNodeTypeId.CreateAsync(postgresConnection);
        var actionIdReaderByPath = await ActionIdReaderByPath.CreateAsync(postgresConnection);
        var tenantNodeIdReaderByUrlId = await TenantNodeIdReaderByUrlId.CreateAsync(postgresConnection);
        var tenantNodeReaderByUrlId = await TenantNodeReaderByUrlId.CreateAsync(postgresConnection);
        var fileIdReaderByTenantFileId = await FileIdReaderByTenantFileId.CreateAsync(postgresConnection);

        Console.WriteLine($" took {_stopwatch.ElapsedMilliseconds} ms");
        return new MySqlToPostgresConverter(
            mysqlConnectionPPL,
            mysqlConnectionCPCT,
            postgresConnection,
            nodeIdReader,
            termByNameableIdReader,
            subdivisionReader,
            subdivisionReaderByIsoCode,
            createNodeActionIdReaderByNodeTypeId,
            deleteNodeActionIdReaderByNodeTypeId,
            editNodeActionIdReaderByNodeTypeId,
            actionIdReaderByPath,
            tenantNodeIdReaderByUrlId,
            tenantNodeReaderByUrlId,
            fileIdReaderByTenantFileId);
    }
    internal MySqlConnection MysqlConnectionPPL { get; }
    internal MySqlConnection MysqlConnectionCPCT { get; }
    internal NpgsqlConnection PostgresConnection { get; }
    internal NodeIdReaderByUrlId NodeIdReader { get; }
    internal TermReaderByNameableId TermByNameableIdReader { get; }
    internal SubdivisionIdReaderByName SubdivisionIdReader { get; }
    internal SubdivisionIdReaderByIso3166Code SubdivisionIdReaderByIso3166Code { get; }
    internal CreateNodeActionIdReaderByNodeTypeId CreateNodeActionIdReaderByNodeTypeId { get; }
    internal DeleteNodeActionIdReaderByNodeTypeId DeleteNodeActionIdReaderByNodeTypeId { get; }
    internal EditNodeActionIdReaderByNodeTypeId EditNodeActionIdReaderByNodeTypeId { get; }
    internal ActionIdReaderByPath ActionIdReaderByPath { get; }
    internal TenantNodeIdReaderByUrlId TenantNodeIdByUrlIdReader { get; }
    internal TenantNodeReaderByUrlId TenantNodeByUrlIdReader { get; }

    internal FileIdReaderByTenantFileId FileIdReaderByTenantFileId { get; }
    public MySqlToPostgresConverter(
        MySqlConnection mysqlConnectionPPL,
        MySqlConnection mysqlConnectionCPCT,
        NpgsqlConnection postgresConnection,
        NodeIdReaderByUrlId nodeIdReader,
        TermReaderByNameableId termByNameableIdReader,
        SubdivisionIdReaderByName subdivisionIdReader,
        SubdivisionIdReaderByIso3166Code subdivisionIdReaderByIso3166Code,
        CreateNodeActionIdReaderByNodeTypeId createNodeActionIdReaderByNodeTypeId,
        DeleteNodeActionIdReaderByNodeTypeId deleteNodeActionIdReaderByNodeTypeId,
        EditNodeActionIdReaderByNodeTypeId editNodeActionIdReaderByNodeTypeId,
        ActionIdReaderByPath actionIdReaderByPath,
        TenantNodeIdReaderByUrlId tenantNodeIdReaderByUrlId,
        TenantNodeReaderByUrlId tenantNodeReaderByUrlId,
        FileIdReaderByTenantFileId fileIdReaderByTenantFileId
        )
    {
        MysqlConnectionPPL = mysqlConnectionPPL;
        MysqlConnectionCPCT = mysqlConnectionCPCT;
        PostgresConnection = postgresConnection;
        NodeIdReader = nodeIdReader;
        TermByNameableIdReader = termByNameableIdReader;
        SubdivisionIdReader = subdivisionIdReader;
        SubdivisionIdReaderByIso3166Code = subdivisionIdReaderByIso3166Code;
        CreateNodeActionIdReaderByNodeTypeId = createNodeActionIdReaderByNodeTypeId;
        DeleteNodeActionIdReaderByNodeTypeId = deleteNodeActionIdReaderByNodeTypeId;
        EditNodeActionIdReaderByNodeTypeId = editNodeActionIdReaderByNodeTypeId;
        ActionIdReaderByPath = actionIdReaderByPath;
        TenantNodeIdByUrlIdReader = tenantNodeIdReaderByUrlId;
        TenantNodeByUrlIdReader = tenantNodeReaderByUrlId;
        FileIdReaderByTenantFileId = fileIdReaderByTenantFileId;
    }

    private async Task TruncateDatabase()
    {
        _stopwatch.Restart();
        Console.Write("Cleaning database");
        var sql = """
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
        using var command = PostgresConnection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($" took {_stopwatch.ElapsedMilliseconds} ms");
    }

    public async ValueTask DisposeAsync()
    {
        await PostgresConnection.CloseAsync();
        await MysqlConnectionPPL.CloseAsync();
        await MysqlConnectionCPCT.CloseAsync();
        await PostgresConnection.DisposeAsync();
        await MysqlConnectionPPL.DisposeAsync();
        await MysqlConnectionCPCT.DisposeAsync();
        await NodeIdReader.DisposeAsync();
        await TermByNameableIdReader.DisposeAsync();
        await SubdivisionIdReader.DisposeAsync();
        await SubdivisionIdReaderByIso3166Code.DisposeAsync();
    }
}
