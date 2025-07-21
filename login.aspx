<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clife System | Login</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link rel="stylesheet" href="App_Themes/login.css" />
    <script src="scripts/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="scripts/bootstrap/js/bootstrap.js"></script>
</head>
<script type="text/javascript">

    function btnResetPassword() {
        $("#btnReset").click();
    }
</script>
<body>
    <form id="form1" runat="server">
        <div>
            <section class="container" id="slogin" runat="server">
                <div style="text-align:center; margin-bottom:10px;">
                     <asp:Image runat="server" ID="imgLogo" ImageUrl="~/App_Themes/images/mylogo.png" style="width:150px; height:100px;"  />
                    </div>
                <div class="login">
                    
                    <h1>LOGIN</h1>
                    <p>
                        <asp:TextBox ID="txtusername" runat="server" placeholder="Username" Font-Names="Arial"></asp:TextBox>
                    </p>

                    <p>
                        <asp:TextBox ID="txtpassword" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                    </p>

                    <p class="remember_me" style="display:none;">
                        <label>
                            <asp:CheckBox ID="chkRememberMe" runat="server" />
                            Remember me on this computer
                        </label>
                    </p>

                    <p class="submit">
                        <asp:Button ID="btnSubmit" name="commit" runat="server" Text="Login" OnClick="btnSubmit_Click" />
                    </p>
                </div>

                <div class="login-help">
                    <p>Forgot your password? <a class="reset" onclick="btnResetPassword()" style="cursor:pointer;">Click here to reset it</a>.</p>
                    <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Style="height: 27px; display:none" Text="Reset" onClick="btnReset_Click"/>

                    <p>
                        <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </p>
                    <br />
                </div>
            </section>
            <section id="sPwdAlter" runat="server" class="container">
                <div id="dv_alert" runat="server" class="login">
                     <h1>Notification</h1>
                     <p>
                       
                        <asp:Label ID="lblPasswordAlert" runat="server" Font-Names="Arial"></asp:Label>
                    </p>
                     <p class="submit">
                       <asp:Button ID="btnChange1"  runat="server" Text="Change" OnClick="btnChange1_Click" />
                        <asp:Button ID="btnContinue"  runat="server" Text="Continue" OnClick="btnContinue_Click" />
                    </p>
                   
                </div>
            </section>
              <section id="sChangePwd" runat="server" class="container">
                <div id="dv_changepwd" runat="server" class="login">
                     <h1>Change Password</h1>
                     <p>
                        <asp:TextBox ID="txtUserNameView" runat="server" placeholder="Username" Font-Names="Arial" Enabled="false" ReadOnly="true"></asp:TextBox>
                    </p>

                    <p>
                        <asp:TextBox ID="txtOldPassword" runat="server" placeholder="Old Password" TextMode="Password"></asp:TextBox>
                    </p>
                     <p>
                        <asp:TextBox ID="txtNewPassword" runat="server" placeholder="New Password" TextMode="Password"></asp:TextBox>
                    </p>
                     <p>
                        <asp:TextBox ID="txtConfirmNewPassword" runat="server" placeholder="Confirm New Password" TextMode="Password"></asp:TextBox>
                    </p>
                     <p class="submit">
                           <asp:Button ID="btnBack"  runat="server" Text="Back" OnClick="btnBack_Click" />
                        <asp:Button ID="btnChange"  runat="server" Text="Change" OnClick="btnChange_Click" />
                    </p>
                </div>
                   <div class="login-help">
                  
                    <p>

                        <asp:Label ID="lblChangePwdMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </p>
                    <br />
                </div>
            </section>
        </div>


    </form>
   

</body>
</html>
