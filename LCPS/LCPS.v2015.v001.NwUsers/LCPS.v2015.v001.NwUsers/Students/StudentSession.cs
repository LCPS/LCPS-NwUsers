using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Importing;

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
            throw new NotImplementedException();
        }
    }
}
