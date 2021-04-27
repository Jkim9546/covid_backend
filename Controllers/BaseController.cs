using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using covid19_api.Models;
using System.Configuration;
using System.IO;

namespace covid19_api.Controllers
{
    public class BaseController : ApiController
    {
        protected COVIDInfo db = new COVIDInfo();
        
    }
}