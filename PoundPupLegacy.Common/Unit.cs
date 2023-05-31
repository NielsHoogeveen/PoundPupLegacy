namespace PoundPupLegacy.Common;

public class Unit
{
    private Unit() { }
    public static Unit Instance { get; } = new();
}
