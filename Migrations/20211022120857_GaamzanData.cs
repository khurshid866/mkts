using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MKTS.Migrations
{
    public partial class GaamzanData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "District");

            migrationBuilder.CreateTable(
                name: "GaamzanEnrollment",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<short>(type: "smallint", nullable: false),
                    AdmnMonth = table.Column<short>(type: "smallint", nullable: false),
                    AdmnDay = table.Column<short>(type: "smallint", nullable: false),
                    AdmnYear = table.Column<short>(type: "smallint", nullable: false),
                    Disability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tehsil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnionCouncil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GaamzanEnrollment", x => x.EnrollmentID);
                });

            migrationBuilder.CreateTable(
                name: "GaamzanTraining",
                columns: table => new
                {
                    TrainingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParticipantType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingTheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Partner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GaamzanTraining", x => x.TrainingID);
                });

          

            migrationBuilder.CreateTable(
                name: "SchoolSupported",
                columns: table => new
                {
                    SchoolSupportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Partner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Washroom = table.Column<short>(type: "smallint", nullable: false),
                    SolarPanel = table.Column<short>(type: "smallint", nullable: false),
                    WaterTank = table.Column<short>(type: "smallint", nullable: false),
                    PlasticMat = table.Column<short>(type: "smallint", nullable: false),
                    WaterCooler = table.Column<short>(type: "smallint", nullable: false),
                    BlackBoard = table.Column<short>(type: "smallint", nullable: false),
                    TeacherChair = table.Column<short>(type: "smallint", nullable: false),
                    Registar = table.Column<short>(type: "smallint", nullable: false),
                    Shelter = table.Column<short>(type: "smallint", nullable: false),
                    LearningMaterial = table.Column<short>(type: "smallint", nullable: false),
                    LCDs = table.Column<short>(type: "smallint", nullable: false),
                    Tablets = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSupported", x => x.SchoolSupportID);
                });

            migrationBuilder.CreateTable(
                name: "YouthFacilitation",
                columns: table => new
                {
                    YouthFacilitationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModeFacilitation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillEnhanced = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YouthFacilitation", x => x.YouthFacilitationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GaamzanEnrollment");

            migrationBuilder.DropTable(
                name: "GaamzanTraining");

           

            migrationBuilder.DropTable(
                name: "SchoolSupported");

            migrationBuilder.DropTable(
                name: "YouthFacilitation");

            migrationBuilder.AddColumn<string>(
                name: "ProjectID",
                table: "District",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
