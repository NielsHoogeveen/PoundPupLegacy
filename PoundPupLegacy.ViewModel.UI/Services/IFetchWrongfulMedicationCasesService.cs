namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchWrongfulMedicationCasesService
{
    [RequireNamedArgs]
    Task<WrongfulMedicationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms);
}
