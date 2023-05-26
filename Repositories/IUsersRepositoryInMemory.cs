using TspuWebLabs.Data;

namespace labisharp.Repositories
{
    public interface IUsersRepositoryInMemory
    {
        public IEnumerable<User> GetData();
        public User? Get(int id);
        public bool TryGet(int id, out User user);
        public void Add(string name, string login, string password);
        public bool TryDelete(int id);
        public bool TryEdit(int id, string name, string login);
    }
}
