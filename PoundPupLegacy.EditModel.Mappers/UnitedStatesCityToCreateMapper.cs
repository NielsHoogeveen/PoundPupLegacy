namespace PoundPupLegacy.EditModel.Mappers;

internal class UnitedStatesCityToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<UnitedStatesCity.ToCreate, DomainModel.UnitedStatesCity.ToCreate>
{
    public DomainModel.UnitedStatesCity.ToCreate Map(UnitedStatesCity.ToCreate source)
    {
        return new DomainModel.UnitedStatesCity.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            Population = source.Population,
            Density = source.Density,
            Military = source.Military,
            Incorporated = source.Incorporated,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Timezone = source.Timezone,
            UnitedStatesCountyId = source.CountyId,
            SimpleName = source.SimpleName
        };
    }
}
