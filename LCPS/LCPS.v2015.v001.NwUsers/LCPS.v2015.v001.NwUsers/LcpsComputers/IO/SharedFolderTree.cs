using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Html;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using Anvil.v2015.v001.Domain.Exceptions;
namespace LCPS.v2015.v001.NwUsers.LcpsComputers.IO
{
    public class SharedFolderTree : AnvilTreeNode
    {
        public SharedFolderTree()
        { }

        public SharedFolderTree(string server)
        {
            Text = server;
            Value = server;
            LinkClass = "server";
            InitGlyph = "fa fa-desktop";
            SelectedItemGlyphCss = "fa-desktop";
            ItemGlyphCss = "fa-desktop";

            this.Server = server;
        }

        public string Server { get; set; }

        public bool Recursive { get; set; }

        public void Refresh()
        {
            try
            {
                ApplicationBase app = LcpsDbContext.DefaultApp;

                RemoteComputer c = new RemoteComputer(this.Server, app.LDAPDomainUserName, app.LDAPPassword);

                foreach(Win32_Share s in c.SharedFolders)
                {
                    AnvilTreeNode n = new AnvilTreeNode() 
                    {
                        Text = s.Name,
                        Value = "\\\\" + this.Server + "\\" + s.Name,
                        SelectedItemGlyphCss = "fa-folder-open-o",
                        ItemGlyphCss = "fa-folder-o",
                        InitGlyph = "fa fa-folder-o",
                        LinkClass = "folder"
                    };

                    if (Recursive)
                    {
                        string sp = "\\\\" + this.Server + "\\" + s.Name;
                        try
                        {


                            DirectoryInfo d = new DirectoryInfo(sp);

                            GetFolders(d, ref n);
                        }
                        catch (Exception ex)
                        {
                            n.ItemError = ex;
                        }
                    }

                    Children.Add(n);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error enumerating shares on server", ex);
            }
        }

        private void GetFolders(DirectoryInfo d, ref AnvilTreeNode n)
        {
            AnvilTreeNode dn = new AnvilTreeNode() 
            {
                Text = d.Name,
                Value = d.FullName,
                SelectedItemGlyphCss = "fa-folder-open-o",
                ItemGlyphCss = "fa-folder-o",
                InitGlyph = "fa-folder-o"
            };

            DirectoryInfo[] dd = null;

            try
            {
                dd = d.GetDirectories();
                foreach (DirectoryInfo child in dd)
                {
                    GetFolders(child, ref dn);
                }
            }
            catch (Exception ex)
            {
                n.ItemError = ex;
            }


            n.Children.Add(dn);

        }

        public static SharedFolderTree Get(string server)
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
