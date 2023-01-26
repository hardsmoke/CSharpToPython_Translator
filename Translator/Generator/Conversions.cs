namespace Translator.Generator
{
    public class Conversions
    {
        public readonly List<string> ConversionsRaw;
        public readonly List<Conversion<string>> ConvertionList;

        public Conversions(List<string> conversionsRaw, char tokenInChar, string conversionDelimiter)
        {
            ConversionsRaw = conversionsRaw;
            ConvertionList = Parse(conversionsRaw, tokenInChar, conversionDelimiter);
        }

        private List<Conversion<string>> Parse(List<string> conversionsRaw, char tokenInChar, string conversionDelimiter)
        {
            List<Conversion<string>> result = new List<Conversion<string>>();

            foreach (var conversionRaw in conversionsRaw)
            {
                string[] conversionSplit = conversionRaw.Split(conversionDelimiter);

                result.Add(new Conversion<string>(conversionSplit[0].Trim(tokenInChar), conversionSplit[1].Trim(tokenInChar)));
            }

            result.Add(new Conversion<string>(";", "\r\n"));
            result.Add(new Conversion<string>("\r\n", ""));
            result.Add(new Conversion<string>("\t", ""));

            return result;
        }

        public Conversion<string> GetConversion(string converted)
        {
            foreach (var conversion in ConvertionList)
            {
                if (conversion.Initial == converted)
                {
                    return conversion;
                }
            }

            return null;
        }

        public void Print(List<Conversion<string>> conversions)
        {
            foreach (var conversion in conversions)
            {
                Console.WriteLine($"{conversion.Initial} to {conversion.Converted}");
            }
        }
    }
}
