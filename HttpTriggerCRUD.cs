using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Sample.FunctionApp.Services;

namespace Sample.FunctionApp;

public class HttpTriggerCrud(ILogger<HttpTriggerCrud> logger, IOperation operation)
{

    [Function("RunPost")]
    public async Task<IActionResult> RunPost([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Insert")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var userDate= JsonSerializer.Deserialize<UserModel>(requestBody, options);
        
        logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var endpoint = Environment.GetEnvironmentVariable("CosmosEndpointUri");
        var dataBaseName = Environment.GetEnvironmentVariable("CosmosDatabaseName");
        var primarykey = Environment.GetEnvironmentVariable("CosmosPrimaryKey");
        
        CosmosClient client = new CosmosClient(endpoint, primarykey);
        
        Database db =await client.CreateDatabaseIfNotExistsAsync(dataBaseName);
        Container container = await db.CreateContainerIfNotExistsAsync("Users","/id");// client.GetContainer(dataBaseName, endpoint);
        await container.CreateItemAsync<UserModel>(userDate!);
        return new OkObjectResult("Data Inserted");

    }
    
    [Function("RunUpdate")]
    public async Task<IActionResult> RunUpdate([HttpTrigger(AuthorizationLevel.Function, "put", Route = "Update")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var userDate= JsonSerializer.Deserialize<UserModel>(requestBody, options);
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }
    
    [Function("RunDelete")]
    public async Task<IActionResult> RunDelete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "delete")] HttpRequest req)
    {
        string requestBody = req?.Query["userId"];

        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }
    
    [Function("RunGet")]
    public async Task<IActionResult> RunGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "get")] HttpRequest req)
    {
        string requestBody = req?.Query["userId"];

        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }
    
    [Function("RunAll")]
    public async Task<IActionResult> RunAll([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getall")] HttpRequest req)
    {
        var users = operation.GetAll();

        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }

}

public class UserModel
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string partitionKey { get; set; } = nameof(UserModel);
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public int Age { get; set; }
}