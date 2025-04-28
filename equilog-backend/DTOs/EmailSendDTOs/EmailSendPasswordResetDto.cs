using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.DTOs.EmailSendDTOs;

public class EmailSendPasswordResetDto(PasswordResetRequestDto? passwordResetDto) : IEmail
{
    public string SenderName { get; set; } = "Equilog";
    public string SenderEmail { get; set; } = "pontus.dorsay@outlook.com";
    public string Subject { get; set; } = "Equilog password reset";

    public string PlainTextMessage { get; set; } =
        $"Hi! You have requested a password reset for your Equilog account.\nUse this code to reset your password {passwordResetDto?.ResetCode}.\nThe code will expire {passwordResetDto?.ExpirationDate}\n If you did not request a password reset you can ignore this email.";

    public string HtmlMessage { get; set; } =
        $"<!DOCTYPE html>\n<html>\n<head>\n  <meta charset=\"UTF-8\">\n  <title>Equilog password reset</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;\">\n  <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); overflow: hidden;\">\n    <tr>\n      <td style=\"background-color: #4CAF50; padding: 20px; text-align: center;\">\n        <h1 style=\"color: #ffffff; margin: 0;\">Equilog Password Reset</h1>\n      </td>\n    </tr>\n    <tr>\n      <td style=\"padding: 30px;\">\n        <p style=\"font-size: 18px; color: #333;\">Hi there 👋</p>\n        <p style=\"font-size: 16px; color: #555;\">\n          Hi! You have requested a password reset for your Equilog account.\n        </p>\n        <p style=\"font-size: 16px; color: #555;\">\n          Use this code to reset your password.\n        </p>\n        <p style=\"font-size: 24px; color: #4CAF50; font-weight: bold; text-align: center; padding: 15px; background-color: #f0f0f0; border-radius: 5px; letter-spacing: 2px;\">\n          {passwordResetDto?.ResetCode}\n        </p>\n        <p style=\"font-size: 16px; color: #555;\">\n          The code will expire {passwordResetDto?.ExpirationDate}\n        </p>\n        <p style=\"font-size: 16px; color: #555;\">\n          If you did not request a password reset you can ignore this email.\n        </p>\n        <p style=\"font-size: 16px; color: #4CAF50; font-weight: bold;\">\n          Thank you,<br>The Equilog Team 🐎\n        </p>\n      </td>\n    </tr>\n    <tr>\n      <td style=\"padding: 20px; text-align: center; background-color: #f0f0f0; font-size: 12px; color: #999;\">\n        &copy; 2025 Equilog. All rights reserved.<br>\n        You received this email because you have an account with Equilog.\n      </td>\n    </tr>\n  </table>\n</body>\n</html>";

}