***************************************************************
* JDENTICON ASP.NET CORE
***************************************************************

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon_AspNetCore.html

PREPARATIONS
============

1.	Update Startup.cs

Enable Jdenticon in your application by calling UseJdenticon in 
your Startup class. Put it right above UseStaticFiles.

+++ using Jdenticon.AspNetCore;
    /* ... */

    public class Startup
    {
        /* ... */

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /* ... */
+++         app.UseJdenticon();
            app.UseStaticFiles();
            app.UseMvc();
            /* ... */
        }
    }

2.  Update _ViewImports.cshtml

Make IdenticonTagHelper and the extensions for @Html and @Url 
available to your views by adding the following code to 
_ViewImports.cshtml.

    @using Jdenticon.AspNetCore
    @addTagHelper "*, Jdenticon.AspNetCore"


USAGE
=====

Use any of the approaches listed below to add an identicon to your application.

TAG HELPER APPROACH
==========================

Anywhere in your .CSHTML page, add the following code:

	@{
		var userId = "jdenticon";
	}
	<img identicon-value="userId" width="60" height="60" alt="Identicon" />

This will produce the following HTML markup:

	<img src="/identicons/5AMA8Xyneag78XyneQ--" width="60" height="60" alt="Identicon" />


HTMLHELPER APPROACH
===================

Anywhere in your .CSHTML page, add the following code:

	@Html.Identicon("TestIcon", 60, alt: "Identicon")

This will produce the following HTML markup:

	<img src="/identicons/5AMA8Xyneag78XyneQ--" width="60" height="60" alt="Identicon" />


URLHELPER APPROACH
==================

Anywhere in your .CSHTML page, add the following code:

	<img src="@Url.Identicon("TestIcon", 60)" alt="Identicon" width="60" height="60" />

This will produce the following HTML markup:

	<img src="/identicons/5AMA8Xyneag78XyneQ--" alt="Identicon" width="60" height="60" />


IDENTICON AS ACTION RESULT
==========================

It is possible to return an identicon from a controller action. Just return an
instance of the IdenticonResult class.

	using Jdenticon.AspNetCore;

	public class IconController : Controller
	{
		public IActionResult Icon(string value, int size)
		{
			return IdenticonResult.FromValue(value, size);
		}
	}

