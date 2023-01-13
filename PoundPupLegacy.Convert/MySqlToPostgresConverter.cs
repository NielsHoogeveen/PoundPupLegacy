using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Convert;

internal partial class MySqlToPostgresConverter: IAsyncDisposable
{

    public async Task Convert()
    {
        await TruncateDatabase();
        await (new PublicationStatusMigrator(this)).Migrate();
        await (new FileMigrator(this)).Migrate();
        await (new NodeTypeMigrator(this)).Migrate();
        await (new UserMigrator(this)).Migrate();
        await (new VocabularyMigrator(this)).Migrate();
        await (new BasicNameableMigrator(this)).Migrate();
        await (new ChildPlacementTypeMigrator(this)).Migrate();
        await (new OrganizationTypeMigrator(this)).Migrate();
        await (new InterOrganizationalRelationTypeMigrator(this)).Migrate();
        await (new InterPersonalRelationTypeMigrator(this)).Migrate();
        await (new PoliticalEnitityRelationTypeMigrator(this)).Migrate();
        await (new PersonOrganizationRelationTypeMigrator(this)).Migrate();
        await (new TypeOfAbuseMigrator(this)).Migrate();
        await (new TypeOfAbuserMigrator(this)).Migrate();
        await (new FamilySizeMigrator(this)).Migrate();
        await (new ProfessionMigrator(this)).Migrate();
        await (new DenominationMigrator(this)).Migrate();
        await (new HagueStatusMigrator(this)).Migrate();
        await (new DocumentTypeMigrator(this)).Migrate();
        await (new FirstLevelGlobalRegionMigrator(this)).Migrate();
        await (new SecondLevelGlobalRegionMigrator(this)).Migrate();
        await (new BasicCountryMigrator(this)).Migrate();
        await (new BindingCountryMigrator(this)).Migrate();
        await (new BoundCountryMigrator(this)).Migrate();
        await (new CountryAndFirstLevelSubDivisionMigrator(this)).Migrate();
        await (new CountryAndFirstAndSecondLevelSubdivisionMigrator(this)).Migrate();
        await (new FirstAndBottomLevelSubdivisionMigrator(this)).Migrate();
        await (new InformalIntermediateLevelSubdivisionMigrator(this)).Migrate();
        await (new FormalIntermediateLevelSubdivisionMigrator(this)).Migrate();
        await (new BasicSecondLevelSubdivisionMigrator(this)).Migrate();
        await (new BlogPostMigrator(this)).Migrate();
        await (new ArticleMigrator(this)).Migrate();
        await (new DiscussionMigrator(this)).Migrate();
        await (new AdoptionExportMigrator(this)).Migrate();
        await (new DocumentMigrator(this)).Migrate();
        await (new OrganizationMigrator(this)).Migrate();
        await (new PersonMigrator(this)).Migrate();
        await (new AbuseCaseMigrator(this)).Migrate();
        await (new ChildTraffickingCaseMigrator(this)).Migrate();
        await (new CoercedAdoptionCaseMigrator(this)).Migrate();
        await (new DisruptedPlacementCaseMigrator(this)).Migrate();
        await (new DeportationCaseMigrator(this)).Migrate();
        await (new FathersRightsViolationsCaseMigrator(this)).Migrate();
        await (new WrongfulMedicationCaseMigrator(this)).Migrate();
        await (new WrongfulRemovalCaseMigrator(this)).Migrate();
        await (new LocationMigrator(this)).Migrate();
        await (new PageMigrator(this)).Migrate();
        await (new ReviewMigrator(this)).Migrate();
        await (new NodeTermMigrator(this)).Migrate();
        await (new CommentMigrator(this)).Migrate();

    }

    internal const string ConnectionStringMariaDb = "server=localhost;userid=root;Password=niels;database=ppldb";

    internal const string ConnectStringPostgresql = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

    public static async Task<MySqlToPostgresConverter> GetInstance()
    {
        var mysqlConnection = new MySqlConnection(ConnectionStringMariaDb);
        var postgresConnection = new NpgsqlConnection(ConnectStringPostgresql);
        await mysqlConnection.OpenAsync();
        await postgresConnection.OpenAsync();
        var nodeIdReader = await NodeIdByUrlIdReader.CreateAsync(postgresConnection);
        var termByNameableIdReader = await TermReaderByNameableId.CreateAsync(postgresConnection);
        var subdivisionReader = await SubdivisionIdReaderByName.CreateAsync(postgresConnection);
        var subdivisionReaderByIsoCode = await SubdivisionIdReaderByIso3166Code.CreateAsync(postgresConnection);
        return new MySqlToPostgresConverter(mysqlConnection, postgresConnection, nodeIdReader, termByNameableIdReader, subdivisionReader, subdivisionReaderByIsoCode);
    }
    internal MySqlConnection MysqlConnection { get; }
    internal NpgsqlConnection PostgresConnection { get; }
    internal NodeIdByUrlIdReader NodeIdReader { get; }
    internal TermReaderByNameableId TermByNameableIdReader { get; }
    internal SubdivisionIdReaderByName SubdivisionIdReader { get; }
    internal SubdivisionIdReaderByIso3166Code SubdivisionIdReaderByIso3166Code { get; }
    public MySqlToPostgresConverter(MySqlConnection mysqlConnection, NpgsqlConnection postgresConnection, NodeIdByUrlIdReader nodeIdReader, TermReaderByNameableId termByNameableIdReader, SubdivisionIdReaderByName subdivisionIdReader, SubdivisionIdReaderByIso3166Code subdivisionIdReaderByIso3166Code)
    {
        MysqlConnection = mysqlConnection;
        PostgresConnection = postgresConnection;
        NodeIdReader = nodeIdReader;
        TermByNameableIdReader = termByNameableIdReader;
        SubdivisionIdReader = subdivisionIdReader;
        SubdivisionIdReaderByIso3166Code = subdivisionIdReaderByIso3166Code;
    }

    private async Task TruncateDatabase()
    {
        var sql = """
            TRUNCATE publication_status
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
            """;
        using var command = PostgresConnection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await PostgresConnection.CloseAsync();
        await MysqlConnection.CloseAsync();
        await PostgresConnection.DisposeAsync();
        await MysqlConnection.DisposeAsync();
        await NodeIdReader.DisposeAsync();
        await TermByNameableIdReader.DisposeAsync();
        await SubdivisionIdReader.DisposeAsync();
        await SubdivisionIdReaderByIso3166Code.DisposeAsync();
    }
}
