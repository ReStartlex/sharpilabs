using TspuWebLabs.Data;

namespace TspuWebLabs.Repositories
{
    public static class StaticUsersRepository
    {
        private static List<User> _users;
        private static int _validId;

        static StaticUsersRepository()
        {
            _users = new List<User>();
            _validId = 0;
        }

        public static IEnumerable<User> GetData() => _users;

        public static User? Get(int id)
        {
            return _users.Find(x => x.Id == id);
        }

        public static bool TryGet(int id, out User user)
        {
            user = Get(id);
            return user != null;
        }

        public static void Add(string name, string login, string password)
        {
            User user = new User();
            user.Id = _validId++;
            user.Name = name;
            user.Login = login;
            user.Password = password;

            _users.Add(user);
        }

        public static bool TryDelete(int id)
        {
            User? user = Get(id);

            if (user != null)
            {
                _users.Remove(user);
                return true;
            }
            return false;
        }

        public static bool TryEdit(int id, string name, string login)
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
