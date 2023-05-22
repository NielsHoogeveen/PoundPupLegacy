namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableBottomLevelSubdivision : BottomLevelSubdivision, ImmediatelyIdentifiableSubdivision
{

}

public interface EventuallyIdentifiableBottomLevelSubdivision: BottomLevelSubdivision, EventuallyIdentifiableSubdivision
{

}
public interface BottomLevelSubdivision : Subdivision
{
}
