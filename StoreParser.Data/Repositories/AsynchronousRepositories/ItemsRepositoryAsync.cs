using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StoreParser.Data.Repositories
{
    public class ItemsRepositoryAsync : IRepositoryAsync<Item>
    {
        private EFContext db;

        public ItemsRepositoryAsync(EFContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {

            return await db.Items.Include(t => t.Prices).ToListAsync();
        }

        public async Task<Item> FindFirstAsync(Expression<Func<Item, bool>> predicate)
        {
            return await db.Items.Include(t => t.Prices).FirstOrDefaultAsync(predicate);
        }

        public Task CreateAsync(Item item)
        {
            db.Items.Add(item);
            return Task.CompletedTask;         
        }

    }
}
