using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ToDoApp.Tests.Integration;

public class ApiIntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public ApiIntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }
}
