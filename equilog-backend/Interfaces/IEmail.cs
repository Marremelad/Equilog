namespace equilog_backend.Interfaces;

public interface IEmail
{
    string SenderName { get; set; }

    string SenderEmail { get; set; }
    
    string Subject { get; set; }

    string PlainTextMessage { get; set; }

    string HtmlMessage { get; set; }
}