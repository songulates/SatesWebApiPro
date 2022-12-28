using Microsoft.EntityFrameworkCore;
using SatesWebApiPro.Data;
using SatesWebApiPro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SatesWebApiPro.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            //AsNoTracking İZLEMESİN DİYE
            return await _context.Products.AsNoTracking().ToListAsync();
            //sonra sturtapda dependenciy i register et
        }
        //burda geriye liste dönme
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.AsNoTracking().SingleOrDefaultAsync(x=>x.Id==id);
        }

        public async Task RemoveAsync(int id)
        {
            var removeEntity = await _context.Products.FindAsync(id);
            _context.Products.Remove(removeEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            //değişmeyen entitileri  getirelim
            var unchangedEntity = await _context.Products.FindAsync(product.Id);
            _context.Entry(unchangedEntity).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
        }
        //file upload işlemi
        
    }
}
