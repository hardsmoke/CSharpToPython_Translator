namespace Translator.Errors
{
    internal class DetectedEmptyInputError : Error
    {
        public override string GetErrorDescription()
        {
            return "Input can not be empty!";
        }
    }
}
