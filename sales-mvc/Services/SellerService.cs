using Microsoft.EntityFrameworkCore;
using sales_mvc.Data;
using sales_mvc.Models;
using sales_mvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sales_mvc.Services
{
    public class SellerService
    {
        private readonly SalesContext _context;

        public SellerService(SalesContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAll()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task Insert(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindById(int id)
        {
            return await _context.Seller.Include(seller => seller.Department).
                FirstOrDefaultAsync(seller => seller.Id == id);
        }

        public async Task Remove(int id)
        {
            try
            {
                var seller = await _context.Seller.FindAsync(id);

                _context.Seller.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Can't delete seller because he/she has sales.");
            }

        }

        public async Task Update(Seller seller)
        {
            var hasSeller = await _context.Seller.AnyAsync(sellerBd => sellerBd.Id == seller.Id);

            if (!hasSeller)
            {
                throw new NotFoundException("Id not found.");
            }

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException err)
            {
                throw new DbConcurrencyException(err.Message);
            }
        }
    }
}
