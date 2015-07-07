#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anvil.v2015.v001.Domain.Infrastructure;
using Anvil.v2015.v001.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;


#endregion

namespace LCPS.v2015.v001.NwUsers.Infrastructure
{
    public class LcpsDbContext : ApplicationDbContext
    {
        #region Events
        #endregion

        #region Constants
        #endregion

        #region Fields
        #endregion

        #region Constructors

        public LcpsDbContext()
            :base(Properties.Settings.Default.ConnectionString)
        {
        }

        public LcpsDbContext(string connectionString)
            :base(connectionString)
        {
            Properties.Settings.Default.ConnectionString = connectionString;
        }

        #endregion

        #region Properties

        public DbSet<HREmployeeType> EmployeeTypes { get; set; }
        public DbSet<HRJobTitle> JobTitles { get; set; }
        public DbSet<HRBuilding> Buildings { get; set; }

        /*
        public DbSet<HRStaff> StaffMembers { get; set; }
        public DbSet<HRStaffPosition> StaffPositions { get; set; }
        */


        public DbSet<Importing.ImportItem> ImportItems { get; set; }
        public DbSet<Importing.ImportSession> ImportSessions { get; set; }

        /*
        public DbSet<Security.LcpsStaffEmail> StaffEmails { get; set; }
        */


        #endregion


        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<HREmployeeTypeCandidate>();
            base.OnModelCreating(modelBuilder);
        }
        */



    }
}
