using AutoMapper;

namespace equilog_backend.Mapping
{
    public class AutoMapperTests
    {
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HorseProfile>();
            });

            try
            {
                config.AssertConfigurationIsValid();
                Console.WriteLine("AutoMapper configuration is valid.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
