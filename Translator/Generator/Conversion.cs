namespace Translator.Generator
{
    public class Conversion<T>
    {
        public readonly T Initial;
        public readonly T Converted;

        public Conversion(T initial, T converted)
        {
            Initial = initial;
            Converted = converted;
        }
    }
}
