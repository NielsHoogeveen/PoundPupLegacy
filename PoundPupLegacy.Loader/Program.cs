using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
namespace PoundPupLegacy.Loader;

internal class Program
{
    static async Task Main(string[] args)
    {
        await LoadUSCities();
    }
   
    static async Task LoadUSCounties()
    {
       ServiceCollection services  = new ServiceCollection();
        services.AddDomainModelAccessors();
        var sp = services.BuildServiceProvider();
        var connectionString1 = "Host=85.10.148.88;Username=niels;Password=jagath67ag;Database=ppl";
        await using var connection1 = new NpgsqlConnection(connectionString1);
        await connection1.OpenAsync();

        var connectionString2 = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=true";
        await using var connection2 = new NpgsqlConnection(connectionString2);
        await connection2.OpenAsync();

        var factory = sp.GetRequiredService<IEntityCreatorFactory<UnitedStatesCounty.ToCreate>>();
        var creator = await factory.CreateAsync(connection1);
        foreach (var elem in await ReadUSCounties(connection2).ToListAsync()) {
            Console.WriteLine($"Writing {elem.NodeDetails.Title}");
            var tx = await connection1.BeginTransactionAsync();
            try {
                await creator.CreateAsync(elem);
                await tx.CommitAsync();
            }
            finally { 
            }
        }
    }
    static async Task LoadUSCities()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddDomainModelAccessors();
        var sp = services.BuildServiceProvider();
        var connectionString1 = "Host=85.10.148.88;Username=niels;Password=jagath67ag;Database=ppl";
        await using var connection1 = new NpgsqlConnection(connectionString1);
        await connection1.OpenAsync();

        var connectionString2 = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=true";
        await using var connection2 = new NpgsqlConnection(connectionString2);
        await connection2.OpenAsync();

