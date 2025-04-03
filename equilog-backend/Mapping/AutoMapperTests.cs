using AutoMapper;

namespace equilog_backend.Mapping
{
    public class AutoMapperTests
    {
        public string AutoMapperConfigurationIsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HorseProfile>();
            });

            try
            {
                config.AssertConfigurationIsValid();
                return "AutoMapper configuration is valid.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
