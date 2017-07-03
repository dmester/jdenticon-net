# [Jdenticon-net](https://jdenticon.com)
.NET library for generating highly recognizable identicons.

![Sample identicons](https://jdenticon.com/hosted/github-samples.png)

## Features
Jdenticon-net is a .NET port of the JavaScript library [Jdenticon](https://github.com/dmester/jdenticon).

* Runs on .NET Framework 2.x, 3.x, 4.x, .NET Standard 1.x and .NET Core 1.x.
* Render icons as PNG and SVG files with no dependencies to System.Drawing or WPF.
* Integration package available for ASP.NET WebForms, MVC and WebApi.
* Generate SVG fragments to be used inline on websites.
* Render icons directly on screen using GDI+.

## Getting started
Using Jdenticon-net is simple. Follow the steps below to integrate Jdenticon-net into your solution.

### 1. Install the [NuGet package](https://www.nuget.org/packages/Jdenticon-net/)
```
PM> Install-Package Jdenticon-net
```

### 2. Use `Identicon` to generate icons
```csharp
using Jdenticon;
----
var icon = Identicon.FromValue("john.doe@example.faux", size: 100);
icon.SaveAsPng("johndoe.png");
```

### ASP.NET
To get started using Jdenticon for ASP.NET, please see:

* [Code examples for ASP.NET WebForms](https://dmester.github.io/jdenticon-net-docs/html/N_Jdenticon_AspNet_WebForms.htm)
* [Code examples for ASP.NET MVC](https://dmester.github.io/jdenticon-net-docs/html/N_Jdenticon_AspNet_Mvc.htm)
* [Code examples for ASP.NET WebApi](https://dmester.github.io/jdenticon-net-docs/html/N_Jdenticon_AspNet_WebApi.htm)

## Quick Reference
For full documentation, please see https://dmester.github.io/jdenticon-net-docs/.

### Create an instance of Identicon
There are mainly two ways of creating an instance of `Identicon`:

* `Identicon.FromHash(hash, size)`

  Creates an instance with a hash value. You can either provide a byte array containing the hash, or 
  provide a hexadecimal hash string. At least 6 bytes are required in byte arrays and 12 characters 
  in hash strings.
  
* `Identicon.FromValue(value, size[, hashAlgorithmName])`

  Jdenticon-net will create a hash for you using the specified hash algorithm. You can provide any 
  object, even null, as argument. Jdenticon will use `ToString` to get a string representation of the 
  object and then push the UTF8-encoded string through the specified hash algorithm. If no hash 
  algorithm is specified Jdenticon-net will default to SHA1.

### Generate an icon
There are multiple methods in the `Identicon` class for generating icons:

* `SaveAsPng(path|stream)`

  Generates an icon and saves it as a PNG image.
  
* `SaveAsSvg(path|stream|TextWriter)`

  Generates an icon and saves it as an SVG image.
  
* `Draw(graphics, rectangle)`

  Generates an icon and draws it in the specified GDI+ drawing context. Suitable if you want to 
  display the icon on the screen without first saving it to a file.
  
* `ToBitmap()`

  Generates an icon as a [Bitmap](https://msdn.microsoft.com/en-us/library/system.drawing.bitmap(v=vs.110).aspx)
  object for later usage. Remember that you are responsible for disposing the returned object when you don't 
  need it anymore.

* `ToSvg([fragment])`

  Generates an SVG string containing an icon. This can be useful for embedding icons in other SVG files or
  inlining SVG icons on your website. For creating SVG files, please use `Save`.
  
### Change icon appearance
There are properties on `Identicon` that can be used to customize the look of the generated icons.

* `Padding` (default 0.08)

  The padding between the outer bounds of the icon and the content. Specified as percent in the range
  [0.0, 0.4].

* `Style.BackColor` (default white)

  Specifies the background color of the generated icon. Set to `Color.Transparent` to not render any 
  background behind the identicon shapes.
  
* `Style.Saturation` (default 0.5)
  
  Saturation of colored shapes in the range [0.0, 1.0].
  
* `Style.ColorLightness` (default [0.4, 0.8])

  Lightness range of colored shapes in the range [0.0, 1.0]. The lightness of the shapes can be inverted by
  specifying a range where `Range.From` is greater than `Range.To`.
  
* `Style.GrayscaleLightness` (default [0.3, 0.9])

  Lightness range of grayscale shapes in the range [0.0, 1.0]. The lightness of the shapes can be inverted by
  specifying a range where `Range.From` is greater than `Range.To`.
  
Example

```csharp
using Jdenticon;
using System.Drawing;
----
var iconStyle = new IdenticonStyle
{
    BackColor = Color.Transparent,
    Saturation = 0.4f,
    ColorLightness = Range.Create(0.4f, 0.9f),
    GrayscaleLightness = Range.Create(0.3f, 0.9f)
};

var icon = Identicon.FromValue("john.doe@example.faux");

icon.Padding = 0.10f;
icon.Style = iconStyle;

icon.Save(100, "johndoe.svg");
```
  
### Advanced customizations
By subclassing `Jdenticon.IconGenerator` you can completely override the look of your icons. Set the
`Identicon.IconGenerator` property to an instance of your own generator to make use of the customized 
icon generator.

## License
Jdenticon-net is released under the [zlib license](https://github.com/dmester/jdenticon-net/blob/master/LICENSE.txt).
