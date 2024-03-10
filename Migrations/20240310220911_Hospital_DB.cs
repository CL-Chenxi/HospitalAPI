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
                    Drug_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Drug_Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Drug_AllergyList = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugSet", x => x.Drug_ID);
                });

            migrationBuilder.CreateTable(
                name: "MedicationPlanSet",
                columns: table => new
                {
                    MPlanEntry_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedPlan_ID = table.Column<int>(type: "int", nullable: false),
                    MedPlan_Date = table.Column<DateTime>(type: "date", nullable: false),
                    MedPlan_Posology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drug_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationPlanSet", x => x.MPlanEntry_ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientSet",
                columns: table => new
                {
                    Patient_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patient_fName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Patient_lName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patient_DoB = table.Column<DateTime>(type: "date", maxLength: 50, nullable: false),
                    Patient_PhoneNum = table.Column<int>(type: "int", nullable: false),
                    Patient_Allergy = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Staff_PhoneNum = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    Staff_Grade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSet", x => x.Staff_ID);
                });

            migrationBuilder.CreateTable(
                name: "TestResultSet",
                columns: table => new
                {
                    TestRes_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestRes_Date = table.Column<DateTime>(type: "date", nullable: false),
                    TestRes_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestRes_Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestRes_Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Staff_ID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResultSet", x => x.TestRes_ID);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlanSet",
                columns: table => new
                {
                    TPlanEntry_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plan_ID = table.Column<int>(type: "int", nullable: false),
                    Patient_ID = table.Column<int>(type: "int", nullable: false),
                    Staff_ID = table.Column<int>(type: "int", nullable: false),
                    TPlan_CycleLen = table.Column<int>(type: "int", nullable: false),
                    TPlan_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plan_Date = table.Column<DateTime>(type: "date", nullable: false),
                    TPlan_Observation = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    TPlan_ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TPlan_ActionLink = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlanSet", x => x.TPlanEntry_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugSet");

            migrationBuilder.DropTable(
                name: "MedicationPlanSet");

            migrationBuilder.DropTable(
                name: "PatientSet");

            migrationBuilder.DropTable(
                name: "StaffSet");

            migrationBuilder.DropTable(
                name: "TestResultSet");

            migrationBuilder.DropTable(
                name: "TreatmentPlanSet");
        }
    }
}
