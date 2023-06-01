﻿namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class ChildTraffickingCaseToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<ChildTraffickingCase.ToCreate, CreateModel.ChildTraffickingCase.ToCreate>
{
    public CreateModel.ChildTraffickingCase.ToCreate Map(ChildTraffickingCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.ChildTraffickingCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            ChildTraffickingCaseDetails = new CreateModel.ChildTraffickingCaseDetails {
                NumberOfChildrenInvolved = viewModel.ResolvedChildTraffickingCaseDetails.NumberOfChildrenInvolved,
                CountryIdFrom = viewModel.ResolvedChildTraffickingCaseDetails.CountryFrom.Id,
            }
        };
    }
}
