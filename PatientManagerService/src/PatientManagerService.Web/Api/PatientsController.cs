using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PatientManagerService.Core.PatientAggregate;
using PatientManagerService.Core.PatientAggregate.Specifications;
using PatientManagerService.SharedKernel.Interfaces;
using PatientManagerService.Web.ApiModels;

namespace PatientManagerService.Web.Api;

public class PatientsController : BaseApiController
{
  private readonly IRepository<Patient> _repository;
  private int _patientsAdded;

  public PatientsController(IRepository<Patient> repository)
  {
    _repository = repository;
  }

  // GET: api/patients
  [HttpGet]
  public async Task<IActionResult> List()
  {
    //TODO: Add server side paging, sorting, filtering
    var patientDTOs = (await _repository.ListAsync())
        .Select(patient => new PatientDTO
        (
            id: patient.Id,
            firstName: patient.FirstName,
            lastName: patient.LastName,
            birthday: patient.Birthday,
            gender: patient.Gender
        ))
        .ToList();

    return Ok(patientDTOs);
  }

  //TODO: Guid
  // GET: api/patients
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id)
  {
    var patientSpec = new PatientByIdSpec(id);
    var patient = await _repository.FirstOrDefaultAsync(patientSpec);
    if (patient == null)
    {
      return NotFound();
    }

    var result = new PatientDTO
    (
        id: patient.Id,
        firstName: patient.FirstName,
        lastName: patient.LastName,
        birthday: patient.Birthday,
        gender: patient.Gender
    );

    return Ok(result);
  }

  // api/patients/{id}
  [HttpPut, Route("{id}"), ActionName("Patient")]
  public async Task<IActionResult> UpdatePatient(int id, PatientDTO request)
  {
    //TODO: Guid id == null || id == Guid.Empty || id != patient.Id
    if (request.Id == 0 || id != request.Id)
    {
      return BadRequest();
    }

    var patientSpec = new PatientByIdSpec(id);
    var existingPatient = await _repository.FirstOrDefaultAsync(patientSpec);
    if (existingPatient == null)
    {
      return NotFound();
    }

    existingPatient.Update(request.FirstName, request.LastName, request.Birthday, request.Gender);

    await _repository.UpdateAsync(existingPatient);

    return Ok("204"); //TODO: This is temp; should define standard json response
  }

  // api/patients/{id}
  [HttpDelete, Route("{id}"), ActionName("Patient")]
  public async Task<IActionResult> DeletePatient(int id)
  {
    //TODO: Guid id == null || id == Guid.Empty || id != patient.Id
    if (id == 0)
    {
      return BadRequest();
    }

    var patientSpec = new PatientByIdSpec(id);
    var existingPatient = await _repository.FirstOrDefaultAsync(patientSpec);
    if (existingPatient == null)
    {
      return NotFound();
    }    

    await _repository.DeleteAsync(existingPatient);

    return Ok("204"); //TODO: This is temp; need to define proper responses and/or use HTTP response codes
  }


  //api/patients
  //[DisableRequestSizeLimit]
  [HttpPost, ActionName("patients")]
  public async Task<IActionResult> UploadPatients(IFormFile file, CancellationToken cancellationToken)
  {
    //CancellationToken cancellationToken = new();
    await UploadData(file, cancellationToken);
    return Ok(_patientsAdded); //TODO: formal json response for all api endpoints
  }

  private async Task UploadData(IFormFile file, CancellationToken cancellationToken)
  {
    var fileextension = Path.GetExtension(file.FileName);
    if (fileextension == ".csv")
    {
      //TODO: File name cleanse, content check for malicious content, etc. 
      var filename = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{fileextension}";
      var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Uploads", filename);
      using (FileStream fs = System.IO.File.Create(filepath))
      {
        file.CopyTo(fs);
      }

      //TODO: add header validation
      var csvHelper = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = true,
        HeaderValidated = null,
        MissingFieldFound = null,
      };

      using (var reader = new StreamReader(filepath))
      using (var csv = new CsvReader(reader, csvHelper))
      {

        var records = csv.GetRecords<PatientFileDTO>();
        IList<Patient> patientsToAdd = new List<Patient>();
        foreach (var record in records)
        {
          //TODO: add some validation, duplicate check, etc. and return proper message to caller -- move to core
          if (string.IsNullOrWhiteSpace(record.FirstName) || string.IsNullOrWhiteSpace(record.LastName) || string.IsNullOrWhiteSpace(record.Birthday) || string.IsNullOrWhiteSpace(record.Gender))
          {
            throw new ApplicationException("Invalid data in file");
            // TODO: Log details here individually if want to continue to try to process valid records
            //return Result<List<PatientFileDTO>>.Error(new[] { ex.Message });
          }

          var patient = new Patient(record.FirstName, record.LastName, record.Birthday, record.Gender);

          //TODO: Basic Dup check 
          var patientUniqueSpec = new PatientUniqueSpec(patient);
          if (!await _repository.AnyAsync(patientUniqueSpec))
          {
            patientsToAdd.Add(patient);
          }
        }
        _patientsAdded = patientsToAdd.Count();
        var createdItem = await _repository.AddRangeAsync(patientsToAdd, cancellationToken);
      }
    }
    else
    {
      throw new ApplicationException("Invalid file format");
    }
  }


}
