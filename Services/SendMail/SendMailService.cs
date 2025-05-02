using FoodFlowSystem.DTOs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;

namespace FoodFlowSystem.Services.SendMail
{
    public class SendMailService : BaseService, ISendMailService
    {
        private readonly SendMailConfig _sendMailConfig;

        public SendMailService(IHttpContextAccessor httpContextAccessor, IOptions<SendMailConfig> options) : base(httpContextAccessor)
        {
            _sendMailConfig = options.Value;
        }

        public async Task SendMailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_sendMailConfig.FromName, _sendMailConfig.FromEmail));
            message.To.Add(new MailboxAddress("Khách hàng", to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();
            try
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await smtpClient.ConnectAsync(_sendMailConfig.SmtpServer, _sendMailConfig.SmtpPort);
                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtpClient.AuthenticateAsync(_sendMailConfig.FromEmail, _sendMailConfig.SmtpPass);
                await smtpClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new ApiException("Lỗi khi gửi mail.", 500, ex);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }

        public Task SendMailWithAttachmentAsync(string to, string subject, string body, string attachmentPath)
        {
            throw new NotImplementedException();
        }

        public Task SendMailWithMultipleAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths)
        {
            throw new NotImplementedException();
        }
    }
}
