<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="UltimateSniper_WebSite.MasterPages.AboutUs" Title="Snipe Agent" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
<asp:Panel ID="PanelTitle" runat="server" CssClass="Title"><%=Resources.lang.AboutUs_MembersTitle%></asp:Panel>
<div id="DivActive" class="DivActive">
    <div class="DivAboutUs">
    <%=Resources.lang.AboutUs_Contact%>
    <br /><br />
    <table>
    <tr>
        <td><asp:Label ID="lblUserEmailAddress" runat="server" Text="<%$ Resources:lang, AboutUs_UserEmailAddress%>"></asp:Label></td>
        <td><asp:TextBox ID="txtBoxUserEmailAddress" runat="server" Width="180px"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang, AboutUs_EmailTitle%>"></asp:Label></td>
        <td><asp:TextBox ID="txtBoxEmailTitle" runat="server" Width="180px"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang, AboutUs_EmailBody%>"></asp:Label></td>
        <td><asp:TextBox ID="txtBoxEmailBody" runat="server" Height="112px" Width="180px"></asp:TextBox></td>
    </tr>
    </table>
    
    <br />
    
    <asp:Button ID="btnSendEmail" runat="server" 
            Text="<%$ Resources:lang, AboutUs_btnSendEmail%>" 
            onclick="btnSendEmail_Click" />
    </div>
    <div class="DivAboutUs">
    <%=Resources.lang.AboutUs_Info%>

<!--
<form action="https://www.paypal.com/cgi-bin/webscr" method="post">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="encrypted" value="-----BEGIN PKCS7-----MIIHLwYJKoZIhvcNAQcEoIIHIDCCBxwCAQExggEwMIIBLAIBADCBlDCBjjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRYwFAYDVQQHEw1Nb3VudGFpbiBWaWV3MRQwEgYDVQQKEwtQYXlQYWwgSW5jLjETMBEGA1UECxQKbGl2ZV9jZXJ0czERMA8GA1UEAxQIbGl2ZV9hcGkxHDAaBgkqhkiG9w0BCQEWDXJlQHBheXBhbC5jb20CAQAwDQYJKoZIhvcNAQEBBQAEgYAX8f+JulpyP/01R57LVpg/Ffkn3i3+GWIMQ+T+zQEbOuQlZAmrSYci3z+F/vDLNEqlzcqn3gLipYybi4X6S59A6xnKCWmVieILmOX9p7iZFfxDqB9sYgZbUFT7nn++ZiEpo4S64OVan1FIZ9MhDmEaDvWEs2rBJEnX03at6B3g1DELMAkGBSsOAwIaBQAwgawGCSqGSIb3DQEHATAUBggqhkiG9w0DBwQIESdH2skxFQ6AgYiet18chkH49YpOl3llZpAVqB8ih6mSbWNl/4Ba69R3PJ0Ev4bMZ7dKpTFYMtBI1nJhsa14FgXD2jZzAu4c/L72D9ciScun8PqSe0GoQMnFg32YTS3AB29q90/Zx7cOQEO1tVpYwWaDNklxvpRmonpIB7Q7djlQLeagcgPk0ngRdfAWMuNnBpuRoIIDhzCCA4MwggLsoAMCAQICAQAwDQYJKoZIhvcNAQEFBQAwgY4xCzAJBgNVBAYTAlVTMQswCQYDVQQIEwJDQTEWMBQGA1UEBxMNTW91bnRhaW4gVmlldzEUMBIGA1UEChMLUGF5UGFsIEluYy4xEzARBgNVBAsUCmxpdmVfY2VydHMxETAPBgNVBAMUCGxpdmVfYXBpMRwwGgYJKoZIhvcNAQkBFg1yZUBwYXlwYWwuY29tMB4XDTA0MDIxMzEwMTMxNVoXDTM1MDIxMzEwMTMxNVowgY4xCzAJBgNVBAYTAlVTMQswCQYDVQQIEwJDQTEWMBQGA1UEBxMNTW91bnRhaW4gVmlldzEUMBIGA1UEChMLUGF5UGFsIEluYy4xEzARBgNVBAsUCmxpdmVfY2VydHMxETAPBgNVBAMUCGxpdmVfYXBpMRwwGgYJKoZIhvcNAQkBFg1yZUBwYXlwYWwuY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDBR07d/ETMS1ycjtkpkvjXZe9k+6CieLuLsPumsJ7QC1odNz3sJiCbs2wC0nLE0uLGaEtXynIgRqIddYCHx88pb5HTXv4SZeuv0Rqq4+axW9PLAAATU8w04qqjaSXgbGLP3NmohqM6bV9kZZwZLR/klDaQGo1u9uDb9lr4Yn+rBQIDAQABo4HuMIHrMB0GA1UdDgQWBBSWn3y7xm8XvVk/UtcKG+wQ1mSUazCBuwYDVR0jBIGzMIGwgBSWn3y7xm8XvVk/UtcKG+wQ1mSUa6GBlKSBkTCBjjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRYwFAYDVQQHEw1Nb3VudGFpbiBWaWV3MRQwEgYDVQQKEwtQYXlQYWwgSW5jLjETMBEGA1UECxQKbGl2ZV9jZXJ0czERMA8GA1UEAxQIbGl2ZV9hcGkxHDAaBgkqhkiG9w0BCQEWDXJlQHBheXBhbC5jb22CAQAwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQCBXzpWmoBa5e9fo6ujionW1hUhPkOBakTr3YCDjbYfvJEiv/2P+IobhOGJr85+XHhN0v4gUkEDI8r2/rNk1m0GA8HKddvTjyGw/XqXa+LSTlDYkqI8OwR8GEYj4efEtcRpRYBxV8KxAW93YDWzFGvruKnnLbDAF6VR5w/cCMn5hzGCAZowggGWAgEBMIGUMIGOMQswCQYDVQQGEwJVUzELMAkGA1UECBMCQ0ExFjAUBgNVBAcTDU1vdW50YWluIFZpZXcxFDASBgNVBAoTC1BheVBhbCBJbmMuMRMwEQYDVQQLFApsaXZlX2NlcnRzMREwDwYDVQQDFAhsaXZlX2FwaTEcMBoGCSqGSIb3DQEJARYNcmVAcGF5cGFsLmNvbQIBADAJBgUrDgMCGgUAoF0wGAYJKoZIhvcNAQkDMQsGCSqGSIb3DQEHATAcBgkqhkiG9w0BCQUxDxcNMTAwNDIxMTMxNDA4WjAjBgkqhkiG9w0BCQQxFgQU5kMYO+4ntYs+q3FNW8PTyiHYuFAwDQYJKoZIhvcNAQEBBQAEgYCoh4vNXFGz+tmfVgnSuCbiXGv2vBC3BmD+Vpg3fNL3o0xErKiMlVPNbcKiSKzaKrVfHJ9tY0KhRDRGsqUKqHzMGoqjejLvteZx7f1SON6xUkKTZW+kA2hfUG7PPdNwZj/T4fSAFTRJwn9Hq7uoY1PoP6fomeDZG/H+uZCozKN35g==-----END PKCS7-----
">
<input type="image" src="https://www.paypal.com/en_US/i/btn/btn_donateCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
<img alt="" border="0" src="https://www.paypal.com/en_US/i/scr/pixel.gif" width="1" height="1">
</form>
-->
    <asp:ImageButton ID="paypalDonate" runat="server" 
        ImageUrl="https://www.paypal.com/en_US/i/btn/btn_donateCC_LG.gif" 
        onclick="paypalDonate_Click" />
    </div>
</div>
</asp:Content>
