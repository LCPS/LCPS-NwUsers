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


#endregion

namespace LCPS.v2015.v001.NwUsers.Importing
{
    public interface IImportEntity
    {
        bool TargetExists();

        void Create();

        void Update();

        bool IsSyncJustified();

        void Record();

    }
}
