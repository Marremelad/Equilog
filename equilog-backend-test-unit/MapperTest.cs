using AutoMapper;
using equilog_backend.Common;

namespace equilog_backend_test_unit
{
    public class MapperTest
    {
        [Fact]
        public void Mapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            config.AssertConfigurationIsValid(); // Throws if any map is misconfigured.
        }
    }
}
