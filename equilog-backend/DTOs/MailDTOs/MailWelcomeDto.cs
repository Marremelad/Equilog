using equilog_backend.Interfaces;

namespace equilog_backend.DTOs.MailDTOs;

public class MailWelcomeDto : IMail
{
    public string Subject { get; set; } = "Welcome to Equilog";
    public string Body { get; set; } = "Hello and welcome to the Equilog app!";
}