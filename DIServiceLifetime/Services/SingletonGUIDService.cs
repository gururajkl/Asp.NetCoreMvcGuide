namespace DIServiceLifetime.Services
{
    public class SingletonGUIDService : ISingletonGUIDService
    {
        private readonly Guid guid;

        public SingletonGUIDService()
        {
            guid = Guid.NewGuid();
        }

        public string GetGUID()
        {
            return guid.ToString();
        }
    }
}
