using Ardalis.Specification;

namespace PatientManagerService.Core.PatientAggregate;
public class PatientUniqueSpec : Specification<Patient>, ISingleResultSpecification
{
  public PatientUniqueSpec(Patient patient)
  {
    Query
        .Where(p => p.FirstName == patient.FirstName)
        .Where(p => p.LastName == patient.LastName)
        .Where(p => p.Birthday == patient.Birthday)
        .Where(p => p.Gender == patient.Gender);
  }
}
