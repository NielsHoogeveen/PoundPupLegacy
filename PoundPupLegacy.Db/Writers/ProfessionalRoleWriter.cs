﻿namespace PoundPupLegacy.Db.Writers;

internal sealed class ProfessionalRoleWriter : DatabaseWriter<ProfessionalRole>, IDatabaseWriter<ProfessionalRole>
{
    private const string PERSON_ID = "person_id";
    private const string PROFESSION_ID = "profession_id";
    private const string DATERANGE = "daterange";
    public static async Task<DatabaseWriter<ProfessionalRole>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateIdentityInsertStatementAsync(
            connection,
            "professional_role",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PERSON_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PROFESSION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATERANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new ProfessionalRoleWriter(command);

    }

    internal ProfessionalRoleWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(ProfessionalRole professionalRole)
    {
        if (professionalRole.Id is null)
            throw new NullReferenceException(nameof(professionalRole.Id));
        WriteValue(professionalRole.PersonId, PERSON_ID);
        WriteValue(professionalRole.ProfessionId, PROFESSION_ID);
        WriteDateTimeRange(professionalRole.DateTimeRange, DATERANGE);
        professionalRole.Id = await _command.ExecuteScalarAsync() switch
        {
            long i => (int)i,
            _ => throw new Exception("Insert of professional role does not return an id.")
        };
    }
}
