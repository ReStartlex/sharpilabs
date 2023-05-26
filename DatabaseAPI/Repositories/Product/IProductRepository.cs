using DatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.Repositories.Product
{
    public interface IProductRepository
    {
        public Task Add(DBProduct product);
        public Task<bool> Update(int id, DBProduct product);
        public Task<bool> Remove(int id);
        public Task<ICollection<DBProduct>> Get();
        public Task<DBProduct> Get(int id);
        public bool TryGet(int id, out DBProduct product);
        public Task<ICollection<DBProduct>> Get(int length, int index, decimal minPrice, decimal maxValue, SortType sortType);
    }
}
