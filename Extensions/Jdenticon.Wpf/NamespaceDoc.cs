#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jdenticon.Wpf
{
    /// <summary>
    /// Classes for using Jdenticon in WPF.
    /// </summary>
    /// <example>
    /// <h4>Install Jdenticon.Wpf</h4>
    /// <para>
    ///     To use Jdenticon in a WPF application, start with installing the Jdenticon.Wpf NuGet package.
    /// </para>
    /// <code language="bat" title="NuGet Package Manager">
    /// PM&gt; Install-Package Jdenticon.Wpf
    /// </code>
    /// 
    /// <h4>Use IdenticonElement</h4>
    /// <para>
    ///     Now you can add the <see cref="IdenticonElement"/> where in your application. Don't forget to
    ///     add the Jdenticon XML namespace.
    /// </para>
    /// <code language="xml" title="Example usage in WPF">
    /// &lt;Window x:Class="SampleApp.MainWindow"
    ///         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    ///         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    ///         xmlns:jd="clr-namespace:Jdenticon.Wpf;assembly=Jdenticon.Wpf"
    ///         Title="MainWindow" Height="250" Width="600"&gt;
    ///     &lt;Window.Resources&gt;
    ///         &lt;Style TargetType="{x:Type jd:IdenticonElement}"&gt;
    ///             &lt;Setter Property="Width" Value="100" /&gt;
    ///             &lt;Setter Property="Height" Value="100" /&gt;
    ///             &lt;Setter Property="Margin" Value="10" /&gt;
    ///         &lt;/Style&gt;
    ///     &lt;/Window.Resources&gt;
    ///     &lt;StackPanel Orientation="Horizontal" HorizontalAlignment="Center"&gt;
    ///         &lt;jd:IdenticonElement Value="icon1" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon2" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon3" /&gt;
    ///         &lt;jd:IdenticonElement Value="icon4" /&gt;
    ///     &lt;/StackPanel&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// <para>
    ///     This is the resulting application:
    /// </para>
    /// <img src="../images/WpfIcons.png" alt="Sample WPF application with four identicons" />
    /// 
    /// <h4>Binding</h4>
    /// <para>
    ///     To make the identicon useful you probably want to make a binding to the <see cref="IdenticonElement.Value"/> property. 
    ///     The value decides what the icon will look like.
    /// </para>
    /// <code language="xml" title="Example usage in WPF">
    /// &lt;jd:IdenticonElement Value="{Binding Path=UserID}" /&gt;
    /// </code>
    /// </example>
    [CompilerGenerated]
    internal static class NamespaceDoc
    {
    }
}
