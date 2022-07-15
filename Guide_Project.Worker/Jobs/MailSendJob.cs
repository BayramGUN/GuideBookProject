using System.Net;
using System.Net.Mail;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Quartz;
using RabbitMQ.Client;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Guide_Project.Worker.Jobs;

public class MailSendJob : IJob
{
    public IModel _channel;
    private readonly ISendGridClient _sendGridClient;
    private readonly ILogger _logger;
 
    public MailSendJob(ILogger<MailSendJob> logger, ISendGridClient sendGridClient)
    {
        _logger = logger;
        _sendGridClient = sendGridClient;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("[REPLACE WITH YOUR EMAIL]", "[REPLACE WITH YOUR NAME]"),
            Subject = "Sending with Twilio SendGrid is Fun",
            PlainTextContent = "and easy to do anywhere, especially with C# .NET"
        };
        var baseUrl = "https://localhost:7081";
        var reportFile = new ReportFileDto();
        WebClient client = new WebClient();
        msg.AddAttachment(reportFile.FileName, client.DownloadFileAsync(baseUrl + reportFile.FileName, reportFile.FileName)); 
        msg.AddTo(new EmailAddress("[REPLACE WITH DESIRED TO EMAIL]", "[REPLACE WITH DESIRED TO NAME]"));

        var response = await _sendGridClient.SendEmailAsync(msg);

        // A success status code means SendGrid received the email request and will process it.
        // Errors can still occur when SendGrid tries to send the email. 
        // If email is not received, use this URL to debug: https://app.sendgrid.com/email_activity 
        _logger.LogInformation(response.IsSuccessStatusCode ? "Email queued successfully!" : "Something went wrong!");
    }
}