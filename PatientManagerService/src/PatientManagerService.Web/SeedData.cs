
using PatientManagerService.Core.PatientAggregate;

using PatientManagerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PatientManagerService.Web;

public static class SeedData
{
  public static readonly Patient Patient1 = new ("Cliff", "G", "8/8/1988", "M");

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
    {
      // Look for any Patients items.
      if (dbContext.Patients.Any())
      {
        return;   // DB has been seeded
      }

      PopulateTestData(dbContext);
    }
  }
  public static void PopulateTestData(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Patients)
    {
      dbContext.Remove(item);
    }
    
    dbContext.SaveChanges();

    dbContext.Patients.Add(Patient1);

    dbContext.SaveChanges();
  }
}
