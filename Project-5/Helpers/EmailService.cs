using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace Project_5.Helpers
{
    public class EmailService:IEmailService
    {
        public async Task SendEmailWithAttachmentAsync(string email, string subject, string messageBody, string attachmentFilePath)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("elgunpheonix@gmail.com", "lorf fqhf icvn gftt");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage("elgunpheonix@gmail.com", email);
            message.Subject = subject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            Attachment attachment = new Attachment(attachmentFilePath, MediaTypeNames.Application.Pdf);
            message.Attachments.Add(attachment);

            await client.SendMailAsync(message);

            attachment.Dispose();
        }
    }
}
