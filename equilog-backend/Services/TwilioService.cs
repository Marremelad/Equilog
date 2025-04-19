using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;

namespace equilog_backend.Services
{
    public class TwilioService(string twilioVerifySid)
    {
        public async Task<bool> SendVerificationCodeAsync(string userEmail)
        {
            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: userEmail,
                    channel: "Email",
                    pathServiceSid: twilioVerifySid
                );
                return verification.Status == "pending";
            }
            catch (ApiException ex)
            {
                Console.Error.WriteLine($"Twilio API error (Code={ex.Code}): {ex.Message}");
                throw new Exception($"Twilio error sending code: {ex.Message}", ex);
            }
        }

        public async Task<bool> VerifyCodeAsync(string userEmail, string code)
        {
            try
            {
                var verificationCheck = await VerificationCheckResource.CreateAsync(
                    to: userEmail,
                    code: code,
                    pathServiceSid: twilioVerifySid
                );
                return verificationCheck.Status == "approved";
            }
            catch (ApiException)
            {
                return false;
            }
        }
    }
}
