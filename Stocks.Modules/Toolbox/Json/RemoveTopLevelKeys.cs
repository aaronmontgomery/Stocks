using System.Text;

namespace Toolbox
{
    public static partial class Json
    {
        public static string RemoveTopLevelKeys(string json)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append('[');

            int entryOpenIndex = json.IndexOf(":{");
            while (entryOpenIndex != -1)
            {
                int i = entryOpenIndex;
                entryOpenIndex = json.IndexOf(":{", entryOpenIndex + 1);

                int entryCloseIndex;
                if (entryOpenIndex != -1)
                {
                    entryCloseIndex = json.IndexOf("},", i);
                }

                else
                {
                    entryCloseIndex = json.LastIndexOf('}', json.Length - 1);
                    entryCloseIndex = json.LastIndexOf('}', entryCloseIndex - 1);
                }

                string entry = json.Substring(i + 1, entryCloseIndex - i);
                stringBuilder.Append(entry);
                if (entryOpenIndex != -1)
                {
                    stringBuilder.Append(',');
                }
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }
    }
}
