using AutoMapper;

namespace equilog_backend.Mapping
{
    public class AutoMapperTest
    {
        public string AutoMapperConfigurationIsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
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
