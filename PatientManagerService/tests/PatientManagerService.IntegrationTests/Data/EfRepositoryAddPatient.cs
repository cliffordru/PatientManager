using PatientManagerService.Core.PatientAggregate;
using Xunit;

namespace PatientManagerService.IntegrationTests.Data;

public class EfRepositoryAddPatient : BaseEfRepoTestFixture
{
  private string _testFirstName = Guid.NewGuid().ToString();
  private string _testLasttName = "G";
  private string _testBirthday = "8/8/1988";
  private string _testGender = "M";
  private Patient? _testPatient;

  private Patient CreatePatient() => new(_testFirstName, _testLasttName, _testBirthday, _testGender);

  [Fact]
  public async Task AddsPatientAndSetsId()
  {
    _testPatient = CreatePatient();
    var repository = GetRepository();

    await repository.AddAsync(_testPatient);

    var newPatient = (await repository.ListAsync())
                    .FirstOrDefault();

    Assert.Equal(_testFirstName, newPatient?.FirstName);
    Assert.Equal(_testLasttName, newPatient?.LastName);
    //TODO: Test guid
    //Assert.True(newPatientt?.Id != Guid.Empty);
  }
}
