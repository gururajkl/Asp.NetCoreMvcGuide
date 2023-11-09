namespace DIServiceLifetime.Services
{
    public class ScopedGUIDService : IScopedGUIDService
    {
        private readonly Guid guid;

        public ScopedGUIDService()
        {
            guid = Guid.NewGuid();
        }

        public string GetGUID()
        {
            return guid.ToString();
        }
    }
}
