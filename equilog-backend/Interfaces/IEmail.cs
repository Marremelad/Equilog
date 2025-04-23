namespace equilog_backend.Interfaces;

public interface IEmail
{
    public string SenderName { get; set; }

    public string SenderEmail { get; set; }
    
    public string Subject { get; set; }

    public string PlainTextMessage { get; set; }

    public string HtmlMessage { get; set; }
}