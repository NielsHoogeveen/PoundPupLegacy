namespace PoundPupLegacy.Model;

public interface Person : Party
{
    public DateTime? DateOfBirth { get; }
    public DateTime? DateOfDeath { get; }
    public int? FileIdPortrait { get; }

}
