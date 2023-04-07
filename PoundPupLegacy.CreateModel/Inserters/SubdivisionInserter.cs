using System.Xml.Linq;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SubdivisionInserterFactory : DatabaseInserterFactory<Subdivision>
{
    public override async Task<IDatabaseInserter<Subdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = SubdivisionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SubdivisionInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = SubdivisionInserter.COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SubdivisionInserter.SUBDIVISION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new SubdivisionInserter(command);
    }
}
internal sealed class SubdivisionInserter : DatabaseInserter<Subdivision>
{
    internal const string ID = "id";
    internal const string NAME = "name";
    internal const string COUNTRY_ID = "country_id";
    internal const string SUBDIVISION_TYPE_ID = "subdivision_type_id";


    internal SubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Subdivision subdivision)
    {
        if (subdivision.Id is null)
            throw new NullReferenceException();
        try {
            SetParameter(subdivision.Id, ID);
            SetParameter(subdivision.Name.Trim(), NAME);
            SetParameter(subdivision.CountryId, COUNTRY_ID);
            SetParameter(subdivision.SubdivisionTypeId, SUBDIVISION_TYPE_ID);
            await _command.ExecuteNonQueryAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }
}
