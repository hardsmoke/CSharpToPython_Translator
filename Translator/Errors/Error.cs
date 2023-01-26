namespace Translator.Errors
{
    public abstract class Error
    {
        public readonly string Message;

        public Error(string message = default)
        {
            Message = message;
        }

        public abstract string GetErrorDescription();

        sealed public override string ToString()
        {
            return $"{GetType().Name}: {GetErrorDescription()} {(Message == default ? string.Empty : $"\nMessage: {Message}")}";
        }

        public static bool operator ==(Error a, Error b)
        {
            return a.GetErrorDescription() == b.GetErrorDescription();
        }

        public static bool operator !=(Error a, Error b)
        {
            return a.GetErrorDescription() != b.GetErrorDescription();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
