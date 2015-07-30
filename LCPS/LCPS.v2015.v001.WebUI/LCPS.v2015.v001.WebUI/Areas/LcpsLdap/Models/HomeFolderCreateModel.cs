using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.NwUsers.LcpsComputers.IO;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class HomeFolderCreateModel
    {
        public HomeFolderTemplate HomeFolderTemplate { get; set; }

        public SharedFolderTree GetFolderTree(string server)
        {
            if (server == null)
                return new SharedFolderTree();
            else
            {
                SharedFolderTree t = new SharedFolderTree(server);
                t.Refresh();
                return t;

            }
        }
    }
}