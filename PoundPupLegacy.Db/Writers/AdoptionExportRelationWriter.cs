using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class AdoptionExportRelationWriter : DatabaseWriter<AdoptionExportRelation>, IDatabaseWriter<AdoptionExportRelation>
{

    private const string COUNTRY_ID_TO = "country_id_to";
    private const string COUNTRY_ID_FROM = "country_id_from";
    private const string COUNTRY_NAME_FROM = "country_name_from";
    public static DatabaseWriter<AdoptionExportRelation> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "adoption_export_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = COUNTRY_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_NAME_FROM,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new AdoptionExportRelationWriter(command);

    }

    internal AdoptionExportRelationWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(AdoptionExportRelation adoptionExportRelation)
    {
        WriteValue(adoptionExportRelation.CountryIdTo, COUNTRY_ID_TO);
        WriteNullableValue(adoptionExportRelation.CountryIdFrom, COUNTRY_ID_FROM);
        WriteNullableValue(adoptionExportRelation.CountryNameFrom, COUNTRY_NAME_FROM);
        _command.ExecuteNonQuery();
    }
}
