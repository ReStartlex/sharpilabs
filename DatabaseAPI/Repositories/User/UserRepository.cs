using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<DBUser>> Get()
        {
            var result = await dbContext.Users.ToListAsync();
            return result;
        }

        public async Task<ICollection<DBUser>> Get(int length, int index)
        {
            ICollection<DBUser> result = await dbContext.Users
                .OrderBy(x => x.Id)
                .Skip(index * length)
                .Take(length)
                .ToListAsync();

            return result;
        }

        public async Task Add(DBUser user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, DBUser user)
        {
            DBUser userDataBase = await dbContext.Users.FindAsync(id);

            if(userDataBase != null)
            {
                userDataBase.Name = user.Name;
                userDataBase.Login = user.Login;
                userDataBase.Password = user.Password;

                await dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
