# [Jdenticon-net](https://jdenticon.com)
.NET library for generating highly recognizable identicons.

![Sample identicons](https://jdenticon.com/hosted/github-samples.png)

[![Build Status](https://img.shields.io/github/actions/workflow/status/dmester/jdenticon-net/build.yml?branch=master)](https://github.com/dmester/jdenticon-net/actions)
[![Downloads](https://img.shields.io/nuget/dt/Jdenticon-net.svg)](https://www.nuget.org/packages/Jdenticon-net/)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/dmester/jdenticon-net/blob/master/LICENSE.txt)

## Features
Jdenticon-net is a .NET port of the JavaScript library [Jdenticon](https://github.com/dmester/jdenticon).

* Runs on multiple .NET platforms.
  * .NET Framework 2.0 and later
  * .NET Standard 1.0 and later
  * .NET Core 1.0 and later
* Render icons as PNG and SVG files with no dependencies to System.Drawing or WPF.
* Integration package available for ASP.NET Core, ASP.NET WebForms, MVC and WebApi.
* Generate SVG fragments to be used inline on websites.
* Controls available for WPF and WinForms.

## Getting started

### ASP.NET
* [Using Jdenticon in ASP.NET Core](https://jdenticon.com/get-started/aspnet-core.html)
* [Using Jdenticon in ASP.NET MVC](https://jdenticon.com/get-started/aspnet-mvc.html)
* [Using Jdenticon in ASP.NET WebApi](https://jdenticon.com/get-started/aspnet-webapi.html)
* [Using Jdenticon in ASP.NET WebForms](https://jdenticon.com/get-started/aspnet-webforms.html)

### Desktop
* [Showing identicons in WPF applications](https://jdenticon.com/get-started/wpf.html)
* [Showing identicons in WinForms applications](https://jdenticon.com/get-started/winforms.html)

### General usage
* [Using Jdenticon in .NET](https://jdenticon.com/get-started/generic-net.html)

## Quick Reference
For full documentation, please see https://jdenticon.com/net-api/.

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
  
* `ToSvg([fragment])`

  Generates an SVG string containing an icon. This can be useful for embedding icons in other SVG files or
  inlining SVG icons on your website. For creating SVG files, please use `Save`.
  
### Change icon appearance
You can customize the colors of the generated icons, by using [IdenticonStyle](https://jdenticon.com/net-api/T_Jdenticon_IdenticonStyle.html). The easiest way of creating a new style is to use the [icon designer](https://jdenticon.com/icon-designer.html).
  
### Advanced customizations
By subclassing `Jdenticon.IconGenerator` you can completely override the look of your icons. Set the
`Identicon.IconGenerator` property to an instance of your own generator to make use of the customized 
icon generator.

## License
Jdenticon-net is released under the [MIT license](https://github.com/dmester/jdenticon-net/blob/master/LICENSE.txt).
