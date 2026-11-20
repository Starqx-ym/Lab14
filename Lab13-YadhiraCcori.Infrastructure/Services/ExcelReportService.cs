using ClosedXML.Excel;
using Lab13_YadhiraCcori.Application.Interfaces;
using Lab13_YadhiraCcori.Application.UsesCases.Reports;
using MediatR;

namespace Lab13_YadhiraCcori .Infrastructure.Services;

public class ExcelReportService : IExcelReportService
{
    private readonly IMediator _mediator;

    public ExcelReportService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<byte[]> GenerateClientOrdersExcelAsync()
    {
        // 1. Llamar al Use Case (CQRS)
        var data = await _mediator.Send(new GetClientOrdersReportQuery());

        // 2. Crear libro Excel con ClosedXML
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Clientes y Pedidos");

        // Cabeceras
        worksheet.Cell(1, 1).Value = "ID Cliente";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Correo";
        worksheet.Cell(1, 4).Value = "Total Pedidos";
        worksheet.Cell(1, 5).Value = "Total Gastado ($)";

        // Estilos de cabecera (Parte 5 de la guía)
        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Llenar filas
        int row = 2;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.ClientId;
            worksheet.Cell(row, 2).Value = item.ClientName;
            worksheet.Cell(row, 3).Value = item.Email;
            worksheet.Cell(row, 4).Value = item.TotalOrders;
            worksheet.Cell(row, 5).Value = item.TotalSpent;
            worksheet.Cell(row, 5).Style.NumberFormat.Format = "$ #,##0.00";
            row++;
        }

        // Crear tabla y ajustar columnas (Parte 4 de la guía)
        if (data.Any())
        {
            var range = worksheet.Range(1, 1, row - 1, 5);
            range.CreateTable();
        }
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GenerateProductSalesExcelAsync()
    {
        // 1. Llamar al Use Case (CQRS)
        var data = await _mediator.Send(new GetProductSalesReportQuery());

        // 2. Crear libro Excel con ClosedXML
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Ventas por Producto");

        // Cabeceras
        worksheet.Cell(1, 1).Value = "ID Producto";
        worksheet.Cell(1, 2).Value = "Producto";
        worksheet.Cell(1, 3).Value = "Precio Unitario ($)";
        worksheet.Cell(1, 4).Value = "Cantidad Vendida";
        worksheet.Cell(1, 5).Value = "Ingresos Totales ($)";

        // Estilos de cabecera
        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGreen;
        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Llenar filas
        int row = 2;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.ProductId;
            worksheet.Cell(row, 2).Value = item.ProductName;
            worksheet.Cell(row, 3).Value = item.UnitPrice;
            worksheet.Cell(row, 3).Style.NumberFormat.Format = "$ #,##0.00";
            worksheet.Cell(row, 4).Value = item.TotalQuantitySold;
            worksheet.Cell(row, 5).Value = item.TotalRevenue;
            worksheet.Cell(row, 5).Style.NumberFormat.Format = "$ #,##0.00";
            row++;
        }

        if (data.Any())
        {
            var range = worksheet.Range(1, 1, row - 1, 5);
            range.CreateTable();
        }
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}