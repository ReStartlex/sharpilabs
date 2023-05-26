using DatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.Repositories.User
{
    public interface IUserRepository
    {
        Task<ICollection<DBUser>> Get();
        Task<ICollection<DBUser>> Get(int length, int index);
        Task<DBUser> Get(string login);
        Task Add(DBUser user);
        Task<bool> Update(int id, DBUser user);

    }
}