using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jdenticon.WebApi.Sample.Controllers
{
    public class IconApiController : ApiController
    {
        // The fully qualified name is used to clarify the difference between the MVC IdenticonResult 
        // and the WebApi IdenticonResult.

        [HttpGet]
        public Jdenticon.AspNet.WebApi.IdenticonResult Icon(string name, int size)
        {
            return Jdenticon.AspNet.WebApi.IdenticonResult.FromValue(name, size);
        }
    }
}
