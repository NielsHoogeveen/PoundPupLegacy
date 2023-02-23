namespace PoundPupLegacy.Convert;

internal partial class Program
{
    static async Task Main(string[] args)
    {
        await using var converter = await MySqlToPostgresConverter.GetInstance();
        await converter.Convert();
    }

}
