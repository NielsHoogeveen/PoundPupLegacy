namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableFirstLevelSubdivision : FirstLevelSubdivision, ImmediatelyIdentifiableSubdivision
{

}
public interface EventuallyIdentifiableFirstLevelSubdivision : FirstLevelSubdivision, EventuallyIdentifiableSubdivision
{

}
public interface FirstLevelSubdivision : Subdivision
{
}
