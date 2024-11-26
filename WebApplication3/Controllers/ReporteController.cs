using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication3.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication3.Controllers
{
    public class SubastaReporte
    {
        public string Titulo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PrecioInicial { get; set; }
        public int CantidadPujas { get; set; }
        public decimal MontoMaximo { get; set; }
    }

    public class UsuarioReporte
    {
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public int SubastasParticipadas { get; set; }
        public int TotalPujas { get; set; }
        public decimal MontoTotalPujado { get; set; }
    }

    public class ReporteController : Controller
    {
        private readonly PlSubastas2Context _context;

        public ReporteController(PlSubastas2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        private void DibujarMarcaAgua(PdfContentByte cb, Document document)
        {
            float centerX = document.PageSize.Width / 2;
            float centerY = document.PageSize.Height / 2;

            // Configurar transparencia
            PdfGState gstate = new PdfGState();
            gstate.FillOpacity = 0.06f;
            cb.SetGState(gstate);

            // Colores
            BaseColor colorPrincipal = new BaseColor(41, 128, 185); // Azul profesional
            cb.SetRGBColorFill(colorPrincipal.R, colorPrincipal.G, colorPrincipal.B);
            cb.SetRGBColorStroke(colorPrincipal.R, colorPrincipal.G, colorPrincipal.B);

            // Círculos decorativos
           

            // Estrella decorativa
         
            

            // Texto circular
            String text = "• SISTEMA DE SUBASTAS • REPORTE OFICIAL • ";
            float radio = 240f;
            float fontSize = 14;

            cb.BeginText();
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, fontSize);

            for (int i = 0; i < text.Length; i++)
            {
                double angle = i * 360.0 / text.Length - 90;
                double angleRad = angle * Math.PI / 180.0;
                float x = centerX + radio * (float)Math.Cos(angleRad);
                float y = centerY + radio * (float)Math.Sin(angleRad);

                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                    text[i].ToString(), x, y,
                    (float)angle);
            }
            cb.EndText();

            // Martillo central
            cb.SaveState();
            cb.ConcatCTM(1, 0, 0, 1, centerX, centerY);
            cb.ConcatCTM(
                (float)Math.Cos(-45 * Math.PI / 180),
                (float)Math.Sin(-45 * Math.PI / 180),
                -(float)Math.Sin(-45 * Math.PI / 180),
                (float)Math.Cos(-45 * Math.PI / 180),
                0, 0
            );

            // Cabeza del martillo mejorada
            cb.MoveTo(-80, 30);
            cb.LineTo(80, 30);
            cb.LineTo(80, -10);
            cb.LineTo(-80, -10);
            cb.ClosePath();
            cb.Fill();

            // Mango decorativo
            cb.MoveTo(-10, -90);
            cb.LineTo(10, -90);
            cb.LineTo(10, 10);
            cb.LineTo(-10, 10);
            cb.ClosePath();
            cb.Fill();

            // Detalles decorativos
            cb.SetLineWidth(2);
            for (int i = -70; i < 70; i += 20)
            {
                cb.MoveTo(i, 25);
                cb.LineTo(i + 10, 25);
                cb.Stroke();
            }

            cb.RestoreState();
        }

        private void DibujarLogo(PdfContentByte cb, float x, float y)
        {
            cb.SaveState();

            // Colores
            BaseColor colorBase = new BaseColor(41, 128, 185);
            BaseColor colorOscuro = new BaseColor(31, 97, 141);

            // Escala y posición
            float scale = 0.4f;
            cb.ConcatCTM(1, 0, 0, 1, x, y);

            // Círculo base
            cb.SetRGBColorFill(colorBase.R, colorBase.G, colorBase.B);
            cb.Circle(25 * scale, 25 * scale, 25 * scale);
            cb.Fill();

            // Martillo estilizado
            cb.SaveState();
            cb.SetRGBColorFill(255, 255, 255);

            cb.ConcatCTM(
                (float)Math.Cos(-45 * Math.PI / 180),
                (float)Math.Sin(-45 * Math.PI / 180),
                -(float)Math.Sin(-45 * Math.PI / 180),
                (float)Math.Cos(-45 * Math.PI / 180),
                25 * scale,
                25 * scale
            );

            // Cabeza del martillo
            cb.Rectangle(-20 * scale, -5 * scale, 40 * scale, 10 * scale);
            cb.Fill();

            // Mango
            cb.Rectangle(-3 * scale, -25 * scale, 6 * scale, 25 * scale);
            cb.Fill();

            cb.RestoreState();

            // Texto "SUBASTA"
            cb.BeginText();
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, 8 * scale);
            cb.SetRGBColorFill(255, 255, 255);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "SUBASTA", 25 * scale, 10 * scale, 0);
            cb.EndText();

            cb.RestoreState();
        }

        private void AgregarContenidoComun(Document document, PdfWriter writer, string titulo)
        {
            // Dibujar marca de agua
            DibujarMarcaAgua(writer.DirectContentUnder, document);

            // Dibujar logo
            DibujarLogo(writer.DirectContent, 25, document.PageSize.Height - 60);

            // Añadir título
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
            Paragraph title = new Paragraph(titulo, titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            // Añadir fecha
            Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Paragraph date = new Paragraph($"Fecha del reporte: {DateTime.Now:dd/MM/yyyy HH:mm}", dateFont);
            date.Alignment = Element.ALIGN_RIGHT;
            date.SpacingAfter = 20f;
            document.Add(date);
        }

        public async Task<IActionResult> DescargarReporteSubastas()
        {
            try
            {
                var subastas = await _context.Subasta
                    .Include(s => s.Inscripcions)
                    .ThenInclude(i => i.Pujas)
                    .Select(s => new SubastaReporte
                    {
                        Titulo = s.Titulo,
                        FechaInicio = s.FechaInicio,
                        FechaFin = s.FechaFin,
                        PrecioInicial = s.PrecioInicial,
                        CantidadPujas = s.Inscripcions.SelectMany(i => i.Pujas).Count(),
                        MontoMaximo = s.Inscripcions.SelectMany(i => i.Pujas)
                            .DefaultIfEmpty()
                            .Max(p => p != null ? p.Monto : 0)
                    })
                    .ToListAsync();

                Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                MemoryStream ms = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                try
                {
                    document.Open();
                    AgregarContenidoComun(document, writer, "Reporte de Subastas");

                    // Crear tabla
                    PdfPTable table = new PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3f, 2f, 2f, 2f, 1.5f, 2f });

                    // Estilos
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                    BaseColor headerColor = new BaseColor(240, 240, 240);

                    // Encabezados
                    string[] headers = { "Título", "Fecha Inicio", "Fecha Fin", "Precio Inicial", "# Pujas", "Puja Máxima" };
                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                        cell.BackgroundColor = headerColor;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (var subasta in subastas)
                    {
                        table.AddCell(new PdfPCell(new Phrase(subasta.Titulo ?? "", contentFont)));
                        table.AddCell(new PdfPCell(new Phrase(subasta.FechaInicio.ToString("dd/MM/yyyy"), contentFont)));
                        table.AddCell(new PdfPCell(new Phrase(subasta.FechaFin.ToString("dd/MM/yyyy"), contentFont)));
                        table.AddCell(new PdfPCell(new Phrase(subasta.PrecioInicial.ToString("C"), contentFont))
                        { HorizontalAlignment = Element.ALIGN_RIGHT });
                        table.AddCell(new PdfPCell(new Phrase(subasta.CantidadPujas.ToString(), contentFont))
                        { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(subasta.MontoMaximo.ToString("C"), contentFont))
                        { HorizontalAlignment = Element.ALIGN_RIGHT });
                    }

                    document.Add(table);
                    document.Close();

                    byte[] bytes = ms.ToArray();
                    return File(bytes, "application/pdf", $"ReporteSubastas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error generando el PDF: " + ex.Message);
                }
                finally
                {
                    if (document.IsOpen())
                        document.Close();
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al generar el reporte: " + ex.Message);
            }
        }

        public async Task<IActionResult> DescargarReporteUsuarios()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.Inscripcions)
                    .ThenInclude(i => i.Pujas)
                    .Select(u => new UsuarioReporte
                    {
                        NombreCompleto = u.Nombre + " " + u.Apellido,
                        Email = u.Email,
                        SubastasParticipadas = u.Inscripcions.Count(),
                        TotalPujas = u.Inscripcions.SelectMany(i => i.Pujas).Count(),
                        MontoTotalPujado = u.Inscripcions.SelectMany(i => i.Pujas)
                            .Sum(p => p.Monto)
                    })
                    .ToListAsync();

                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                MemoryStream ms = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                try
                {
                    document.Open();
                    AgregarContenidoComun(document, writer, "Reporte de Usuarios");

                    // Crear tabla
                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3f, 3f, 2f, 2f, 2.5f });

                    // Estilos
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                    BaseColor headerColor = new BaseColor(240, 240, 240);
                    string[] headers = { "Nombre", "Email", "Subastas", "Total Pujas", "Monto Total" };
                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                        cell.BackgroundColor = headerColor;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 8;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (var usuario in usuarios)
                    {
                        table.AddCell(new PdfPCell(new Phrase(usuario.NombreCompleto ?? "", contentFont)));
                        table.AddCell(new PdfPCell(new Phrase(usuario.Email ?? "", contentFont)));
                        table.AddCell(new PdfPCell(new Phrase(usuario.SubastasParticipadas.ToString(), contentFont))
                        { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(usuario.TotalPujas.ToString(), contentFont))
                        { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(usuario.MontoTotalPujado.ToString("C"), contentFont))
                        { HorizontalAlignment = Element.ALIGN_RIGHT });
                    }

                    document.Add(table);
                    document.Close();

                    byte[] bytes = ms.ToArray();
                    return File(bytes, "application/pdf", $"ReporteUsuarios_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error generando el PDF: " + ex.Message);
                }
                finally
                {
                    if (document.IsOpen())
                        document.Close();
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al generar el reporte: " + ex.Message);
            }
        }
    }
}

