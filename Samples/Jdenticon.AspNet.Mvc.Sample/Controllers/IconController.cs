using Jdenticon.AspNet.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jdenticon.WebApi.Sample.Controllers
{
    public class IconController : ApiController
    {
        public IdenticonResult Get(string name, int size)
        {
            return IdenticonResult.FromValue(name, size);
        }
    }
}
