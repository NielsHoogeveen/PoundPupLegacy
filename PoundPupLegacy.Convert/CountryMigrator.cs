namespace PoundPupLegacy.Convert;
internal abstract class CountryMigrator(
        IDatabaseConnections databaseConnections
    ) : MigratorPPL(databaseConnections)
{
    protected static string GetISO3166Code2ForCountry(int id)
    {
        return id switch {
            3891 => "NL-AW",
            3914 => "US-PR",
            3920 => "US-VI",
            3980 => "CN-HK",
            3981 => "CN-MO",
            4048 => "US-GU",
            4053 => "US-MP",
            4055 => "US-AS",

            3878 => "FR-YT",
            3879 => "FR-RE",
            3887 => "FR-PM",
            3903 => "FR-GP",
            3908 => "FR-MQ",
            3935 => "FR-GF",
            4044 => "FR-NC",
            4057 => "FR-PF",
            4063 => "FR-WF",

            11570 => "GB-ENG",
            11569 => "GB-SCT",
            11571 => "GB-WLS",
            _ => throw new Exception($"No ISO3166-2 code is defined for {id}")
        };
    }
    protected static int GetSupervisingCountryId(int id)
    {
        return id switch {
            3891 => 4023,
            3914 => 3805,
            3920 => 3805,
            3980 => 3975,
            3981 => 3975,
            4048 => 3805,
            4053 => 3805,
            4055 => 3805,

            3878 => 4018,
            3879 => 4018,
            3887 => 4018,
            3903 => 4018,
            3908 => 4018,
            3935 => 4018,
            4044 => 4018,
            4057 => 4018,
            4063 => 4018,
            _ => throw new Exception($"No supervising country is defined for {id}")
        };
    }

}
