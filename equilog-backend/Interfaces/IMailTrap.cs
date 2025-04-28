namespace equilog_backend.Interfaces;

public interface IMailTrap
{
    string Subject { get; set; }
    string Body { get; set; }
}