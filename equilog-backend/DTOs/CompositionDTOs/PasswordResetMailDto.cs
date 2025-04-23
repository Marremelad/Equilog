using equilog_backend.DTOs.MailTrapDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;

namespace equilog_backend.DTOs.CompositionDTOs;

public class PasswordResetMailDto
{
    public required PasswordResetRequestDto PasswordResetRequestDto { get; set; }
    public required MailTrapPasswordResetDto MailTrapPasswordResetDto { get; set; }
}