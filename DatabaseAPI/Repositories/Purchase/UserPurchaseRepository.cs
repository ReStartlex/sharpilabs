using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.Repositories.Purchase
{
    public class UserPurchaseRepository : IUserPurchaseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserPurchaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(DBUserPurchase purchase)
        {
            await _dbContext.Purchases.AddAsync(purchase);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<DBUserPurchase>> Get()
        {
            ICollection<DBUserPurchase> result = await _dbContext.Purchases.ToArrayAsync();
            return result;
        }

        public async Task<ICollection<DBUserPurchase>> Get(int userId)
        {
            var userPurchases = await _dbContext.Purchases
                .Include(u => u.Product)
                .Where(purchase => purchase.UserId == userId)
                .ToListAsync();

            return userPurchases;
        }
    }
}