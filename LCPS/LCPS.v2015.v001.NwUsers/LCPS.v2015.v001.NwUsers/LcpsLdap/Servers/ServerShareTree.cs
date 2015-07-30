using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using Anvil.v2015.v001.Domain.Html;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.Servers
{
    public class ServerShareTreeRoot : AnvilTreeNode
    {
        #region Constant

        const string ServerSearchExpression = "(&(objectCategory=computer)(operatingSystem=*Server*))";


        #endregion

        #region Refresh

        public void Refresh()
        {
            try
            {
                LcpsAdsDomain dom = LcpsAdsDomain.Default;
                List<LcpsAdsComputer> cc = LcpsAdsObject.FindAll(ServerSearchExpression, dom.DirectoryEntry, true, typeof(LcpsAdsComputer)).Cast<LcpsAdsComputer>().ToList();

                this.Text = dom.Name;
                this.Value = dom.ObjectGuid.ToString();
                this.LinkClass = "domain";
                this.InitGlyph = "fa fa-eye";
                this.ItemGlyphCss = "fa-eye";
                this.SelectedItemGlyphCss = "fa-globe";

                foreach(LcpsAdsComputer c in cc)
                {
                    Children.Add(new AnvilTreeNode()
                        {
                            Text = c.ComputerName,
                            Value = c.ObjectGuid.ToString(),
                            LinkClass = "server",
                            InitGlyph = "fa fa-desktop"
                        });
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Could not enumerate servers or their shares", ex);
            }
        }

        #endregion
    }
}
