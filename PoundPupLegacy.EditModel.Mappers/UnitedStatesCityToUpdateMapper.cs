namespace PoundPupLegacy.EditModel.Mappers;

internal class UnitedStatesCityToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<UnitedStatesCity.ToUpdate, DomainModel.UnitedStatesCity.ToUpdate>
{
    public DomainModel.UnitedStatesCity.ToUpdate Map(UnitedStatesCity.ToUpdate source)
    {
        return new DomainModel.UnitedStatesCity.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
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
