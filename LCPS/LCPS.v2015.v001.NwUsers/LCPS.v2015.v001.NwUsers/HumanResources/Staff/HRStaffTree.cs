using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Html;
using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffTree : AnvilTreeNode
    {
        #region Fields

        private LcpsDbContext _dbContext = new LcpsDbContext();

        #endregion

        #region Constructrors

        public HRStaffTree()
        {
            ApplicationBase app = LcpsDbContext.DefaultApp;
            Text = app.AppName;
            LinkClass = "division";
            InitGlyph = "fa fa-cubes";
            ItemGlyphCss = "fa-cube";
            SelectedItemGlyphCss = "fa-cubes";
        }

        #endregion

        #region Refresh

        public void Refresh()
        {
            HRBuilding[] bb = _dbContext.Buildings.OrderBy(x => x.Name).ToArray();
            foreach(HRBuilding b in bb)
            {
                AnvilTreeNode bn = new AnvilTreeNode()
                {
                    Text = b.Name, 
                    Value = b.BuildingKey.ToString(),
                    InitGlyph = "fa fa-building",
                    ItemGlyphCss = "fa-building",
                    SelectedItemGlyphCss = "fa-building-o"
                };


            }

        }

        #endregion


    }
}
