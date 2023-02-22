﻿namespace PoundPupLegacy.Model;

public interface ProfessionalRole: Identifiable
{
    int? PersonId { get; set; }
    DateTimeRange? DateTimeRange { get; }

    int ProfessionId { get; }
}

public record BasicProfessionalRole: ProfessionalRole
{
    public required int? Id { get; set; }

    public required int? PersonId { get; set; }

    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }
}