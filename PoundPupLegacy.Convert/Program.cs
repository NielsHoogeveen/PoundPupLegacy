namespace PoundPupLegacy.Convert;

internal partial class Program
{
    static async Task Main(string[] args)
    {
        var converter = new MySqlToPostgresConverter();
        await converter.Convert();
    }

}
