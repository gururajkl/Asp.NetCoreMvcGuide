namespace DIServiceLifetime.Services
{
    public class TransientGUIDService : ITransientGUIDService
    {
        private readonly Guid guid;

        public TransientGUIDService()
        {
            guid = Guid.NewGuid();
        }

        public string GetGUID()
        {
            return guid.ToString();
        }
    }
}
