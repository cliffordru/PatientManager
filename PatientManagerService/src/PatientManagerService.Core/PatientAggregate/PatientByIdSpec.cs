using Ardalis.Specification;

namespace PatientManagerService.Core.PatientAggregate.Specifications;

public class PatientByIdSpec : Specification<Patient>, ISingleResultSpecification
{
  public PatientByIdSpec(int patientId)
  {
    Query
        .Where(p => p.Id == patientId);
  }
}
