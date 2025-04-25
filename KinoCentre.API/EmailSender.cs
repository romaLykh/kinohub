using MailKit.Net.Smtp;
using MimeKit;

namespace KinoCentre;

public static class EmailSender
{
    public static async Task SendEmailAsync(string email, string subject, string message, string filePath)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("KinoHub", "maksimsevcenko250@gmail.com"));
        mailMessage.To.Add(new MailboxAddress(email, email));
        mailMessage.Subject = subject;
    
   
        var bodyBuilder = new BodyBuilder
        {
            TextBody = message
        };
    
       
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            bodyBuilder.Attachments.Add(filePath);
        }
    
        mailMessage.Body = bodyBuilder.ToMessageBody();
    
        using (var smtpClient = new SmtpClient())
        {
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate("maksimsevcenko250@gmail.com", "mqds jyhm wmzk urjk");
            smtpClient.Send(mailMessage);
            smtpClient.Disconnect(true);
        }
    }
}