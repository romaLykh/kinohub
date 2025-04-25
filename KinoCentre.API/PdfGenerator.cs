// c#

using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using KinoCentre.DB.Entities;
using MongoDB.Driver.Linq;

namespace KinoCentre;

public static class PdfGenerator
{
    public static void GeneratePdf(string filePath, Ticket ticket)
    {
        // Get the directory from the filePath
        var directory = Path.GetDirectoryName(filePath);
        
        // Ensure the directory exists if the directory path is valid
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Create a PDF writer
        using var writer = new PdfWriter(filePath);

        // Create a PDF document
        using var pdf = new PdfDocument(writer);


        // Create a document layout
        var document = new Document(pdf);
        
        document.SetBackgroundColor(ColorConstants.BLACK);

        
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        
        // Add an image
        var imagePath = ticket.Session.Movie.PosterUrl;
        var imageData = ImageDataFactory.Create(imagePath);
        var image = new Image(imageData)
            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
            .ScaleToFit(320, 180);
        
        document.Add(image);
        
        // Заголовок
        var title = new Paragraph("TICKET DETAILS")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(24);

        var sessionDate =
            new Paragraph(ticket.Session.SessionDateTime.ToString("dd.MM.yyyy HH:mm"))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16)
                .SetFont(boldFont);
        
        var movieTitle = new Paragraph(ticket.Session.Movie.Title)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(16)
            .SetFont(boldFont);
        
        var seat = new Paragraph(ticket.Seat.ToString())
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(16)
            .SetFont(boldFont);
        
        document.Add(sessionDate);
        
        document.Add(seat);
        
        document.Add(movieTitle);
        
        document.Add(title);
        
        
        document.Add(new Paragraph("\n"));

        
        var table = new Table(2).UseAllAvailableWidth();
        
        table.AddCell(new Cell().Add(new Paragraph("Seat").SetFont(boldFont)));
        table.AddCell(new Cell().Add(
            new Paragraph($"Row - {ticket.Seat.Row}, Seat - {ticket.Seat.Number}")
                .SetFont(regularFont)));
        
     table.AddCell(new Cell().Add(new Paragraph("Session date").SetFont(boldFont)));
     table.AddCell(new Cell().Add(
         new Paragraph($"{ticket.Session.SessionDateTime:dd.MM.yyyy HH:mm}")
             .SetFont(regularFont)));
        
        table.AddCell(new Cell().Add(
            new Paragraph($"Movie:").SetFont(boldFont)));
        
        table.AddCell(new Cell().Add(
            new Paragraph($"Movie: {ticket.Session.Movie.Title}")
                .SetFont(regularFont)));
        
        table.AddCell(new Cell().Add(new Paragraph("Email").SetFont(boldFont)));
        table.AddCell(new Cell().Add(new Paragraph(ticket.Email).SetFont(regularFont)));
        
        table.AddCell(new Cell().Add(new Paragraph("Phone").SetFont(boldFont)));
        table.AddCell(new Cell().Add(new Paragraph(ticket.Phone ?? "N/A").SetFont(regularFont)));

        table.AddCell(new Cell().Add(new Paragraph("Ticket ID").SetFont(boldFont)));
        table.AddCell(new Cell().Add(new Paragraph(ticket.Id).SetFont(regularFont)));
        
        table.AddCell(new Cell().Add(new Paragraph("Session ID").SetFont(boldFont)));
        table.AddCell(new Cell().Add(new Paragraph(ticket.SessionId).SetFont(regularFont)));
        
        document.Add(table);
        
        
        // Add a footer
       var footerTable = new Table(2).UseAllAvailableWidth();

       footerTable.SetMarginTop(50);
       
       footerTable.AddCell(new Cell().Add(new Paragraph("Thank you for choosing us!")
           .SetTextAlignment(TextAlignment.LEFT)
           .SetFontSize(12)
           .SetFontColor(ColorConstants.GRAY))
           .SetBorder(Border.NO_BORDER));
       
       footerTable.AddCell(new Cell().Add(new Paragraph("KinoHub")
           .SetTextAlignment(TextAlignment.RIGHT)
           .SetFontSize(12)
           .SetFontColor(ColorConstants.GRAY))
           .SetBorder(Border.NO_BORDER));
       
       footerTable.AddCell(new Cell().Add(new Paragraph(DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm"))
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(12)
               .SetFontColor(ColorConstants.GRAY))
           .SetBorder(Border.NO_BORDER));
       
       document.Add(footerTable);
        
        
        
        document.Close();
    }
}