using TspuWebLabs.Data;

namespace labisharp.DatabaseAPI.Repositories.User
{
    public class UsersRepositoryInMemory : IUsersRepositoryInMemory
    {
        private MemoryProvider memoryProvider;

        public UsersRepositoryInMemory(MemoryProvider memoryProvider)
        {
            this.memoryProvider = memoryProvider;
        }

        public void Add(string name, string login, string password)
        {
            User user = new User();
            user.Id = memoryProvider.ValidId++;
            user.Name = name;
            user.Login = login;
            user.Password = password;

            memoryProvider.Users.Add(user);
        }

        public User? Get(int id) => memoryProvider.Users.Find(x => x.Id == id);
        public bool TryGet(int id, out User user)
        {
            user = Get(id);
            return user != null;
        }

        public IEnumerable<User> GetData() => memoryProvider.Users;

        public bool TryDelete(int id)
        {
            User? user = Get(id);

            if (user != null)
            {
                memoryProvider.Users.Remove(user);
                return true;
            }
            return false;
        }

        public bool TryEdit(int id, string name, string login)
        {
            User? user = Get(id);

            if (user != null)
            {
                user.Name = name;
                user.Login = login;
                return true;
            }
            return false;
        }

    }
}
