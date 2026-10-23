namespace Infrastructure.Converters
{
    [Serializable]
    internal class DbConversionException : Exception
    {
        public DbConversionException()
        {
        }

        public DbConversionException(string? message) : base(message)
        {
        }

        public DbConversionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}