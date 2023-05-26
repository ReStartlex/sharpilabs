using DatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.Repositories.Purchase
{
    public interface IUserPurchaseRepository
    {
        public Task Add(DBUserPurchase purchase);
        public Task<ICollection<DBUserPurchase>> Get();
        public Task<ICollection<DBUserPurchase>> Get(int userId);
    }
}