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

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

using Anvil.v2015.v001.Domain.Entities;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.HRImport
{
    public class HRJobTitleSession : ImportFileTSV
    {

        LcpsDbContext db = new LcpsDbContext();

        public HRJobTitleSession(ImportSession s)
            : base(s)
        { 

        }

        public override void DetectConflicts()
        {
            try 
            {
                List<HRJobTitleCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<HRJobTitleCandidate>().ToList();
                List<HRJobTitleCandidate> dups = candidates.Duplicates(true).Cast<HRJobTitleCandidate>().ToList();
                foreach (HRJobTitleCandidate dup in dups)
                {
                    Candidates[dup.LineIndex].Comment = "Duplicates were found for job title Id";
                    Candidates[dup.LineIndex].EntityStatus = ImportEntityStatus.Duplicate;
                    Candidates[dup.LineIndex].Record();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Could not detect conflicts", ex);
            }
        }
    }
}
