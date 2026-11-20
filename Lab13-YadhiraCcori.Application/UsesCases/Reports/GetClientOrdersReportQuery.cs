using Lab13_YadhiraCcori.Application.DTOs;
using Lab13_YadhiraCcori.Domain.Interfaces;
using MediatR;

namespace Lab13_YadhiraCcori.Application.UsesCases.Reports;

public record GetClientOrdersReportQuery() : IRequest<List<ClientOrdersReportDto>>;

public class GetClientOrdersReportHandler : IRequestHandler<GetClientOrdersReportQuery, List<ClientOrdersReportDto>>
{
    private readonly IReportRepository _repository;

    public GetClientOrdersReportHandler(IReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ClientOrdersReportDto>> Handle(GetClientOrdersReportQuery request, CancellationToken cancellationToken)
    {
        var clients = await _repository.GetClientsWithOrdersAsync();

        return clients.Select(c => new ClientOrdersReportDto
        {
            ClientId = c.Clientid,
            ClientName = c.Name,
            Email = c.Email,
            TotalOrders = c.Orders.Count,
            TotalSpent = c.Orders
                .SelectMany(o => o.Orderdetails)
                .Sum(od => od.Quantity * od.Product.Price)
        }).ToList();
    }
}