        var factory = sp.GetRequiredService<IEntityCreatorFactory<UnitedStatesCity.ToCreate>>();
        var creator = await factory.CreateAsync(connection1);
        await creator.CreateAsync(ReadUSCities(connection2));
    }

    private record City
    {
        public required string CityName { get; init; }
        public required decimal Latitude { get; init; }
        public required decimal Longitude { get; init; }
        public required int Population { get; init; }
        public required double Density { get; init; }
        public required bool Military { get; init; }
        public required bool Incorporated { get; init; }
        public required string Timezone { get; init; }
        public required int CountyId { get; init; }
        public required int TermId { get; init; }
        public required string SimpleName { get; init; }
    }

    private UnitedStatesCity.ToCreate Create(City city)
    {
        
        return new UnitedStatesCity.ToCreate {
            Identification = new Common.Identification.Possible { Id = null },
            NameableDetails = new NameableDetails.ForCreate {
                Description = city.CityName,
                FileIdTileImage = null,
                Terms = new List<Term.ToCreateForNewNameable> {
                        new Term.ToCreateForNewNameable {
                            Identification = new Common.Identification.Possible {
                                Id = null
                            },
                            Name = city.CityName,
                            ParentTermIds = new List<int>{ city.TermId },
                            VocabularyId = 100000
                        }
                    },
            },
            NodeDetails = new NodeDetails.ForCreate {
                AuthoringStatusId = 1,
                FilesToAdd = new(),
                ChangedDateTime = DateTime.Now.AddDays(-100),
                CreatedDateTime = DateTime.Now.AddDays(-100),
                NodeTypeId = 70,
                PublisherId = 1,
                Title = city.CityName,
                OwnerId = 1,
                TenantNodes = new List<TenantNode.ToCreate.ForNewNode> {
                        new TenantNode.ToCreate.ForNewNode {
                            Identification = new Common.Identification.Possible {
                                Id= null
                            },
                            PublicationStatusId = 1,
                            SubgroupId = null,
                            TenantId = 1,
                            UrlId = null,
                        }
                    },
                TermIds = new(),
            },
            Latitude = city.Latitude,
            Longitude = city.Longitude,
            Population = city.Population,
            Density = city.Density,
            Military = city.Military,
            Incorporated = city.Incorporated,
            Timezone = city.Timezone,
            UnitedStatesCountyId = city.CountyId,
            SimpleName = city.SimpleName,
        };
    }


    static async IAsyncEnumerable<UnitedStatesCity.ToCreate> ReadUSCities(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT
            DISTINCT
            case
                when cs.city = 'New York' then 'New York City'
                when cs2 is null then cs.city || ' - '|| cs.state_id
                else cs.city || ' - ' || cs.county_name || ' county '|| cs.state_id
            end "name",
            cs.lat latitude,
            cs.lon longitude,
            cs.population,
            cs.density,
            cs.military,
            cs.incorporated,
            cs.timezone,
            usc.id county_id,
            t.id county_term_id,
            cs.city simple_name
            FROM city_source cs
            join united_states_county usc on usc.fips = cs.county_fips
            join term t on t.nameable_id = usc.id and t.vocabulary_id = 100000
            left join city_source cs2 on cs2.city = cs.city and cs2.state_id = cs.state_id and cs.id <> cs2.id
            """;
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var cityName = reader.GetString(0);
            var latitude = reader.GetDecimal(1);
            var longitude = reader.GetDecimal(2);
            var population = reader.GetInt32(3);
            var density = reader.GetDouble(4);
            var military = reader.GetBoolean(5);
            var incorporated = reader.GetBoolean(6);
            var timezone = reader.GetString(7);
            var countyId = reader.GetInt32(8);
            var termId = reader.GetInt32(9);
            var simpleName = reader.GetString(10);
            yield return new UnitedStatesCity.ToCreate {
                Identification = new Common.Identification.Possible { Id = null },
                NameableDetails = new NameableDetails.ForCreate {
                    Description = cityName,
                    FileIdTileImage = null,
                    Terms = new List<Term.ToCreateForNewNameable> {
                        new Term.ToCreateForNewNameable {
                            Identification = new Common.Identification.Possible {
                                Id = null
                            },
                            Name = cityName,
                            ParentTermIds = new List<int>{ termId },
                            VocabularyId = 100000
                        }
                    },
                },
                NodeDetails = new NodeDetails.ForCreate {
                    AuthoringStatusId = 1,
                    FilesToAdd = new(),
                    ChangedDateTime = DateTime.Now.AddDays(-100),
                    CreatedDateTime = DateTime.Now.AddDays(-100),
                    NodeTypeId = 70,
                    PublisherId = 1,
                    Title = cityName,
                    OwnerId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode> {
                        new TenantNode.ToCreate.ForNewNode {
                            Identification = new Common.Identification.Possible {
                                Id= null
                            },
                            PublicationStatusId = 1,
                            SubgroupId = null,
                            TenantId = 1,
                            UrlId = null,
                        }
                    },
                    TermIds = new(),
                },
                Latitude = latitude,
                Longitude = longitude,
                Population = population,
                Density = density,
                Military = military,
                Incorporated = incorporated,
                Timezone = timezone,
                UnitedStatesCountyId = countyId,
                SimpleName = simpleName,
            };
        }
    }
    static async IAsyncEnumerable<UnitedStatesCounty.ToCreate> ReadUSCounties(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT 
            distinct 
            sd.id,
            t.id term_id,
            county_fips, 
            county_name || ' county (' || state_id || ')'
            FROM public.city_source cs
            join iso_coded_subdivision sd on sd.iso_3166_2_code = 'US-' || state_id
            join node n on n.id = sd.id
            join node_type nt on nt.id = n.node_type_id
            join term t on t.nameable_id = sd.id and t.vocabulary_id = 100000
            --left join united_states_county usc on usc.fips = county_fips
            where state_id <> 'DC'
            and state_id <> 'PR'
            and county_fips not in (24510, 29510, 51600, 51620, 51760, 51770)
            --and usc.id is null
            order by county_fips
            """;
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var subdivisionId = reader.GetInt32(0);
            var termId = reader.GetInt32(1);
            var fips = reader.GetInt32(2);
            var countyName = reader.GetString(3);
            yield return new UnitedStatesCounty.ToCreate {
                Identification = new Common.Identification.Possible { Id = null },
                NameableDetails = new NameableDetails.ForCreate { 
                    Description = countyName,
                    FileIdTileImage = null,
                    Terms = new List<Term.ToCreateForNewNameable> {
                        new Term.ToCreateForNewNameable {
                            Identification = new Common.Identification.Possible {
                                Id = null
                            },
                            Name = countyName,
                            ParentTermIds = new List<int>{ termId },
                            VocabularyId = 100000
                        }
                    },
                },
                NodeDetails = new NodeDetails.ForCreate { 
                    AuthoringStatusId = 1,
                    FilesToAdd = new(),
                    ChangedDateTime = DateTime.Now.AddDays(-100),
                    CreatedDateTime = DateTime.Now.AddDays(-100),
                    NodeTypeId = 69,
                    PublisherId = 1,
                    Title = countyName,
                    OwnerId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode> {
                        new TenantNode.ToCreate.ForNewNode {
                            Identification = new Common.Identification.Possible {
                                Id= null
                            },
                            PublicationStatusId = 1,
                            SubgroupId = null,
                            TenantId = 1,
                            UrlId = null,
                        }
                    },
                    TermIds = new(),
                },
                UnitedStatesStateId = subdivisionId,
                Fips = fips
            };
        }
    }
}
