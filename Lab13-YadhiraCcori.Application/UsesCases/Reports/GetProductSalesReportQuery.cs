using Lab13_YadhiraCcori.Application.DTOs;
using Lab13_YadhiraCcori.Domain.Interfaces;
using MediatR;

namespace Lab13_YadhiraCcori.Application.UsesCases.Reports;

public record GetProductSalesReportQuery() : IRequest<List<ProductSalesReportDto>>;

public class GetProductSalesReportHandler : IRequestHandler<GetProductSalesReportQuery, List<ProductSalesReportDto>>
{
    private readonly IReportRepository _repository;

    public GetProductSalesReportHandler(IReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductSalesReportDto>> Handle(GetProductSalesReportQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetProductsWithSalesAsync();

        return products.Select(p => new ProductSalesReportDto
        {
            ProductId = p.Productid,
            ProductName = p.Name,
            UnitPrice = p.Price,
            TotalQuantitySold = p.Orderdetails.Sum(od => od.Quantity),
            TotalRevenue = p.Orderdetails.Sum(od => od.Quantity * p.Price)
        }).ToList();
    }
}