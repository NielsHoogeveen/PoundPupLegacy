namespace PoundPupLegacy.Model;

public interface ProfessionalRole : Documentable
{
    int? PersonId { get; set; }
    DateTimeRange? DateTimeRange { get; }

    int ProfessionId { get; }
}

