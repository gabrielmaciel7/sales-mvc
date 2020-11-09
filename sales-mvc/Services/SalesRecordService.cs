using Microsoft.EntityFrameworkCore;
using sales_mvc.Data;
using sales_mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sales_mvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesContext _context;

        public SalesRecordService(SalesContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDate(DateTime? minDate, DateTime? maxDate)
        {
            var result = _context.SalesRecord.Select(sale => sale);

            if (minDate.HasValue)
            {
                result = result.Where(sale => sale.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(sale => sale.Date <= maxDate.Value);
            }

            return await result.Include(sale => sale.Seller).
                Include(sale => sale.Seller.Department).
                OrderByDescending(sale => sale.Date).ToListAsync();
        }

    }
}
