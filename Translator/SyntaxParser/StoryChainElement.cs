namespace Translator.SyntaxParser
{
    public class StoryChainElement
    {
        public readonly string Symbol;
        public readonly int SerialNumber;

        public StoryChainElement(string symbol, int serialNumber)
        {
            Symbol = symbol;
            SerialNumber = serialNumber;
        }
    }
}
