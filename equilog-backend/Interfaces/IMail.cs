namespace equilog_backend.Interfaces;

public interface IMail
{
    public string Recipient { get; set; }
    
    public string Subject { get; set; }
    public string Body { get; set; }
}