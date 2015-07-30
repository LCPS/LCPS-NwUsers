using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anvil.v2015.v001.Domain.Exceptions;
namespace Anvil.v2015.v001.Domain.Html
{
    public class AnvilTreeNode
    {
        private List<AnvilTreeNode> _children = new List<AnvilTreeNode>();


        public AnvilTreeNode()
        { }

        public AnvilTreeNode(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }

        public AnvilTreeNode(string text, string value, string cssClass)
        {
            this.Text = text;
            this.Value = value;
            this.LinkClass = cssClass;
        }

        public AnvilTreeNode(string text, string value, object item)
        {
            this.Text = text;
            this.Value = value;
        }

        public AnvilTreeNode(string text, string value, string cssClass, object item)
        {
            this.Text = text;
            this.Value = value;
            this.LinkClass = cssClass;
        }


        public string Text { get; set;}
        
        public string Value { get; set;}

        public string LinkClass { get; set; }

        public string InitGlyph { get; set; }

        public string ItemGlyphCss { get; set; }
        
        public string SelectedItemGlyphCss { get; set; }

        public Exception ItemError { get; set; }

        public List<AnvilTreeNode> Children
        {
            get{
                return _children;
            }
        }

        public string ToUL()
        {
            if (this.Text == "")
                return "";

            string ul = "<ul>";

            CreateUl(this, ref ul);

            ul += "</ul>";

            return ul;
        }

        private void CreateUl(AnvilTreeNode n, ref string output)
        {
            if (n.ItemError == null)
            {
                bool IsParent = (n.Children.Count() > 0);


                if (IsParent)
                {
                    output += "<li><span style='margin-right: 8px;' selectedGlyph='" + n.SelectedItemGlyphCss + "' normalGlyph='" + n.ItemGlyphCss + "'><i class='" + n.InitGlyph + "'></i></span><a value='" + n.Value + "' class='" + n.LinkClass + "'>" + n.Text + "</a>\n";
                    output += "<ul>\n";
                    foreach (AnvilTreeNode child in n.Children)
                    {
                        CreateUl(child, ref output);
                    }
                    output += "</ul></li>\n";
                }
                else
                {
                    output += "<li><span style='margin-right: 8px;' selectedGlyph='" + n.SelectedItemGlyphCss + "' normalGlyph='" + n.ItemGlyphCss + "'><i class='" + n.InitGlyph + "'></i></span><a value='" + n.Value + "' class='" + n.LinkClass + "'>" + n.Text + "</a></li>\n";
                }

            }
            else
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ItemError);
                string result = "<li class='text-danger'>";
                result += ec.ToUL() + "</li>\n";
                output += result;
            }
        }

    }
}
