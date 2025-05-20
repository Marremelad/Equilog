namespace equilog_backend.DTOs.BlobStorageDTOs;

public class BlobStorageDto
{
    public required int UserId { get; set; }
    
    public required string BlobName  { get; set; }
}