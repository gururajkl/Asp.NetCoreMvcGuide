using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            db.ShoppingCarts.Update(shoppingCart);
        }
    }
}
