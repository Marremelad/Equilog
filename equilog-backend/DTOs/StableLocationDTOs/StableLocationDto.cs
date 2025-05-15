namespace equilog_backend.DTOs.StableLocationDtos
{
    public class StableLocationDto
    {
        public required string PostCode { get; set; }
        public required string City { get; set; }
        public required string MunicipalityName { get; set; }
        public required string CountyName { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public required string GoogleMaps { get; set; }
    }
}
