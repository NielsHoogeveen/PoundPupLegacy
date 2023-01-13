namespace PoundPupLegacy.Convert;

internal partial class Program
{

    public static string CreateMD5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return System.Convert.ToHexString(hashBytes); 

        }
    }
    static async Task Main(string[] args)
    {
        await using var converter = await MySqlToPostgresConverter.GetInstance();
        await converter.Convert();
    }
}
