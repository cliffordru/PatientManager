using Ardalis.HttpClientTestExtensions;
using PatientManagerService.Web;
using PatientManagerService.Web.ApiModels;
using Xunit;

namespace PatientManagerService.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ApiPatientsControllerGetById : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ApiPatientsControllerGetById(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GetPatientById()
  {
    //TODO: Guid
    int patientId = SeedData.Patient1.Id;

    var result = await _client.GetAndDeserializeAsync<PatientDTO>($"api/patients/{patientId}");

    Assert.True(result.FirstName == SeedData.Patient1.FirstName);
  }
}
