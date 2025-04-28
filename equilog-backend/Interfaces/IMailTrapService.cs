using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IMailTrapService
{
    public ApiResponse<Unit> SendEmail(IMailTrap mailTrap, string recipient);
}