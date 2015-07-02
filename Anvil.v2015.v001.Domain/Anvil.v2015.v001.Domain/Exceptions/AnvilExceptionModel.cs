using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Exceptions
{
    public class AnvilExceptionModel : AnvilExceptionCollector
    {
        public AnvilExceptionModel(Exception ex, string title, string area, string controller, string action)
            :base(ex)
        {
            this.Area = area;
            this.Controller = controller;
            this.Action = action;
            this.Title = title;
        }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
    }
}
