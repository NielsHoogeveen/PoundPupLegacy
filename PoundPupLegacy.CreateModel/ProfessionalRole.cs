using PoundPupLegacy.Common;

namespace PoundPupLegacy.CreateModel;

public interface ProfessionalRole : Identifiable
{
    int? PersonId { get; set; }
    DateTimeRange? DateTimeRange { get; }

    int ProfessionId { get; }
}

