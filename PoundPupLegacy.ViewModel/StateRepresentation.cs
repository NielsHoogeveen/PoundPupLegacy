
namespace PoundPupLegacy.ViewModel;
public record StateRepresentation 
{
    public required Link State { get; set; }

    private MemberOfCongress[] members = Array.Empty<MemberOfCongress>();
    public required MemberOfCongress[] Members {
        get => members;
        init {
            if(value is not null) 
            {
                members = value;
            }
        }
    }
}

