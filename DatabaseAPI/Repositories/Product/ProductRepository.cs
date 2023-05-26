using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Add(DBProduct product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<DBProduct>> Get()
        {
            ICollection<DBProduct> result = await dbContext.Products.ToArrayAsync();
            return result;
        }

        public async Task<DBProduct> Get(int id)
        {
            return await dbContext.Products.FindAsync(id);
        }

        public bool TryGet(int id, out DBProduct product)
        {
            product = Get(id).Result;

            if (product != null) return true;
            return false;
        }

        public async Task<ICollection<DBProduct>> Get(int length, int index, decimal minPrice = 0, decimal maxValue = Decimal.MinValue, SortType sortType = SortType.None)
        {
            ICollection<DBProduct> products = await Get();

            switch (sortType)
            {
                case SortType.Increasing:
                    products = products.OrderBy(x => x.Price).ToList();
                    break;
                case SortType.Decreasing:
                    products = products.OrderByDescending(x => x.Price).ToList();
                    break;
            }

            products = products.Where(x => x.IsDeleted != true && x.Price < maxValue && x.Price > minPrice)
                .Skip(index * length)
                .Take(length)
                .ToList();

            return products;
        }

        public async Task<bool> Remove(int id)
        {
            DBProduct dbProduct = await dbContext.Products.FindAsync(id);

            if(dbProduct != null)
            {
                dbProduct.IsDeleted = true;
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Update(int id, DBProduct product)
        {
            DBProduct dbProduct = await Get(id);

            if (dbProduct != null)
            {
                dbProduct.Name = product.Name;
                dbProduct.Description = product.Description;
                dbProduct.Price = product.Price;
                dbProduct.IsDeleted = product.IsDeleted;

                await dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
