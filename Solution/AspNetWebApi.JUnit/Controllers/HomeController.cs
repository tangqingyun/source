using Basement.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace AspNetWebApi.JUnit.Controllers
{

    public class HomeController : BaseWebApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public string Index()
        {
            return "测试";
        }

    }
}
