using PatientManagerService.Core.PatientAggregate;
using Xunit;

namespace PatientManagerService.UnitTests.Core.ProjectAggregate;

public class PatientConstructor
{
  private string _testFirstName = "Cliff";
  private string _testLasttName = "G";
  private string _testBirthdate = "8/8/1988";
  private string _testGender = "M";
  private Patient? _testPatient;

  private Patient CreatePatient()
  {
    return new Patient(_testFirstName, _testLasttName, _testBirthdate, _testGender);
  }

  [Fact]
  public void InitializesFirstName()
  {
    _testPatient = CreatePatient();

    Assert.Equal(_testFirstName, _testPatient.FirstName);
  }

  [Fact]
  public void InitializesLastName()
  {
    _testPatient = CreatePatient();

    Assert.Equal(_testLasttName, _testPatient.LastName);
  }

  [Fact]
  public void InitializesBirthday()
  {
    _testPatient = CreatePatient();

    Assert.Equal(_testBirthdate, _testPatient.Birthday);
  }

  [Fact]
  public void InitializesBirthdayVariant()
  {
    var variant = "1/1/88";
    var patient = new Patient(_testFirstName, _testLasttName, "1/1/88", _testGender);

    Assert.Equal(variant, patient.Birthday);
  }

  [Fact]
  public void DoesNotInitializeInvalidBirthday()
  {
    Assert.Throws<Exception>(() => new Patient(_testFirstName, _testLasttName, "1/1", _testGender));    
  }

  [Fact]
  public void InitializesGender()
  {
    _testPatient = CreatePatient();

    Assert.Equal(_testGender, _testPatient.Gender);
  }


}
