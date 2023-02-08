using Ardalis.GuardClauses;
using PatientManagerService.SharedKernel;
using PatientManagerService.SharedKernel.Interfaces;

namespace PatientManagerService.Core.PatientAggregate;

public class Patient : EntityBase, IAggregateRoot
{
  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string Birthday { get; set; }

  public string Gender { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  public Patient(string firstName, string lastName, string birthday, string gender)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  {
    SetPatientProperties(firstName, lastName, birthday, gender);
  }

  public void Update(string newFirstName, string newLastName, string newBirthday, string newGender)
  {
    SetPatientProperties(newFirstName, newLastName, newBirthday, newGender);
  }

  private void SetPatientProperties(string firstName, string lastName, string birthday, string gender)
  {
    //TODO: temp exception for bad date
    if (!IsValidDate(birthday))
      throw new Exception("Invalid Date");
    FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
    LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
    Birthday = Guard.Against.NullOrEmpty(birthday, nameof(birthday));
    Gender = Guard.Against.NullOrEmpty(gender, nameof(gender));
  }

  static bool IsValidDate(string birthday)
  {
    var isValid = false;

    if (!string.IsNullOrEmpty(birthday) && birthday.Length >= 6)
    {
      isValid = DateTime.TryParse(birthday, out _);
    }

    return isValid;
  }
}
