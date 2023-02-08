using CsvHelper.Configuration.Attributes;

namespace PatientManagerService.Web.ApiModels;

// ApiModel DTOs are used by ApiController classes and are typically kept in a side-by-side folder
public class PatientFileDTO 
{
    //TODO: change to Guid
    public int Id { get; set; }

    [Name("First Name")]
    public string? FirstName { get; set; }

    [Name("Last Name")]
    public string? LastName { get; set; }

    public string? Birthday { get; set; }

    public string? Gender { get; set; }
}

