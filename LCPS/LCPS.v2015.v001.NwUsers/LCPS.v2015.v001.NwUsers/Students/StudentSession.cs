using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;

using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class StudentSession : ImportFileTSV
    {

        public StudentSession()
            : base(
                addIfNotExists: true,
                updateIfExists: true,
                itemType: typeof(StudentCandidate),
                viewTitle: "Import Students",
                area: "Students",
                controller: "Student",
                action: "Preview",
                viewLayoutPath: "~/Areas/Students/Views/Shared/_StudentLayout.cshtml")
        {
            SessionId = Guid.NewGuid();
        }


        public StudentSession(ImportSession s)
            :base(s)
        { }
        public override void DetectConflicts()
        {
            List<StudentCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<StudentCandidate>().ToList();
            List<string> Ids = candidates.Select(x => x.StudentId).ToList();
            List<string> dups = Ids.Duplicates(true).Cast<string>().ToList();

            

            foreach (string dup in dups)
            {
                StudentCandidate[] cc = candidates.Where(x => x.StudentId == dup).ToArray();
                foreach(StudentCandidate c in cc)
                {

                    int index = c.LineIndex;
                    Candidates[index].Comment = "Duplicates were found for student Id";
                    Candidates[index].EntityStatus = ImportEntityStatus.Duplicate;
                    Candidates[index].Record();

                }

            }
        }
    }
}
