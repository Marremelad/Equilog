using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace equilog_backend.Security;

public class AzuriteStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var blobServiceClient = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
                
                // Configure CORS
                try
                {
                    // Get current properties
                    BlobServiceProperties serviceProperties = blobServiceClient.GetProperties();
                    
                    // Create CORS rule
                    var corsRule = new BlobCorsRule
                    {
                        AllowedOrigins = "*",
                        AllowedMethods = "GET,PUT,POST,DELETE,HEAD,OPTIONS",
                        AllowedHeaders = "Authorization,Content-Type,Accept,Origin,User-Agent,x-ms-*",
                        ExposedHeaders = "x-ms-*",
                        MaxAgeInSeconds = 3600
                    };
                    
                    // Set the CORS rules
                    serviceProperties.Cors = new List<BlobCorsRule> { corsRule };
                    
                    // Update the properties
                    blobServiceClient.SetProperties(serviceProperties);
                    
                    Console.WriteLine("CORS policy configured for Azure Blob Storage");
                }
                catch (Exception ex)
                {
                    // Log the error but don't crash the application
                    Console.WriteLine($"Failed to configure CORS for Azure Blob Storage: {ex.Message}");
                }
                
                // Ensure the container exists with proper access
                try
                {
                    var containerClient = blobServiceClient.GetBlobContainerClient("equilog-media");
                    containerClient.CreateIfNotExists(PublicAccessType.Blob);
                    Console.WriteLine("Container 'equilog-media' is ready");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create container: {ex.Message}");
                }
            }
            
            next(app);
        };
    }
}