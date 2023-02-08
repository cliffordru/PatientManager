using Ardalis.HttpClientTestExtensions;
using PatientManagerService.Web;
using PatientManagerService.Web.ApiModels;
using Xunit;

namespace PatientManagerService.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class PatientCreate : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public PatientCreate(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOnePatient()
  {
    var result = await _client.GetAndDeserializeAsync<IEnumerable<PatientDTO>>("/api/patients");

    Assert.Single(result);
    Assert.Contains(result, i => i.FirstName == SeedData.Patient1.FirstName);
  }
}
