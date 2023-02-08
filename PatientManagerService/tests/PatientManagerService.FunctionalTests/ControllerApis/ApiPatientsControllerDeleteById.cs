using Ardalis.HttpClientTestExtensions;
using PatientManagerService.Core.PatientAggregate;
using PatientManagerService.Web;
using PatientManagerService.Web.ApiModels;
using Xunit;

namespace PatientManagerService.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ApiPatientsControllerDeleteById : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ApiPatientsControllerDeleteById(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task DeletePatientById()
  {
    //TODO: Guid
    int patientId = SeedData.Patient1.Id;

    //TODO: Volitile, not ideal, consider TestPriority(x)
    var result = await _client.DeleteAsync($"api/patients/{patientId}");

    Assert.True(result.IsSuccessStatusCode);
  }
}
