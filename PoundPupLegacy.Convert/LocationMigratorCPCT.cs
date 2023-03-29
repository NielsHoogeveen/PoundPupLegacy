namespace PoundPupLegacy.Convert;

internal sealed class LocationMigratorCPCT : CPCTMigrator
{
    public LocationMigratorCPCT(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "locations (cpct)";
    protected override async Task MigrateImpl()
    {
        await LocationCreator.CreateAsync(ReadLocations(), _postgresConnection);
    }

    private static string? GetStreet(int id, string? street)
    {
        return id switch {
            _ => street
        };
    }
    private static string? GetAdditional(int id, string? additional)
    {
        return id switch {
            _ => additional
        };
    }
    private static string? GetPostalCode(int id, string? postalCode)
    {
        return id switch {
            _ => postalCode
        };
    }

    private static decimal? GetLatitude(int id, decimal? longitude)
    {
        return id switch {
            _ => longitude
        };
    }
    private static decimal? GetLongitude(int id, decimal? lattitude)
    {
        return id switch {
            _ => lattitude
        };
    }
    private static string? GetCity(int id, string? city)
    {
        return id switch {
            2914 => "Den Haag",
            2916 => "Den Haag",
            2924 => "Den Haag",
            2946 => "Den Haag",
            2947 => "Den Haag",
            2957 => "Den Haag",
            2949 => "Amsterdam",
            2832 => "Champsecret",
            2921 => null,
            2848 => "Dublin",
            2870 => "Tylstrup",
            2886 => "Jakarta",
            2842 => "Kathmandu",
            2867 => "Thiruvanmiyur",
            2951 => "Bangalore",
            2829 => "Accra",
            2893 => "Accra",
            2840 => "Addis Ababa",
            2882 => "Addis Ababa",
            2883 => "Addis Ababa",
            _ => city
        };
    }

    private async Task<int?> GetSubdivisionId(int id, int? stateId, int? countryId, string? code)
    {
        if (countryId is null) {
            return null;
        }

        var res = await GetSubdivisionId(id, stateId);
        if (res != null) {
            return res;
        }
        var stateCode = code switch {
            "HK-HWC" => null,
            "HK-HCW" => null,
            "HK-KYT" => null,
            "GB-JSY" => null,
            "CZ-JM" => "CZ-643",
            "RO-DI" => "RO-DB",
            "RO-HA" => "RO-HR",
            "UA-LV" => "UA-46",
            "UA-KV" => "UA-32",
            "UA-KL" => "UA-63",
            "UA-LU" => "UA-09",
            "GT-GU" => "GT-01",
            "GT-QZ" => "GT-09",
            "CR-AL" => "CR-A",
            "UK-KL" => "UA-63",
            "CN-43" => "CN-HN",
            "CN-11" => "CN-BJ",
            "CN-52" => "CN-GZ",
            "CN-36" => "CN-FJ",
            "CN-41" => "CN-HA",
            "CN-13" => "CN-HE",
            "CN-44" => "CN-GD",
            "CN-61" => "CN-SN",
            "VN-BT" => "VN-40",
            "VN-HC" => "VN-SG",
            _ => code
        };
        if (stateCode == null) {
            return null;
        }
        return await _subdivisionIdReaderByIso3166Code.ReadAsync(stateCode);
    }


    private async Task<int?> GetSubdivisionId(int id, int? stateId)
    {
        if (stateId == null) {
            var stateCode = id switch {
                2941 => "AU-QLD",
                2954 => "AU-NSW",
                2856 => "RU-SPE",
                2905 => "RU-MOW",
                2805 => "RO-BV",
                2833 => "RO-B",
                2834 => "RO-B",
                2929 => "RO-B",
                2854 => "BG-23",
                2855 => "BG-18",
                2925 => "BG-16",
                2814 => "CH-BE",
                2858 => "CH-GE",
                2859 => "CH-GE",
                2861 => "CH-GE",
                2928 => "CH-GE",
                2791 => "NL-NH",
                2804 => "NL-ZH",
                2885 => "NL-ZH",
                2914 => "NL-ZH",
                2916 => "NL-ZH",
                2896 => "NL-GE",
                2918 => "NL-GE",
                2924 => "NL-ZH",
                2940 => "NL-NB",
                2946 => "NL-ZH",
                2947 => "NL-ZH",
                2948 => "NL-LI",
                2949 => "NL-NH",
                2950 => "NL-OV",
                2957 => "NL-ZH",
                2958 => "NL-GE",
                2794 => "DE-BY",
                2839 => "DE-BY",
                2849 => "DE-BW",
                2903 => "DE-BW",
                2923 => "DE-BE",
                2943 => "DE-BW",
                2823 => "FR-6AE",
                2832 => "FR-61",
                2910 => "FR-6AE",
                2919 => "FR-75C",
                2930 => "FR-75C",
                2935 => "FR-6AE",
                2809 => "BE-BRU",
                2917 => "BE-BRU",
                2926 => "BE-BRU",
                2927 => "BE-BRU",
                2931 => "BE-BRU",
                2932 => "BE-BRU",
                2933 => "BE-BRU",
                2936 => "BE-BRU",
                2937 => "BE-BRU",
                2944 => "BE-VOV",
                2934 => "IT-MI",
                2820 => "GB-ESX",
                2826 => "GB-LND",
                2827 => "GB-WIL",
                2828 => "GB-LND",
                2845 => "GB-LND",
                2852 => "GB-BIR",
                2921 => "GB-WIL",
                2938 => "GB-LND",
                2952 => "GB-LND",
                2848 => "IE-D",
                2872 => "IE-D",
                2904 => "IE-D",
                2857 => "DK-82",
                2870 => "DK-81",
                2908 => "VN-DN",
                2884 => "ID-SS",
                2886 => "ID-JK",
                2887 => "ID-JK",
                2842 => "NP-P3",
                2871 => "NP-P3",
                2898 => "NP-P3",
                2906 => "NP-P3",
                2837 => "IN-MH",
                2864 => "IN-TN",
                2865 => "IN-MH",
                2866 => "IN-TN",
                2867 => "IN-TN",
                2873 => "IN-MH",
                2874 => "IN-TN",
                2875 => "IN-MH",
                2876 => "IN-MH",
                2880 => "IN-MH",
                2889 => "IN-MH",
                2899 => "IN-MH",
                2900 => "IN-MH",
                2901 => "IN-MH",
                2902 => "IN-MH",
                2907 => "IN-MH",
                2922 => "IN-AP",
                2939 => "IN-MH",
                2951 => "IN-KA",
                2956 => "IN-WB",
                2802 => "KG-GB",
                2838 => "AE-DU",
                2829 => "GH-AA",
                2893 => "GH-AA",
                2909 => "MA-TNG",
                2913 => "UG-204",
                2798 => "KE-30",
                2799 => "KE-30",
                2840 => "ET-AA",
                2882 => "ET-AA",
                2883 => "ET-AA",
                2945 => "BE-WHT",
                2841 => "US-NY",
                2881 => "US-OK",
                2953 => "US-CO",
                2920 => "AR-C",
                2806 => "HT-OU",
                2807 => "HT-OU",
                2808 => "HT-OU",
                2810 => "HT-OU",
                2812 => "HT-OU",
                2813 => "HT-OU",
                2815 => "HT-OU",
                2817 => "HT-OU",
                2803 => "CA-BC",
                _ => null
            };
            if (stateCode == null) {
                return null;
            }
            return await _subdivisionIdReaderByIso3166Code.ReadAsync(stateCode);
        }
        else {
            var ret = id switch {
                _ => stateId
            };
            if (ret == null) {
                return null;
            }
            else {
                return await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = (int)ret!
                });
            }
        }
    }
    private async Task<int?> GetCountryId(int id, int? countryId)
    {
        var ret = id switch {
            2945 => 4017,
            _ => countryId
        };
        if (ret == null) {
            return null;
        }
        else {
            return await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = (int)ret!
            });
        }
    }



    private async IAsyncEnumerable<Location> ReadLocations()
    {
        var sql = $"""
            SELECT 
                l.lid id,
                n.nid node_id,
                n.title title,
                case when l.street = '' then NULL ELSE l.street END street,
                l.additional,
                case when l.city = '' then NULL ELSE l.city END city,
                l.postal_code,
                case 
            		when l.province = '' then null
            		when l.province = 'xx'  then null
            		ELSE  concat(upper(c.field_country_code_value), '-',l.province)
            	END subdivision_code,
                s.nid subdivision_id,
                c.nid country_id,
                l.latitude,
                l.longitude
                FROM location l
                JOIN node_revisions nr on nr.vid = l.eid
                JOIN node n ON n.nid = nr.nid AND n.vid = nr.vid
                JOIN content_type_country_type c ON lower(c.field_country_code_value) = lower(l.country)
                JOIN node n2 ON n2.nid = c.nid
                LEFT JOIN content_type_statefact s ON s.field_country_1_nid = c.nid AND s.field_statecode_value = l.province
                WHERE n.`type` NOT IN ('content_organisations', 'prisons', 'prisonpup')
                AND n.nid NOT IN (11108, 7760, 12700, 30638, 38588 )
                AND l.lid > 2790
            """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var renamedId = id + 4175 - 2790;
            int? subDivisionId = reader.IsDBNull("subdivision_id") ? null : reader.GetInt32("subdivision_id");
            string? code = reader.IsDBNull("subdivision_code") ? null : reader.GetString("subdivision_code").Replace("UK-", "GB-");
            int? countryId = reader.IsDBNull("country_id") ? null : reader.GetInt32("country_id");
            var (urlId, tenantId) = GetUrlIdAndTenant(reader.GetInt32("node_id"));
            var locatableId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest {
                TenantId = tenantId,
                UrlId = urlId
            });
            yield return new Location {
                Id = renamedId,
                Street = GetStreet(id, reader.IsDBNull("street") ? null : reader.GetString("street")),
                Additional = GetAdditional(id, reader.IsDBNull("additional") ? null : reader.GetString("additional")),
                City = GetCity(id, reader.IsDBNull("city") ? null : reader.GetString("city")),
                PostalCode = GetPostalCode(id, reader.IsDBNull("postal_code") ? null : reader.GetString("postal_code")),
                SubdivisionId = await GetSubdivisionId(id, subDivisionId, countryId, code),
                CountryId = await GetCountryId(id, countryId),
                Latitude = GetLatitude(id, reader.IsDBNull("latitude") ? null : reader.GetDecimal("latitude")),
                Longitude = GetLongitude(id, reader.IsDBNull("longitude") ? null : reader.GetDecimal("longitude")),
                Locatables = new List<LocationLocatable> { new LocationLocatable { LocationId = renamedId, LocatableId = locatableId } }
            };

        }
        await reader.CloseAsync();
    }
}
