namespace PoundPupLegacy.ViewModel.Models;


public record OrganizationSearch
{
    private Organizations organizations = new Organizations { Entries = Array.Empty<OrganizationListEntry>(), NumberOfEntries = 0 };
    public required Organizations Organizations {
        get => organizations;
        init {
            if (value is not null) {
                organizations = value;
            }
        }
    }

    private SelectionItem[] _countries = Array.Empty<SelectionItem>();
    public required SelectionItem[] Countries {
        get => _countries;
        init {
            if (value is not null) {
                _countries = value;
            }
        }
    }
    private SelectionItem[] _organizationTypes = Array.Empty<SelectionItem>();
    public SelectionItem[] OrganizationTypes {
        get => _organizationTypes;
        set {
            if (value is not null) {
                _organizationTypes = value;
            }
        }
    }
}
