using PatientManagerService.Core.PatientAggregate;
using Xunit;

namespace PatientManagerService.IntegrationTests.Data;

public class EfRepositoryDeletePatient : BaseEfRepoTestFixture
{
  private string _testFirstName = Guid.NewGuid().ToString();
  private string _testLasttName = "G";
  private string _testBirthday = "8/8/1988";
  private string _testGender = "M";
  private Patient? _testPatient;

  private Patient CreatePatient() => new(_testFirstName, _testLasttName, _testBirthday, _testGender);

  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    // add a patient
    _testPatient = CreatePatient();
    var repository = GetRepository();    
    await repository.AddAsync(_testPatient);

    // delete the item
    await repository.DeleteAsync(_testPatient);

    // verify it's no longer there
    Assert.DoesNotContain(await repository.ListAsync(),
        p => p.FirstName == _testFirstName);
  }
}
