using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.CssClass = cssClass;
        }


        public string Text { get; set;}
        
        public string Value { get; set;}

        public string CssClass { get; set; }

        public List<AnvilTreeNode> Children
        {
            get{
                return _children;
            }
        }
    }
}
