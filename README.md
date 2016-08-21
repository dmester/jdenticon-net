# [Jdenticon-net](https://jdenticon.com)
.NET library for generating highly recognizable identicons.

![Sample identicons](https://jdenticon.com/hosted/github-samples.png)

## Features
Jdenticon-net is a .NET port of the JavaScript library [Jdenticon](https://github.com/dmester/jdenticon).

* Render icons as PNG, EMF or SVG files.
* Render icons directly on screen using GDI+.
* Generate SVG fragments to be used inline on websites.

## Getting started
Using Jdenticon-net is simple. Follow the steps below to integrate Jdenticon-net into your solution.

### 1. Install Jdenticon-net
Open NuGet package manager and install the package `Jdenticon-net`.

### 2. Use the class `Identicon`
Create an instance of the class `Identicon` to generate identicons.

```csharp
using Jdenticon;
----
var icon = Identicon.FromValue("john.doe@example.faux");
icon.Save(100, "johndoe.svg");
```

## Reference
### Create an instance of Identicon
There are mainly two ways of creating an instance of `Identicon`:

* `Identicon.FromHash(hash)`

  Creates an instance with a hash value. You can either provide a byte array containing the hash, or 
  provide a hexadecimal hash string. At least 6 bytes are required in byte arrays and 12 characters 
  in hash strings.
  
* `Identicon.FromValue(value[, hashAlgorithmName])`

  Jdenticon-net will create a hash for you using the specified hash algorithm. You can provide any 
  object, even null, as argument. Jdenticon will use `ToString` to get a string representation of the 
  object and then push the UTF8-encoded string through the specified hash algorithm. If no hash 
  algorithm is specified Jdenticon-net will default to SHA1.

### Generate an icon
There are multiple methods in the `Identicon` class for generating icons:

* `Save(size, path|stream[, format])`

  Generates an icon with the specified size and saves it to the specified file path or stream. If a path
  is specified, the format is determined from the filename extension if not explicitly specified.
  
* `Draw(graphics, rectangle)`

  Generates an icon and draws it in the specified GDI+ drawing context. Suitable if you want to 
  display the icon on the screen without first saving it to a file.
  
* `ToBitmap(size)`

  Generates an icon as a `Bitmap` object for later usage. Remember that you are responsible for 
  disposing the returned object when you don't need it anymore.

* `ToSvg(size[, fragment])`

  Generates an SVG string containing an icon. This can be useful for embedding icons in other SVG files or
  inlining SVG icons on your website. For creating SVG files, please use `Save`.

## License
Jdenticon-net is released under the [zlib license](https://github.com/dmester/jdenticon-net/blob/master/LICENSE.txt).
