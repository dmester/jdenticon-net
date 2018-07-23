***************************************************************
* JDENTICON ASP.NET WEBAPI
***************************************************************

Please see the code example below how to use Jdenticon in your API.

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon_AspNet_WebApi.html

IDENTICON AS ACTION RESULT
==========================

It is possible to return an identicon from an API controller action. Just return an
instance of the IdenticonResult class.

	using Jdenticon.AspNet.WebApi;

	public class IdenticonController : ApiController
	{
		public IdenticonResult Get(string name, int size)
		{
			return IdenticonResult.FromValue(name, size);
		}
	}

