using static PoundPupLegacy.Common.Identification;

namespace PoundPupLegacy.Convert;

internal sealed class ActionMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<BasicAction> basicActionCreatorFactory,
    IEntityCreatorFactory<CreateNodeAction> createNodeActionCreatorFactory,
    IEntityCreatorFactory<DeleteNodeAction> deleteNodeActionCreatorFactory,
    IEntityCreatorFactory<EditNodeAction> editNodeActionCreatorFactory,
    IEntityCreatorFactory<EditOwnNodeAction> editOwnNodeActionCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "actions";
    protected override async Task MigrateImpl()
    {
        await using var basicActionCreator = await basicActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var createNodeActionCreator = await createNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var deleteNodeActionCreator = await deleteNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var editNodeActionCreator = await editNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var editOwnNodeActionCreator = await editOwnNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await basicActionCreator.CreateAsync(GetBasicActions());
        await createNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new CreateNodeAction 
        { 
            Identification = new Possible {
                Id = null,
            },
            NodeTypeId = x.Identification.Id!.Value 
        }));
        await deleteNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new DeleteNodeAction {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeTypeId = x.Identification.Id!.Value 
        }));
        await editNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new EditNodeAction {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeTypeId = x.Identification.Id!.Value 
        }));
        await editOwnNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Where(x => x.AuthorSpecific).Select(x => new EditOwnNodeAction {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeTypeId = x.Identification.Id!.Value 
        }));
    }

    private async IAsyncEnumerable<BasicAction> GetBasicActions()
    {
        await Task.CompletedTask;
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/organizations",
            Description = "Show all organizations"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/persons",
            Description = "Show all persons"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/abuse_cases",
            Description = "Show all abuse cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/child_trafficking_cases",
            Description = "Show all child trafficking cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/coerced_adoption_cases",
            Description = "Show all coerced adoption cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/deportation_cases",
            Description = "Show all deportation cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/fathers_rights_violation_cases",
            Description = "Show all father's rights violation cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/wrongful_medication_cases",
            Description = "Show all wrongful medication cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/wrongful_removal_cases",
            Description = "Show all wrongful removal cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/disrupted_placement_cases",
            Description = "Show all disrupted placement cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/documents",
            Description = "Show all documents"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/blogs",
            Description = "Show all blogs"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/topics",
            Description = "Show all topics"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/countries",
            Description = "Show all topics"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/cases",
            Description = "Show all cases"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/contact",
            Description = "Contact page"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/search",
            Description = "Full text search"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/polls",
            Description = "Show all polls"
        };

        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/node/{Id:int}",
            Description = "View a node"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/blog/{Id:int}",
            Description = "View a blog"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/united_states_congress",
            Description = "View information about the United States congress"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/united_states_senate/{Id:int}",
            Description = "View information about the United States Senate"
        };
        yield return new BasicAction {
            Identification = new Identification.Possible {
                Id = null
            },
            Path = "/united_states_house_of_representatives/{Id:int}",
            Description = "View information about the United States House of Representatives"
        };
    }
}
