using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    [Table("StableLocation")]
    public class StableLocation
    {
        [Key]
        public required string PostCode { get; set; }
        public required string City { get; set; }
        public required string MunicipalityName { get; set; }
        public required string MunicipalityCode { get; set; }
        public required string CountyName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public required string GoogleMaps { get; set; }
    }
}
