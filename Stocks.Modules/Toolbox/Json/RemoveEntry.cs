namespace Toolbox
{
    public static partial class Json
    {
        public static string RemoveEntry(string json, string search)
        {
            int searchIndex = json.IndexOf(search, 0);
            while (searchIndex != -1)
            {
                int closeBraceIndex = json.IndexOf('}', searchIndex);
                int openBraceIndex = json.Substring(0, searchIndex).LastIndexOf('{');
                string entry = json[closeBraceIndex + 1] == ',' ? json.Substring(openBraceIndex, closeBraceIndex - openBraceIndex + 2) : json.Substring(openBraceIndex, closeBraceIndex - openBraceIndex + 1);
                json = json.Replace(entry, string.Empty);
                searchIndex = json.IndexOf(search, 0);
            }

            return json;
        }
    }
}
