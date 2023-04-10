using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel;

public record DocumentListItem
{
    public string Path { get; set; }

    public string Title { get; set; }

    public DateTime? PublicationDateFrom { get; set; }

    public DateTime? PublicationDateTo { get; set; }
    public FuzzyDate? Published {
        get {
            if (PublicationDateFrom is not null && PublicationDateTo is not null) {
                var dateTimeRange = new DateTimeRange(PublicationDateFrom, PublicationDateTo);
                if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                    return result;
                }
            }
            return null;
        }
    }

    public int SortOrder { get; set; }
}
