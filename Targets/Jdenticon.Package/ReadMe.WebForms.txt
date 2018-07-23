***************************************************************
* JDENTICON ASP.NET WEBFORMS                                       
***************************************************************

Please see the code example below how to add an identicon to your application.

Full documentation is available at
https://jdenticon.com/net-api/N_Jdenticon_AspNet_WebForms.html

DATABINDING EXAMPLE
===================

Add the following code to an .ASPX file:

	<asp:Repeater ID="repeater" runat="server">
		<ItemTemplate>
			Value: <asp:Literal runat="server" Text='<%# Container.DataItem %>' />: <br/>
			<jdenticon:Icon runat="server" Value='<%# Container.DataItem %>' Size="60" /> <br/><br/>
		</ItemTemplate>
	</asp:Repeater>

Add the following code to the code-behind file:

	protected void Page_Load(object sender, EventArgs e)
	{
		repeater.DataSource = new object[]
		{
			123,
			124,
			"Item3",
			"Item4"
		};
		repeater.DataBind();
	}