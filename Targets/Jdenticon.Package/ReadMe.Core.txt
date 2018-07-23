***************************************************************
* JDENTICON-NET
***************************************************************

Use the Identicon class to generate an identicon:

	using Jdenticon;
	// ...
	Identicon
		.FromValue("string to hash", size: 160)
		.SaveAsPng("test.png");

The save operations are implemented as extension methods so remember
to include a using for the Jdenticon namespace.

Also consider the following NuGet packages if they are relevant for
your application:

  * Jdenticon.AspNet.Mvc

  * Jdenticon.AspNet.WebApi

  * Jdenticon.AspNet.WebForms

  * Jdenticon.Gdi
    If you want to render icons in a WinForms application.

  * Jdenticon.Wpf

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon.html

