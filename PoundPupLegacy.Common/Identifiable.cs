namespace PoundPupLegacy.Common;

public interface Identifiable: IRequest
{
    public int? Id { get; set; }
}
