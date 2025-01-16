// using System.Net;
// using System.Net.Mail;
// using System.Threading.Tasks;

// namespace carwash.Services
// {
//     public class EmailService
//     {
//         private readonly string _smtpServer;
//         private readonly int _smtpPort;
//         private readonly string _smtpUser;
//         private readonly string _smtpPass;

//         public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
//         {
//             _smtpServer = smtpServer;
//             _smtpPort = smtpPort;
//             _smtpUser = smtpUser;
//             _smtpPass = smtpPass;
//         }

//         public async Task SendEmailAsync(string toEmail, string subject, string body)
//         {
//             var smtpClient = new SmtpClient(_smtpServer)
//             {
//                 Port = _smtpPort,
//                 Credentials = new NetworkCredential(_smtpUser, _smtpPass),
//                 EnableSsl = true,
//             };

//             var mailMessage = new MailMessage
//             {
//                 From = new MailAddress(_smtpUser),
//                 Subject = subject,
//                 Body = body,
//                 IsBodyHtml = true,
//             };
//             mailMessage.To.Add(toEmail);

//             await smtpClient.SendMailAsync(mailMessage);
//         }
//     }
// }


using System.Net;
using System.Net.Mail;
 
public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _senderEmail;
    private readonly string _password;
 
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpServer = "smtp.gmail.com";
        _port = 587;
        _senderEmail = "mishraayush75584@gmail.com";
        _password = "wqcwovgespobpahw";
 
        if(string.IsNullOrEmpty(_smtpServer) || string.IsNullOrEmpty(_senderEmail) || string.IsNullOrEmpty(_password))
        {
            throw new ArgumentException("Email settings are not properly configured.");
        }
    }
 
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);
 
            using (var smtpClient = new SmtpClient(_smtpServer, _port))
            {
                smtpClient.Credentials = new NetworkCredential(_senderEmail, _password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        catch(Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while sending email, {ex.Message}", ex);
        }
    }
}