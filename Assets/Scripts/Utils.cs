public class Utils
{
    public static string RemoveDups(string str)
    {
        string result = "";
        foreach (char c in str)
        {
            if (result.IndexOf(c) < 0)
            {
                result += c;
            }
        }
        return result;
    }
}
