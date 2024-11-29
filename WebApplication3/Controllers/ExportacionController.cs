using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using WebApplication3.Models;
using System.Drawing;

namespace WebApplication3.Controllers
{
    public class ExportacionController : Controller
    {
        private readonly PlSubastas2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExportacionController(PlSubastas2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.CategoriaProducto)
                .Include(p => p.Subasta)
                .Include(p => p.ImagenesProductos)
                .ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> ExportarProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.CategoriaProducto)
                .Include(p => p.Subasta)
                .Include(p => p.ImagenesProductos)
                .ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Productos");

            ConfigurarEncabezados(worksheet);
            int row = 2;
            foreach (var producto in productos)
            {
                AgregarProducto(worksheet, producto, ref row);
            }

            worksheet.Cells.AutoFitColumns();

            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Productos_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
            );
        }

        private void ConfigurarEncabezados(ExcelWorksheet worksheet)
        {
            string[] headers = { "ID", "Nombre", "Descripción", "Precio", "Categoría", "Subasta", "Imagen" };
            int[] widths = { 10, 30, 40, 15, 20, 25, 30 };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Column(i + 1).Width = widths[i];
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        private void AgregarProducto(ExcelWorksheet worksheet, Producto producto, ref int row)
        {
            worksheet.Row(row).Height = 50;
            double imageHeight = worksheet.Row(row).Height * 0.9; // 90% of row height

            worksheet.Cells[row, 1].Value = producto.ProductoId;
            worksheet.Cells[row, 2].Value = producto.Nombre;
            worksheet.Cells[row, 3].Value = producto.Descripcion;
            worksheet.Cells[row, 4].Value = producto.Precio;
            worksheet.Cells[row, 5].Value = producto.CategoriaProducto?.Nombre;
            worksheet.Cells[row, 6].Value = producto.Subasta?.Titulo;

            if (!string.IsNullOrEmpty(producto.ImagenPrincipalUrl))
            {
                try
                {
                    var rutaImagen = Path.Combine(_webHostEnvironment.WebRootPath,
                        producto.ImagenPrincipalUrl.TrimStart('~', '/'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        var picture = worksheet.Drawings.AddPicture($"Imagen_{producto.ProductoId}", rutaImagen);
                        picture.SetPosition(row - 1, 0, 6, 0);
                        picture.SetSize(50, (int)imageHeight); // Fixed width, dynamic height
                    }
                    else
                    {
                        worksheet.Cells[row, 7].Value = "Sin imagen";
                    }
                }
                catch (Exception ex)
                {
                    worksheet.Cells[row, 7].Value = "Error de imagen";
                }
            }

            var range = worksheet.Cells[row, 1, row, 7];
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[row, 4].Style.Numberformat.Format = "$#,##0.00";

            row++;
        }
    }
}