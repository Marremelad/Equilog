using equilog_backend.Interfaces;

namespace equilog_backend.DTOs.MailDTOs;

public class MailTwilioDto : IMail
{
    public string Subject { get; set; } = "Thanks for using Twilio.";
    public string Body { get; set; } = "";
}