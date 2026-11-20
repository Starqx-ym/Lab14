using Lab13_YadhiraCcori.Domain.Entities;
using Lab13_YadhiraCcori.Domain.Interfaces;
using Lab13_YadhiraCcori.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lab13_YadhiraCcori.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetClientsWithOrdersAsync()
    {
        return await _context.Clients
            .Include(c => c.Orders)
            .ThenInclude(o => o.Orderdetails)
            .ThenInclude(od => od.Product)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsWithSalesAsync()
    {
        return await _context.Products
            .Include(p => p.Orderdetails)
            .AsNoTracking()
            .ToListAsync();
    }
}