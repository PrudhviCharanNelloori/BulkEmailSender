using BulkEmailSender.Helpers;
using BulkEmailSender.Services;

string smtpHost = "smtp.gmail.com";
int smtpPort = 587;
string smtpUsername = Constants.SmtpUsername;
string smtpPassword = Constants.SmtpPassword;

EmailService emailService = new EmailService(smtpHost, smtpPort, smtpUsername, smtpPassword);
Recipients recipients = new Recipients();

Console.Write("Enter Subject: ");
string subject = Console.ReadLine();
Console.Write("Enter body content: ");
string content = Console.ReadLine();
string templatePath = "./"; 

await emailService.SendBulkEmailsAsync(recipients, subject, content, templatePath);