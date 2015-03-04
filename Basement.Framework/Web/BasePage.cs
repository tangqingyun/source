using Basement.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Basement.Framework.Web
{
    public class BasePage : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            //  base.OnInit(e);
        }

        public string GetForm(string input)
        {
            string str = Request.Form[input];
            return string.IsNullOrWhiteSpace(str) ? "" : str;
        }

        public string GetQueryString(string input)
        {
            string str = Request.QueryString[input];
            return string.IsNullOrWhiteSpace(str) ? "" : str;
        }

        protected ModelStateDictionary ModelState { set; get; }

        protected T RequestAsModel<T>()
        {
            ModelState = new ModelStateDictionary();
            IList<ModelError> ErrorMessage = new List<ModelError>();
            T t = default(T);
            NameValueCollection collection = Request.Form;
            string[] arr = collection.AllKeys;
            if (arr.Length == 0)
            {
                return t;
            }
            t = Activator.CreateInstance<T>();
            for (int i = 0; i < arr.Length; i++)
            {
                var property = t.GetType().GetProperty(arr[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null)
                {
                    continue;
                }
                object value =null;
                try
                {
                    string cvalue=collection.Get(i);
                    value=!string.IsNullOrWhiteSpace(cvalue)?cvalue.Trim():string.Empty;
                    if (property.PropertyType==typeof(bool)) {
                        value = cvalue == "1" ? true : false;
                    }
                    property.SetValue(t, Convert.ChangeType(value, property.PropertyType), null);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Add(new ModelError(property.Name + ex.Message));
                    ModelState.Errors = ErrorMessage;
                }
            }
            return t;
        }

    }
}
