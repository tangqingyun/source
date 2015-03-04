using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Web.Models
{
    public class ModelError
    {
        public ModelError(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
        public string ErrorMessage { set; get; }
    }
}
