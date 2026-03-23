using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Sample.FunctionApp;

public class FileUploadBlobTrigger
{
    private readonly ILogger<FileUploadBlobTrigger> _logger;

    public FileUploadBlobTrigger(ILogger<FileUploadBlobTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(FileUploadBlobTrigger))]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function,"post")]HttpRequest req)
    {
        var file =req.Form.Files["datafile"];
        BlobServiceClient blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
       // blobServiceClient.CreateBlobContainer(Environment.GetEnvironmentVariable("ContainerName"));
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("ContainerName"));
        containerClient.CreateIfNotExists();
        
        var blob = containerClient.GetBlobClient(file?.FileName);
       
       using var stream=file?.OpenReadStream();
       await blob.UploadAsync(stream, overwrite:true);
        
        _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {file.ContentType} \n Data: {file.Name}");
        
    }
   [Function("triggerupload")]
    public static async Task ReadFile([BlobTrigger("upload/{name}", Connection = "")] Stream req, string name)
    {
        string imgUrl = $"https://{ Environment.GetEnvironmentVariable("StorageAccountName")}.blob.core.windows.net/{Environment.GetEnvironmentVariable("ContainerName")}/{name}";
    }
 
}

