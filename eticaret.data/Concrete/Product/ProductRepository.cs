using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Product
{
    public class ProductRepository : Repository<et.Product.Product>, IProductRepository
    {
        private readonly ETicaretDbContext _context;

        public ProductRepository(ETicaretDbContext context) : base(context)
        {
            _context = context;
        }

        public List<et.Product.Product> GetAllWithImages()
            => _context.Products.Include(d => d.ProductImages).ToList();
        
    }
}
