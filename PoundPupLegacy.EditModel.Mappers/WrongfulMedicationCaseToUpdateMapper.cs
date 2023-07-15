﻿namespace PoundPupLegacy.EditModel.Mappers;

internal class WrongfulMedicationCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<WrongfulMedicationCase.ToUpdate, CreateModel.WrongfulMedicationCase.ToUpdate>
{
    public CreateModel.WrongfulMedicationCase.ToUpdate Map(WrongfulMedicationCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.WrongfulMedicationCase.ToUpdate {
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
