namespace FoodFlowSystem.Services.SendMail
{
    public interface ISendMailService
    {
        Task SendMailAsync(string to, string subject, string body);
        Task SendMailWithAttachmentAsync(string to, string subject, string body, string attachmentPath);
        Task SendMailWithMultipleAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths);
    }
}
