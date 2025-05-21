namespace equilog_backend.DTOs.StableLocationDtos
{
    public class StableLocationDto
    {
        public required string PostCode { get; init; }
        
        public required string City { get; init; }
        
        public required string MunicipalityName { get; init; }
        
        public required string CountyName { get; init; }
        
        public required double Latitude { get; init; }
        
        public required double Longitude { get; init; }
        
        public required string GoogleMaps { get; init; }
    }
}
