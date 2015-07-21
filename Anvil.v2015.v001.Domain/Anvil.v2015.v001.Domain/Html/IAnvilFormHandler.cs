using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Html
{
    public interface IAnvilFormHandler
    {
        string FormArea { get; set; }
        string FormController { get; set; }
        string FormAction { get; set; }
        string SubmitText { get; set; }
        string OnErrorActionName { get; set; }
    }
}
