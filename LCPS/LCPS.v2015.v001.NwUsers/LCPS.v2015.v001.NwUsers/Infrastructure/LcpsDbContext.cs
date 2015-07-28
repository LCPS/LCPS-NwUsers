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

using LCPS.v2015.v001.NwUsers.Filters;
using System.Web;

#endregion

namespace LCPS.v2015.v001.NwUsers.Infrastructure
{
    public class LcpsDbContext : ApplicationDbContext
    {
        #region Events
        #endregion

        #region Constants

        public static Guid ADSStudentQueriesAntecedentId = new Guid("A59C22AB-A44B-48E2-8A39-B864D373BCDC");

        #endregion

        #region Fields
        #endregion

        #region Constructors

        public LcpsDbContext()
            : base(Properties.Settings.Default.ConnectionString)
        {
        }

        public LcpsDbContext(string connectionString)
            : base(connectionString)
        {
            Properties.Settings.Default.ConnectionString = connectionString;
        }

        #endregion

        #region Properties

        // ----------------------------- Staff

        public DbSet<HREmployeeType> EmployeeTypes { get; set; }
        public DbSet<HRJobTitle> JobTitles { get; set; }
        public DbSet<HRBuilding> Buildings { get; set; }
        public DbSet<HRRoom> Rooms { get; set; }

        public DbSet<HRStaff> StaffMembers { get; set; }
        public DbSet<HRStaffPosition> StaffPositions { get; set; }


        // ----------------------------- Students

        public DbSet<Students.Student> Students { get; set; }
        public DbSet<Students.InstructionalLevel> InstructionalLevels { get; set; }


        // ----------------------------- LDAP
        public DbSet<LcpsLdap.LdapTemplates.OUTemplate> OUTemplates { get; set; }

        // ----------------------------- Importing

        public DbSet<Importing.ImportItem> ImportItems { get; set; }
        public DbSet<Importing.ImportSession> ImportSessions { get; set; }


        // ----------------------------- Filtering
        public DbSet<Filters.MemberFilter> MemberFilters { get; set; }
        public DbSet<Filters.StaffFilterClause> StaffFilterClauses { get; set; }
        public DbSet<Filters.StudentFilterClause> StudentFilterClauses { get; set; }


        // ----------------------------- Email
        public DbSet<LcpsEmail.EmailAccount> EmailAccounts { get; set; }


        // ----------------------------- Computers
        public DbSet<LcpsComputers.ComputerInfo> Computers { get; set; }
        public DbSet<LcpsComputers.Peripherals.ArchiveNic> ArchivedNics { get; set; }


        #endregion

        #region Methods

        public static ApplicationBase DefaultApp
        {
            get
            {
                try
                {
                    LcpsDbContext db = new LcpsDbContext();
                    ApplicationBase a = db.Applications.FirstOrDefault();
                    if (a == null)
                        throw new Exception("There was no application base record found in the database");

                    return a;
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not get default application from the database", ex);
                }
            }
        }

        #endregion

        #region Seed

        public void SeedSql()
        {
            try 
            {
                Assembly _assembly = typeof(LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext).Assembly;
                string[] names = _assembly.GetManifestResourceNames();
                string[] views = names.Where(x => x.StartsWith("LCPS.v2015.v001.NwUsers.Seed.Sql.Views")).ToArray();
                foreach(string v in views)
                {
                    StreamReader _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream(v));
                    string sql = _textStreamReader.ReadToEnd();
                    this.Database.ExecuteSqlCommand(sql, new object[0]);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Could not create SQL objects during seed", ex);
            }
        }

        #endregion




    }
}
