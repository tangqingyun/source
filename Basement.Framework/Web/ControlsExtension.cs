using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Basement.Framework.Web
{
    /// <summary>
    /// 控件帮助
    /// </summary>
    public sealed class ControlsExtension
    {

        public static void BindDropDown(HtmlSelect ddl, object obj, string textField, string valueField, ListItem item=null)
        {
            ddl.DataSource = obj;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            if (item != null) {
                ddl.Items.Insert(0, item);
            }
        }

    }
}
