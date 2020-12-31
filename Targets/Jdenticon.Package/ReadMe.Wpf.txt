**************************************************************************
* JDENTICON WPF
**************************************************************************

The IdenticonElement control is used to render identicons in WPF
applications.

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon_Wpf.html


USAGE EXAMPLE
=============
Example of a WPF window containing an identicon and a textbox for
controlling the rendered icon.

    <Window x:Class="MySample.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:jd="clr-namespace:Jdenticon.Wpf;assembly=Jdenticon.Wpf"
            Title="Jdenticon WPF Sample" Height="530" Width="450">
        <Grid Margin="25">
            <Grid.RowDefinitions>
                <RowDefinition Height="auto"/>
                <RowDefinition Height="auto"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>

            <Label Grid.Row="0" Padding="5,0,5,5">Value to hash:</Label>

            <TextBox x:Name="tbValue"
                     Grid.Row="1"
                     Padding="5"
                     Text="Jdenticon" />

            <jd:IdenticonElement Grid.Row="2"
                                 Margin="40"
                                 Value="{Binding ElementName=tbValue, Path=Text}" />
        </Grid>
    </Window>
