namespace Lab13_YadhiraCcori.Application.Interfaces;

public interface IExcelReportService
{
    Task<byte[]> GenerateClientOrdersExcelAsync();
    Task<byte[]> GenerateProductSalesExcelAsync();
}