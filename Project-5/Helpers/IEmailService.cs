namespace Project_5.Helpers
{
    public interface IEmailService
    {
        Task SendEmailWithAttachmentAsync(string email, string subject, string messageBody, string attachmentFilePath);
    }
}
