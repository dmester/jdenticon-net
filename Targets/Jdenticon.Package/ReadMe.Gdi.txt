**************************************************************************
* JDENTICON GDI
**************************************************************************

This package is intended for rendering identicons using GDI+.

If you need a simple way of adding an identicon to your Windows Forms
based application, then install the Jdenticon.WinForms package, containing
a WinForms control.

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon.html


CAUTION!
========

Using this package in server-side applications is generally a bad idea!
 
More information:
https://msdn.microsoft.com/en-us/library/system.drawing(v=vs.110).aspx#Anchor_5
https://photosauce.net/blog/post/5-reasons-you-should-stop-using-systemdrawing-from-aspnet
 
For server-side applications, please use the package intended for your
application framework, or if no matching package is available, use the 
Jdenticon-net core library to render icons as PNG or SVG data streams.
