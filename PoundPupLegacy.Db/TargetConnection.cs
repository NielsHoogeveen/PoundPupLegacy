using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Db;

public class TargetConnection
{
    private readonly NpgsqlConnection _connection;

    public TargetConnection(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    public void Create(IEnumerable<BasicNode> nodes, string tableName)
    {
        using var nodeWriter = CreateNodeWriter();
        using var idTableWriter = CreateSingleIdWriter(tableName);

        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            idTableWriter.Write(node.Id);
        }
    }
    public void Create(IEnumerable<BasicCountry> countries)
    {

        using var nodeWriter = CreateNodeWriter();
        using var geographicalEntityWriter = CreateGeographicalEnityWriter();
        using var politicalEntityWriter = CreatePoliticalEnityWriter();
        using var countryWriter = CreateCountryWriter();
        using var topLevelCountryWriter = CreateTopLevelCountryWriter();
        using var basicCountryWriter = CreateBasicCountryWriter();
        using var termHierarchyWriter = CreateTermHierarchyWriter();

        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            topLevelCountryWriter.Write(country);
            basicCountryWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy { ParentId = country.GlobalRegionId, ChildId = country.Id });
        }
    }

    public void Create(IEnumerable<BindingCountry> countries)
    {

        using var nodeWriter = CreateNodeWriter();
        using var geographicalEntityWriter = CreateGeographicalEnityWriter();
        using var politicalEntityWriter = CreatePoliticalEnityWriter();
        using var countryWriter = CreateCountryWriter();
        using var topLevelCountryWriter = CreateTopLevelCountryWriter();
        using var bindingCountryWriter = CreateBindingCountryWriter();
        using var termHierarchyWriter = CreateTermHierarchyWriter();


        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            topLevelCountryWriter.Write(country);
            bindingCountryWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.GlobalRegionId,
                ChildId = country.Id
            });
        }
    }
    public void Create(IEnumerable<BoundCountry> countries)
    {
        using var nodeWriter = CreateNodeWriter();
        using var geographicalEntityWriter = CreateGeographicalEnityWriter();
        using var politicalEntityWriter = CreatePoliticalEnityWriter();
        using var countryWriter = CreateCountryWriter();
        using var subdivisionWriter = CreateSubdivisionWriter();
        using var isoCodedSubdivisionWriter = CreateISOCodedSubdivisionWriter();
        using var boundCountryWriter = CreateBoundCountryWriter();
        using var termHierarchyWriter = CreateTermHierarchyWriter();

        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            subdivisionWriter.Write(country);
            isoCodedSubdivisionWriter.Write(country);
            boundCountryWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.BindingCountryId,
                ChildId = country.Id
            });
        }
    }
    public void Create(IEnumerable<BasicCountryAndFirstLevelSubdivision> countries)
    {
        using var nodeWriter = CreateNodeWriter();
        using var geographicalEntityWriter = CreateGeographicalEnityWriter();
        using var politicalEntityWriter = CreatePoliticalEnityWriter();
        using var countryWriter = CreateCountryWriter();
        using var topLevelCountryWriter = CreateTopLevelCountryWriter();
        using var subdivisionWriter = CreateSubdivisionWriter();
        using var isoCodedSubdivisionWriter = CreateISOCodedSubdivisionWriter();
        using var firstLevelSubdivisionWriter = CreateFirstLevelSubdivisionWriter();
        using var isoCodedFirstLevelSubdivisionWriter = CreateISOCodedFirstLevelSubdivisionWriter();
        using var countryAndFirstLevelSubdivisionWriter = CreateCountryAndFirstLevelSubdivisionWriter();
        using var basicCountryAndFirstLevelSubdivisionWriter = CreateBasicCountryAndFirstLevelSubdivisionWriter();
        using var termHierarchyWriter = CreateTermHierarchyWriter();

        foreach (var country in countries)
        {
            nodeWriter.Write(country);
            geographicalEntityWriter.Write(country);
            politicalEntityWriter.Write(country);
            countryWriter.Write(country);
            topLevelCountryWriter.Write(country);
            subdivisionWriter.Write(country);
            isoCodedSubdivisionWriter.Write(country);
            firstLevelSubdivisionWriter.Write(country);
            isoCodedFirstLevelSubdivisionWriter.Write(country);
            countryAndFirstLevelSubdivisionWriter.Write(country);
            basicCountryAndFirstLevelSubdivisionWriter.Write(country);
            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = country.GlobalRegionId,
                ChildId = country.Id
            });
        }
    }


    public void Create(IEnumerable<FirstLevelGlobalRegion> nodes)
    {
        using var nodeWriter = CreateNodeWriter();
        using var geographicalEntityWriter = CreateGeographicalEnityWriter();
        using var globalRegionWriter = CreateGlobalRegionWriter();
        using var firstLevelGlobalRegionWriter = CreateFirstLevelGlobalRegionWriter();

        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            geographicalEntityWriter.Write(node);
            globalRegionWriter.Write(node);
            firstLevelGlobalRegionWriter.Write(node);
        }
    }

    public void Create(IEnumerable<SecondLevelGlobalRegion> nodes)
    {
        using var nodeWriter = CreateNodeWriter();
        using var geographicalEntityWriter = CreateGeographicalEnityWriter();
        using var globalRegionWriter = CreateGlobalRegionWriter();
        using var secondLevelGlobalRegionWriter = CreateSecondLevelGlobalRegionWriter();
        using var termHierarchyWriter = CreateTermHierarchyWriter();


        foreach (var node in nodes)
        {
            nodeWriter.Write(node);
            geographicalEntityWriter.Write(node);
            globalRegionWriter.Write(node);
            secondLevelGlobalRegionWriter.Write(node);

            termHierarchyWriter.Write(new TermHierarchy
            {
                ParentId = node.FirstLevelGlobalRegionId,
                ChildId = node.Id
            });
        }
    }

    private NpgsqlCommand CreateSingleIdCommand(string tableName)
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."{tableName}" (id) VALUES(@id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        return command;
    }

    private DatabaseWriter<int> CreateSingleIdWriter(string tableName)
    {
        return new SingleIdWriter(CreateSingleIdCommand(tableName));

    }
    private DatabaseWriter<GeographicalEntity> CreateGeographicalEnityWriter()
    {
        return new SingleIdWriter<GeographicalEntity>(CreateSingleIdCommand("geographical_entity"));
    }
    private DatabaseWriter<PoliticalEntity> CreatePoliticalEnityWriter()
    {
        return new SingleIdWriter<PoliticalEntity>(CreateSingleIdCommand("political_entity"));
    }
    private DatabaseWriter<Country> CreateCountryWriter()
    {
        return new SingleIdWriter<Country>(CreateSingleIdCommand("country"));
    }
    private DatabaseWriter<BasicCountryAndFirstLevelSubdivision> CreateBasicCountryAndFirstLevelSubdivisionWriter()
    {
        return new SingleIdWriter<BasicCountryAndFirstLevelSubdivision>(CreateSingleIdCommand("basic_country_and_first_level_subdivision"));
    }
    private DatabaseWriter<CountryAndFirstLevelSubdivision> CreateCountryAndFirstLevelSubdivisionWriter()
    {
        return new SingleIdWriter<CountryAndFirstLevelSubdivision>(CreateSingleIdCommand("country_and_first_level_subdivision"));
    }
    private DatabaseWriter<ISOCodedFirstLevelSubdivision> CreateISOCodedFirstLevelSubdivisionWriter()
    {
        return new SingleIdWriter<ISOCodedFirstLevelSubdivision>(CreateSingleIdCommand("iso_coded_first_level_subdivision"));
    }
    private DatabaseWriter<GlobalRegion> CreateGlobalRegionWriter()
    {
        return new SingleIdWriter<GlobalRegion>(CreateSingleIdCommand("global_region"));
    }
    private DatabaseWriter<FirstLevelGlobalRegion> CreateFirstLevelGlobalRegionWriter()
    {
        return new SingleIdWriter<FirstLevelGlobalRegion>(CreateSingleIdCommand("first_level_global_region"));
    }
    private DatabaseWriter<BasicCountry> CreateBasicCountryWriter()
    {
        return new SingleIdWriter<BasicCountry>(CreateSingleIdCommand("basic_country"));
    }

    private DatabaseWriter<BindingCountry> CreateBindingCountryWriter()
    {
        return new SingleIdWriter<BindingCountry>(CreateSingleIdCommand("binding_country"));
    }

    private DatabaseWriter<FirstLevelSubdivision> CreateFirstLevelSubdivisionWriter()
    {
        return new SingleIdWriter<FirstLevelSubdivision>(CreateSingleIdCommand("first_level_subdivision"));
    }

    private DatabaseWriter<SecondLevelGlobalRegion> CreateSecondLevelGlobalRegionWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."second_level_global_region" (id, first_level_global_region_id) VALUES(@id,@first_level_global_region_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("first_level_global_region_id", NpgsqlDbType.Integer);
        return new SeconcLevelGlobalRegionWriter(command);
    }

    private DatabaseWriter<TermHierarchy> CreateTermHierarchyWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."term_hierarchy" (term_id_parent, term_id_child) VALUES(@term_id_parent,@term_id_child)""";
        command.Parameters.Add("term_id_parent", NpgsqlDbType.Integer);
        command.Parameters.Add("term_id_child", NpgsqlDbType.Integer);
        return new TermHierarchyWriter(command);

    }
    private DatabaseWriter<TopLevelCountry> CreateTopLevelCountryWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."top_level_country" (id, iso_3166_1_code, global_region_id) VALUES(@id,@iso_3166_1_code,@global_region_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("iso_3166_1_code", NpgsqlDbType.Char);
        command.Parameters.Add("global_region_id", NpgsqlDbType.Integer);
        return new TopLevelCountryWriter(command);
    }
    private DatabaseWriter<Subdivision> CreateSubdivisionWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."subdivision" (id, name, country_id) VALUES(@id,@name,@country_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        command.Parameters.Add("country_id", NpgsqlDbType.Integer);
        return new SubdivisionWriter(command);

    }


    private DatabaseWriter<ISOCodedSubdivision> CreateISOCodedSubdivisionWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."iso_coded_subdivision" (id, iso_3166_2_code) VALUES(@id,@iso_3166_2_code)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("iso_3166_2_code", NpgsqlDbType.Char);
        return new ISOCodedSubdivisionWriter(command);

    }

    private DatabaseWriter<BoundCountry> CreateBoundCountryWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."bound_country" (id, binding_country_id) VALUES(@id,@binding_country_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("binding_country_id", NpgsqlDbType.Integer);
        return new BoundCountryWriter(command);
    }

    private DatabaseWriter<Node> CreateNodeWriter()
    {
        var command = _connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = """INSERT INTO public."node" (id, user_id, created, changed, title, status, node_type_id, is_term) VALUES(@id, @user_id, @created, @changed, @title, @status, @node_type_id, @is_term)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlDbType.Integer);
        command.Parameters.Add("created", NpgsqlDbType.Timestamp);
        command.Parameters.Add("changed", NpgsqlDbType.Timestamp);
        command.Parameters.Add("title", NpgsqlDbType.Varchar);
        command.Parameters.Add("status", NpgsqlDbType.Integer);
        command.Parameters.Add("node_type_id", NpgsqlDbType.Integer);
        command.Parameters.Add("is_term", NpgsqlDbType.Boolean);
        command.Prepare();
        return new NodeWriter(command);
    }
}
