using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAPI.Migrations
{
    /// <inheritdoc />
    public partial class Hospital_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugSet",
                columns: table => new
                {
                    Drug_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Drug_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Drug_Dosage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Drug_AllergyList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Drug_Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugSet", x => x.Drug_ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientSet",
                columns: table => new
                {
                    Patient_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patient_fName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Patient_lName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Patient_DoB = table.Column<DateTime>(type: "date", nullable: false),
                    Patient_PhoneNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Patient_Allergy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSet", x => x.Patient_ID);
                });

            migrationBuilder.CreateTable(
                name: "StaffSet",
                columns: table => new
                {
                    Staff_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Staff_fName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Staff_lName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Staff_PhoneNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Staff_Grade = table.Column<int>(type: "int", nullable: false),
                    Staff_Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSet", x => x.Staff_ID);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlanEntrySet",
                columns: table => new
                {
                    Entry_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plan_ID = table.Column<int>(type: "int", nullable: false),
                    Staff_ID = table.Column<int>(type: "int", nullable: false),
                    Last_Update = table.Column<DateTime>(type: "date", nullable: false),
                    Entry_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Drug_ID = table.Column<int>(type: "int", nullable: true),
                    Posology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlanEntrySet", x => x.Entry_ID);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlanSet",
                columns: table => new
                {
                    Plan_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patient_ID = table.Column<int>(type: "int", nullable: false),
                    Staff_ID = table.Column<int>(type: "int", nullable: false),
                    Plan_CycleLen = table.Column<int>(type: "int", nullable: false),
                    Plan_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plan_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Plan_Observation = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlanSet", x => x.Plan_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugSet");

            migrationBuilder.DropTable(
                name: "PatientSet");

            migrationBuilder.DropTable(
                name: "StaffSet");

            migrationBuilder.DropTable(
                name: "TreatmentPlanEntrySet");

            migrationBuilder.DropTable(
                name: "TreatmentPlanSet");
        }
    }
}
