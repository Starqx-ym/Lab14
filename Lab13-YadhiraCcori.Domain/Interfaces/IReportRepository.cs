using Lab13_YadhiraCcori.Domain.Entities;
namespace Lab13_YadhiraCcori.Domain.Interfaces;

public interface IReportRepository
{
    Task<List<Client>> GetClientsWithOrdersAsync();
    Task<List<Product>> GetProductsWithSalesAsync();
}