***************************************************************
* JDENTICON ASP.NET MVC                                       
***************************************************************

Use any of the approaches listed below to add an identicon to your application.

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon_AspNet_Mvc.html

HTMLHELPER APPROACH
===================

Add the following line to the top of the .CSHTML page:

	@using Jdenticon.AspNet.Mvc;

Anywhere in your .CSHTML page, add the following code:

	@Html.Identicon("TestIcon", 60, alt: "Identicon")

This will produce the following HTML markup:

	<img src="/identicon.axd?5AMA8Xyneag78XyneQ--" width="60" height="60" alt="Identicon" />

URLHELPER APPROACH
==================

Add the following line to the top of the .CSHTML page:

	@using Jdenticon.AspNet.Mvc;

Anywhere in your .CSHTML page, add the following code:

	<img src="@Url.Identicon("TestIcon", 60)" alt="Identicon" width="60" height="60">

This will produce the following HTML markup:

	<img src="/identicon.axd?5AMA8Xyneag78XyneQ--" alt="Identicon" width="60" height="60">

IDENTICON AS ACTION RESULT
==========================

It is possible to return an identicon from a controller action. Just return an
instance of the IdenticonResult class.

	using Jdenticon.AspNet.Mvc;

	public class IconController : Controller
	{
		public ActionResult Icon(string value, int size)
		{
			return IdenticonResult.FromValue(value, size);
		}
	}

