namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class SubdivisionTypeWriter : IDatabaseWriter<SubdivisionType>
{
    public static async Task<DatabaseWriter<SubdivisionType>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<SubdivisionType>("subdivision_type", connection);
    }
}
