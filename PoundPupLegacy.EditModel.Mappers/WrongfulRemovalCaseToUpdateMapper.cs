﻿namespace PoundPupLegacy.EditModel.Mappers;

internal class WrongfulRemovalCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<WrongfulRemovalCase.ToUpdate, DomainModel.WrongfulRemovalCase.ToUpdate>
{
    public DomainModel.WrongfulRemovalCase.ToUpdate Map(WrongfulRemovalCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.WrongfulRemovalCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
        };
    }
}
