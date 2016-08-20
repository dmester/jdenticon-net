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
Open NuGet package manager and install the package Jdenticon-net.

### 2. Use the class Identicon
Create an instance of the class Identicon to generate identicons.

```csharp
using Jdenticon;
----
var icon = Identicon.FromValue("john.doe@example.faux");
icon.Save(100, "johndoe.svg");
```

## Reference
### Create an instance of Identicon.
There are mainly two ways of creating an instance of `Identicon`:

* `Identicon.FromHash(hash)`

  Creates an instance with a hash value. You can either provide a byte array containing the hash, or 
  provide a hexadecimally encoded hash as a string.
  
* `Identicon.FromValue(value, hashAlgorithmName)`

  Jdenticon-net will create a hash for you using the specified hash algorithm. If no hash algorithm is 
  specified SHA1 will be used. You can input any object, even null, as argument. Jdenticon will use 
  ToString to get a string representation of the object and then push the UTF8-encoded string through
  the specified hash algorithm.

### Generate an icon
There are multiple methods on Identicon for generating an icon:

* `Save(size, path|stream[, format])`

  Generates an icon with the specified size and saves it to the specified file or stream. If a path
  is specified, the format is determined from the filename extension if not explicitly specified.
  
* `Draw(graphics, rectangle)`

  Generates an icon and draws it in the specified GDI+ drawing context. Sutiable if you want to 
  display the icon on the screen without first saving it to a file.
  
* `ToBitmap(size)`

  Generates an icon as a Bitmap object for later usage. Remember that you are responsible for 
  disposing the returned object.
  
* `ToSvg(size, fragment)`

  Generates an icon as an SVG string. Note that if you want to create an SVG file the Save method 
  is prefered over the ToSvg method.

## License
Jdenticon-net is released under the [zlib license](https://github.com/dmester/jdenticon-net/blob/master/LICENSE.txt).
