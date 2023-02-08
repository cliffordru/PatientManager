using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientManagerService.Infrastructure.Migrations;

  /// <inheritdoc />
  public partial class Initial : Migration
  {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "Patients",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  FirstName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                  LastName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                  Birthday = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                  Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Patients", x => x.Id);
              });
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "Patients");
      }
  }
