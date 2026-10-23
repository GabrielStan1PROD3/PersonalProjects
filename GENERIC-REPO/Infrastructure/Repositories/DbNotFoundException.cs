namespace Infrastructure.Repositories
{
    [Serializable]
    internal class DbNotFoundException : Exception
    {
        public DbNotFoundException()
        {
        }

        public DbNotFoundException(string? message) : base(message)
        {
        }

        public DbNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}