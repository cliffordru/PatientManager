
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Ardalis.HttpClientTestExtensions;
using Ardalis.Result;
using PatientManagerService.Web;
using PatientManagerService.Web.ApiModels;
using Xunit;

namespace PatientManagerService.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ApiPatientsControllerUpdate : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ApiPatientsControllerUpdate(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task UpdatePatient()
  {
    //TODO: Guid
    var patient = SeedData.Patient1;
    var testNewFirstName = Guid.NewGuid().ToString().Substring(0,30);
    var updatedPatient = new PatientDTO(patient.Id, testNewFirstName, patient.LastName, patient.Birthday, patient.Gender);

    //var response = await _client.PatchAsJsonAsync<PatientDTO>($"api/patients/{patient.Id}", GetJsonStringContent(updatedPatient));
    var response = await _client.PutAsJsonAsync<PatientDTO>($"api/patients/{patient.Id}", updatedPatient);

    Assert.True(response.IsSuccessStatusCode);
  }

  private static StringContent GetJsonStringContent<T>(T model)
    => new(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
}
