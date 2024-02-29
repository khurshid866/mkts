using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MKTS.Models;
using MKTS.Models.Data;

namespace MKTS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MKTS.Models.Data.District> District { get; set; }
        public DbSet<MKTS.Models.Data.Province> Province { get; set; }
        public DbSet<MKTS.Models.Data.Year> Year { get; set; }
        public DbSet<MKTS.Models.Data.Season> Season { get; set; }
        public DbSet<MKTS.Models.Data.TypeOfEducation> TypeOfEducation { get; set; }
        public DbSet<MKTS.Models.Data.Partner> Partner { get; set; }
        public DbSet<MKTS.Models.Data.Class> Class { get; set; }
        public DbSet<MKTS.Models.Data.TrainingTheme> TrainingTheme { get; set; }
        public DbSet<MKTS.Models.Data.TargetGroup> TargetGroup { get; set; }
        public DbSet<MKTS.Models.Data.TrainingConductedBy> TrainingConductedBy { get; set; }
        public DbSet<MKTS.Models.Data.TypeOfParticipant> TypeOfParticipant { get; set; }

        public DbSet<MKTS.Models.Data.Enrollment> Enrollment { get; set; }
        public DbSet<MKTS.Models.Data.Retention> Retention { get; set; }
        public DbSet<MKTS.Models.Data.SchoolSupply> schoolSupply { get; set; }
        public DbSet<MKTS.Models.Data.Training> Training { get; set; }

        public DbSet<MKTS.Models.CheckDuplicate> checkDuplicates { get; set; }
        public DbSet<MKTS.Models.View.ProvinceSupply> ProvinceSupply { get; set; }
        public DbSet<MKTS.Models.View.GenderSP> GenderSP { get; set; }
        public DbSet<MKTS.Models.View.LevelSP> LevelSP { get; set; }
        public DbSet<MKTS.Models.Data.Project> Project { get; set; }

        //Gaamzan

        public DbSet<MKTS.Models.Gaamzan.GaamzanEnrollment> GaamzanEnrollment { get; set; }
        public DbSet<MKTS.Models.Gaamzan.GaamzanTraining> GaamzanTraining { get; set; }
        public DbSet<MKTS.Models.Gaamzan.SchoolSupported> SchoolSupported { get; set; }
        public DbSet<MKTS.Models.Gaamzan.YouthFacilitation> YouthFacilitation { get; set; }


        /// <summary>
        /// Old Data
        /// </summary>
        //public DbSet<MKTS.Models.Department> Department { get; set; }
        //public DbSet<MKTS.Models.Forward> Forward { get; set; }
        //public DbSet<MKTS.Models.ForwardChat> ForwardChat { get; set; }
        //public DbSet<MKTS.Models.TaskHead> TaskHead { get; set; }
        //public DbSet<MKTS.Models.TaskSource> TaskSource { get; set; }
        //public DbSet<MKTS.Models.TaskHeadStatus> taskHeadStatus { get; set; }
        //public DbSet<MKTS.Models.TaskType> TaskType { get; set; }
        //public DbSet<MKTS.Models.Performance> Performance { get; set; }
    }
}
