using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class AdoptionImportMigrator : PPLMigrator
{
    public AdoptionImportMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    private interface AdoptionImports
    {
        public int CountryIdTo { get; }

        public int Amount { get; }

        public int Year { get; }

    }

    private struct SpecificAdoptionImports : AdoptionImports
    {
        public required int CountryIdTo { get; init; }
        public required int CountryIdFrom { get; init; }
        public required int Year { get; init; }
        public required int Amount { get; init; }
    }
    private struct NonSpecificAdoptionImports : AdoptionImports
    {
        public required int CountryIdTo { get; init; }
        public required int Year { get; init; }
        public required int Amount { get; init; }
    }
    private async IAsyncEnumerable<AdoptionImports> AdoptionImportCsvFiles()
    {
        foreach (var f in new DirectoryInfo(@"..\..\..\files\imports").EnumerateFiles().Where(x => x.Extension == ".csv"))
        {
            Console.WriteLine($"Processing file {f.Name}");
            if (int.TryParse(f.Name.Substring(0, f.Name.Length - 4), out var countryIdTo))
            {

                var years = new List<int>();
                await foreach (string line in System.IO.File.ReadLinesAsync(f.FullName))
                {
                    if (!years.Any())
                    {
                        years.AddRange(line.Split(';').Skip(2).Select(x => int.Parse(x)));
                        continue;
                    }
                    var parts = line.Split(new char[] { ';' });
                    for (int i = 2; i < parts.Length; i++)
                    {

                        if (!string.IsNullOrEmpty(parts[i]))
                        {
                            var amount = int.Parse(parts[i]);
                            if (!string.IsNullOrEmpty(parts[0]))
                            {
                                var countryFromId = int.Parse(parts[0]);
                                yield return new SpecificAdoptionImports
                                {
                                    CountryIdFrom = countryFromId,
                                    Amount = amount,
                                    CountryIdTo = countryIdTo,
                                    Year = years[i - 2]
                                };

                            }
                            else
                            {
                                yield return new NonSpecificAdoptionImports
                                {
                                    Amount = amount,
                                    CountryIdTo = countryIdTo,
                                    Year = years[i - 2]
                                };

                            }
                        }
                    }


                }
            }

        }

    }

    protected override string Name => "adoption imports";

    protected override async Task MigrateImpl()
    {
        await using var nodeReader = await NodeReaderByUrlI.CreateAsync(_postgresConnection);

        var x = AdoptionImportCsvFiles()
            .OfType<SpecificAdoptionImports>()
            .Select(async x => await GetInterCountryRelation(x.CountryIdFrom, x.CountryIdTo, x.Year, x.Amount, nodeReader))
            .Select(y => y.Result);


        var r = ReadAdoptionExportYears(nodeReader);

        await InterCountryRelationCreator.CreateAsync(r, _postgresConnection);
        await InterCountryRelationCreator.CreateAsync(x, _postgresConnection);

        var cmd = _postgresConnection.CreateCommand();
        cmd.CommandText = $"""
            insert into country_report(country_id, date_range, number_of_children_imported, number_of_children_imported_of_unknown_origin)
            select
            	icr.country_id_from,
            	icr.date_range,
            	SUM(icr.number_of_children_involved),
            	0
            from inter_country_relation icr
            join tenant_node tn on tn.url_id = {Constants.ADOPTION_IMPORT} and tn.tenant_id = 1
            group by icr.country_id_from, icr.date_range
            """;
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task<InterCountryRelation> GetInterCountryRelation(int countryIdFrom, int countryIdTo, int year, int numberOfChildren, NodeReaderByUrlI nodeReader)
    {
        var nodeFrom = await nodeReader.ReadAsync(Constants.PPL, countryIdFrom);
        var nodeTo = await nodeReader.ReadAsync(Constants.PPL, countryIdTo);

        var title = $"Adoption exports from {nodeFrom.Title} to {nodeTo.Title} in {year}";

        return new InterCountryRelation
        {
            //The relation is about imports so the relation from is the receiving party
            //and relation to is the sending party
            //even though the children go from the sending party to the receiving party
            CountryIdFrom = (int)nodeTo.Id,
            CountryIdTo = (int)nodeFrom.Id,
            DateTimeRange = GetDateTimeRange(countryIdTo, year),
            Title = title,
            OwnerId = Constants.OWNER_GEOGRAPHY,
            PublisherId = 1,
            ChangedDateTime = DateTime.Now,
            CreatedDateTime = DateTime.Now,
            DocumentIdProof = null,
            Id = null,
            InterCountryRelationTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, Constants.ADOPTION_IMPORT),
            NodeTypeId = 50,
            MoneyInvolved = 0,
            NumberOfChildrenInvolved = numberOfChildren,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
        };
    }
    private async IAsyncEnumerable<InterCountryRelation> ReadAdoptionExportYears(NodeReaderByUrlI nodeReader)
    {

        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                    DISTINCT
                    n.nid country_id_to,
                    country_id country_id_from,
                    country_name country_name_from,
                    m.col + 1998 `year`,
                    cast(m.`value` AS INT) number_of_children
                    FROM(
                    SELECT 
                    n.nid,
                    n.vid,
                    n.`row`,
                    n.country_id,
                    case when country_id IS NULL then country_name END country_name
                    FROM
                    (
                    SELECT DISTINCT
                    n.nid,
                    n.vid,
                    m.`row`,
                    case 
                        when m.value = '<a href="/laos">Laos</a>' then 3967
                        when m.value = '<a href="/guyana">Guyana</a>' then 3927
                        when m.value = '<a href="/burma">Burma</a>' then 3969
                        when m.value = '<a href="/myamar">Myamar</a>' then 3969
                        when m.value = '<a href="/domican_repubic">Domican Repubic</a>' then 3900
                        when m.value = 'Palestine Authority' then 4082
                        when m.value = 'Palestine' then 4082
                        when m.value = '<a href="/palestine_authorities">Palestine Authorities</a>' then 4082
                        when m.value = 'Gaza Strip' then 4082
                        when m.value = '<a href="/tunesia">Tunesia</a>' then 3834
                        when m.value = '<a href="/bosnia_herzigovina">Bosnia and Herzegovina Federation</a>' then 3999
                        when m.value = '<a href="/united kingdom">United Kingdom</a>' then 6185
                        when m.value = '<a href="/cote_ivoir">Côte d’Ivoire</a>' then 3839
                        when m.value = '<a href="/cote_ivoir">Cote d\'Ivoire</a>' then 3839
                        when m.value = '<a href="/laos">Laos</a>' then 3967
                        when m.value = '<a href="/morrocco">Morrocco</a>' then 3832
                        when m.value = '<a href="/guyana">Guyana</a>' then 3927
                        when m.value = '<a href="/antigua_and_barbuda ">Antigua and Barbuda</a>' then 4073
                        when m.value = '<a href="/antigua_and_barbuda">Antigua and Barbuda</a>' then 4073
                        when m.value = '<a href="czeck_republic">Czeck Replublic</a>' then 4029
                        when m.value = '<a href="/surinam">Surinam</a>' then 3930
                        when m.value = '<a href="/node/3857">Democratic Republic of The Congo</a>' then 3857
                        when m.value = '<a href="/mauretania">Mauretania</a>' then 3846
                        when m.value = '<a href="/marhsall_islands">Marshall Islands</a>' then 4050
                        when m.value = '<a href="/kyrgystan">Kyrgystan</a>' then 3952
                        when m.value = '<a href="/sao_tome_et_principe">Sao Tome et Principe</a>' then 3860
                        when m.value = '<a href="/sao_tome_e_pricipe">Sao Tome e Pricipe</a>' then 3860
                        when m.value = '<a href="/comores">Comores</a>' then 3863
                        when m.value = '<a href="/timor-leste">Timor-Leste</a>' then 3973
                        when m.value = '<a href="/new_zealan">New Zealand</a>' then 4039
                        when m.value = '<a href="/russia">Russia</a>' then 4034
                        when m.value = '<a href="/tadjikistan">Tadjikistan</a>' then 3954
                        when m.value = '<a href="/saint_barthelemy">St. Barthelemy</a>' then 4097
                        when m.value = '<a href="/south_sudan">South Sudan</a>' then 4093
                        when m.value = '<a href="/guinea_bissau">Guinea-Bissau</a>' then 3843
                        when m.value = '<a href="/guinea_bissau">Guinea Bissau</a>' then 3843
                        when m.value = '<a href="/guinea">Guinea</a>' then 3842
                        when m.value = '<a href="/equitorial_guinea">Equitorial Guinea</a>' then 3858
                        when m.value = '<a href="/equatorial_guinea">Equatorial Guinea</a>' then 3858
                        when m.value = '<a href="/ecuatorial_guinea">Ecuatorial Guinea</a>' then 3858
                        when m.value = '<a href="/papua_new_guinea">Papua New Guinea</a>' then 4045
                        when m.value = '<a href="/niger">Niger</a>' then 3847
                        when m.value = '<a href="/nigeria">Nigeria</a>' then 3848
                        when m.value = '<a href="/solvakia">Slovakia</a>' then 4035
                        when m.value = '<a href="/slovakia">Slovakia</a>' then 4035
                        when m.value = '<a href="/slovakia">Slovakia</a>' then 4035
                        when m.value = '<a href="/democratic_republic_of_the_congo">Democratic Republic of the Congo</a>' then 3857
                        when m.value = '<a href="/democratic_republic_of_the_congo">Democratic Republic of the Congo</a>' then 3857
                        when m.value = '<a href="/republic_of_the_congo">Republic of The Congo (Brazzaville)</a>' then 3856
                        when m.value = '<a href="/republic_of_the_congo">Republic of The Congo (Brazzaville)</a>' then 3856
                        when m.value = '<a href="/dominican_repubic">Dominican Repubic</a>' then 3899
                        when m.value = '<a href="/dominican_republic">Dominican Republic</a>' then 3899
                        when m.value = '<a href="/dominica">Dominica</a>' then 3900
                        when m.value = '<a href="/dominca">Dominica</a>' then 3900
                        when m.value = '<a href="/somalia">Somalia</a>' then 3867
                        when m.value = '<a href="/slovakia">Algeria</a>' then 3829
                        when a.src IS NOT NULL then CAST(SUBSTR(a.src, 6, 6) AS INT)
                        ELSE NULL 
                    END country_id,
                    case when a.src IS NULL then m.value ELSE NULL END country_name
                    FROM node n
                    JOIN node_field_matrix_data m ON m.nid = n.nid AND m.vid = n.vid AND m.field_name = 'field_adoption_imports'
                    LEFT JOIN url_alias a ON a.dst = SUBSTR(m.value, LOCATE(a.dst,m.value),LENGTH(a.dst))
                    WHERE m.col = 1
                    ) AS n
                    LEFT JOIN node n2 ON n2.nid = country_id AND n2.`type` = 'country_type'
                    WHERE (n.country_id IS NOT NULL AND n2.nid IS NOT NULL) OR (country_id IS NULL AND n2.nid IS NULL) 
                    ) AS n
                    JOIN node_field_matrix_data m ON m.nid = n.nid AND m.vid = n.vid AND m.`row` = n.`row` AND m.field_name = 'field_adoption_imports'
                    WHERE m.col <> 1 AND n.nid NOT IN (3985) 
                ) x
                WHERE country_name_from IS NULL #AND country_name_from <> 'Middle East
                AND NOT (country_id_to = 4023 AND country_id_from = 3936 AND `year` = 2009)
                AND  NOT(country_id_to = 4018 AND `year` = 2017)
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var year = reader.GetInt32("year");

            yield return await GetInterCountryRelation(
                reader.GetInt32("country_id_from"),
                reader.GetInt32("country_id_to"),
                reader.GetInt32("year"),
                reader.GetInt32("number_of_children"),
                nodeReader
                );
        }
        await reader.CloseAsync();
    }

    private static DateTimeRange GetDateTimeRange(int countryId, int year)
    {
        return countryId switch
        {
            3805 => new DateTimeRange(DateTime.Parse($"{year - 1}-10-01"), DateTime.Parse($"{year}-09-30")),
            4038 => new DateTimeRange(DateTime.Parse($"{year - 1}-07-01"), DateTime.Parse($"{year}-06-30")),
            _ => new DateTimeRange(DateTime.Parse($"{year}-01-01"), DateTime.Parse($"{year}-12-31")),
        };
    }
}
