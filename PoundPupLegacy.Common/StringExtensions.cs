using static System.Net.Mime.MediaTypeNames;

namespace PoundPupLegacy.Common;

public static class StringExtensions
{
    public static string CapitalizeFirstCharacter(this string input) {
        if(string.IsNullOrEmpty(input)) {
            return input;
        }
        return input[0].ToString().ToUpper() + input.Substring(1);
    }
    public static string Pluralize(this string input)
    {
        if (string.IsNullOrEmpty(input)) {
            return input;
        }
        Dictionary<string, string> exceptions = new Dictionary<string, string>() {
                { "man", "men" },
                { "woman", "women" },
                { "child", "children" },
                { "tooth", "teeth" },
                { "foot", "feet" },
                { "mouse", "mice" },
            { "belief", "beliefs" } };

        if (exceptions.ContainsKey(input.ToLowerInvariant())) {
            return exceptions[input.ToLowerInvariant()];
        }
        else if (input.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
            !input.EndsWith("ay", StringComparison.OrdinalIgnoreCase) &&
            !input.EndsWith("ey", StringComparison.OrdinalIgnoreCase) &&
            !input.EndsWith("iy", StringComparison.OrdinalIgnoreCase) &&
            !input.EndsWith("oy", StringComparison.OrdinalIgnoreCase) &&
            !input.EndsWith("uy", StringComparison.OrdinalIgnoreCase)) {
            return input.Substring(0, input.Length - 1) + "ies";
        }
        else if (input.EndsWith("us", StringComparison.InvariantCultureIgnoreCase)) {
            // http://en.wikipedia.org/wiki/Plural_form_of_words_ending_in_-us
            return input + "es";
        }
        else if (input.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase)) {
            return input + "es";
        }
        else if (input.EndsWith("s", StringComparison.InvariantCultureIgnoreCase)) {
            return input;
        }
        else if (input.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) ||
            input.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase) ||
            input.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase)) {
            return input + "es";
        }
        else if (input.EndsWith("f", StringComparison.InvariantCultureIgnoreCase) && input.Length > 1) {
            return input.Substring(0, input.Length - 1) + "ves";
        }
        else if (input.EndsWith("fe", StringComparison.InvariantCultureIgnoreCase) && input.Length > 2) {
            return input.Substring(0, input.Length - 2) + "ves";
        }
        else {
            return input + "s";
        }
    }

}
