using System.Text.RegularExpressions;
using Xunit;

namespace PoundPupLegacy.Test;


public class UnitTest1
{
    private const string URL_PATH = "united_states_senate_114th_congress";
    private const string REGEX = "united_states_(senate|house)_([0-9]+)(th|st|nd|rd)_congress";

    [Fact] 
    public void RegularExpressionIsCapableOfExtractingTypeOfChamber() 
    {
        Regex regex = new Regex(REGEX);
        var match = regex.Match(URL_PATH);
        Assert.True(match.Success );
        Assert.Equal(match.Groups.Count,4);
        Assert.Equal(match.Groups[1].Value, "senate");
        Assert.Equal(match.Groups[2].Value, "114");

    }

}