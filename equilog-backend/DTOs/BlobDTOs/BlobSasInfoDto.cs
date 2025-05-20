namespace equilog_backend.DTOs.BlobDTOs
{
    public class BlobSasInfoDto
    {
        public string BlobName { get; set; } = string.Empty;
        public Uri SasUri { get; set; } = null!;
    }
}
