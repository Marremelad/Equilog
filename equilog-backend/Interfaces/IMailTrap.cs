namespace equilog_backend.Interfaces;

public interface IMailTrap
{
    public string Subject { get; set; }
    public string Body { get; set; }
}