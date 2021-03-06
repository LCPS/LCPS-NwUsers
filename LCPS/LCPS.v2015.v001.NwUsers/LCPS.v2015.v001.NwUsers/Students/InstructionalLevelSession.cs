﻿#region Using

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

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;

using Anvil.v2015.v001.Domain.Entities;

#endregion

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class InstructionalLevelSession : ImportFileTSV
    {


 public InstructionalLevelSession()
            : base(
                addIfNotExists: true,
                updateIfExists: false,
                itemType: typeof(InstructionalLevelCandidate),
                viewTitle: "Import Instructional Levels",
                area: "Students",
                controller: "InstructionalLevel",
                action: "Preview",
                viewLayoutPath: "~/Areas/Students/Views/Shared/_StudentLayout.cshtml")
        {
            SessionId = Guid.NewGuid();
        }


 public InstructionalLevelSession(ImportSession s)
            :base(s)
        { }

        public override void DetectConflicts()
        {
            try
            {
                List<InstructionalLevelCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<InstructionalLevelCandidate>().ToList();
                List<InstructionalLevelCandidate> dups = candidates.Duplicates(true).Cast<InstructionalLevelCandidate>().ToList();
                foreach (InstructionalLevelCandidate dup in dups)
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
        }    }
}
