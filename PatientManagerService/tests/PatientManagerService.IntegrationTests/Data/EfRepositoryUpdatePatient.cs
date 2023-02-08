using PatientManagerService.Core.PatientAggregate;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PatientManagerService.IntegrationTests.Data;

public class EfRepositoryUpdatePatient : BaseEfRepoTestFixture
{
  private string _testFirstName = Guid.NewGuid().ToString();
  private string _testLasttName = "G";
  private string _testBirthdate = "8/8/1988";
  private string _testGender = "M";
  private Patient? _testPatient;

  private Patient CreatePatient() => new(_testFirstName, _testLasttName, _testBirthdate, _testGender);

  [Fact]
  public async Task UpdatesItemAfterAddingIt()
  {
    // add a patient
    _testPatient = CreatePatient();
    var repository = GetRepository();
    await repository.AddAsync(_testPatient);

    // detach the item so we get a different instance
    _dbContext.Entry(_testPatient).State = EntityState.Detached;

    // fetch the item and update its title
    var newPatient = (await repository.ListAsync())
        .FirstOrDefault(p => p.FirstName == _testFirstName);
    if (newPatient == null)
    {
      Assert.NotNull(newPatient);
      return;
    }
    Assert.NotSame(_testPatient, newPatient);
    var newFirstName = Guid.NewGuid().ToString();
    newPatient.Update(newFirstName, _testPatient.LastName, _testPatient.Birthday, _testPatient.Gender);

    // Update the item
    await repository.UpdateAsync(newPatient);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(p => p.FirstName == newFirstName);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(_testPatient.FirstName, updatedItem?.FirstName);
    Assert.Equal(_testPatient.LastName, updatedItem?.LastName);
    Assert.Equal(_testPatient.Birthday, updatedItem?.Birthday);
    Assert.Equal(newPatient.Id, updatedItem?.Id);
  }
}
