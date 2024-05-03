using System.Net.Mail;
using System.Net;
using BulkEmailSender.Helpers;

namespace BulkEmailSender.Services
{
    public class EmailService
    {
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public EmailService(string host, int port, string username, string password)
        {
            smtpHost = host;
            smtpPort = port;
            smtpUsername = username;
            smtpPassword = password;
        }

        public async Task SendBulkEmailsAsync(Recipients recipients, string subject, string body, string templatePath = null)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    // Batching: Define batch size for better performance
                    int batchSize = 50; // Send emails in batches of 50
                    int totalRecipients = recipients.GmailAddresses.Count;
                    for (int i = 0; i < totalRecipients; i += batchSize)
                    {
                        var batchRecipients = recipients.GmailAddresses.GetRange(i, Math.Min(batchSize, totalRecipients - i));
                        await SendBatchEmailAsync(smtpClient, batchRecipients, subject, body, templatePath);
                    }

                    Console.WriteLine($"Bulk emails sent successfully to {totalRecipients} recipients.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending bulk emails: {ex.Message}");
            }
        }

        private async Task SendBatchEmailAsync(SmtpClient smtpClient, List<string> recipients, string subject, string body, string templatePath)
        {
            foreach (var recipient in recipients)
            {
                string emailBody = body;
                if (templatePath != null && File.Exists(templatePath))
                {
                    string htmlTemplate = File.ReadAllText(templatePath);
                    emailBody = htmlTemplate
                        .Replace("{subject}", subject)
                        .Replace("{content}", body);
                }

                MailMessage mailMessage = new MailMessage(smtpUsername, recipient, subject, emailBody);
                mailMessage.IsBodyHtml = true;

                // Throttling: Introduce a delay between sending each email to avoid rate limiting
                int delayMilliseconds = 100; // 100 milliseconds delay between emails
                await Task.Delay(delayMilliseconds);

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"Email sent successfully to {recipient}.");
            }
        }
    }
}