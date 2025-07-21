<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="unknow_url.aspx.cs" Inherits="unknow_url" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <div id="dv_notFound" runat="server" style="vertical-align:middle; text-align:center; width:100%; background-color:none; margin-top:50px;" >
       <table border="0" style="width:100%;">
           <tr>
               <td style="text-align:right; width:50%;">
                    <img src="App_Themes/images/prohibit.jpg" style="width:100px; height:100px;" />
               </td>
               <td style="text-align:left;width:50%;">
                    <h3 style="font-weight:bolder; font-size:x-large; color:red;">400 Bad Request</h3>
               </td>
           </tr>
       </table>
         <hr />
                     <p style="color:red;">Your browser sent an invalid request.</p>
        </div>
        <div style=" display: inline;">
            
                   
        </div>
            
               
                
</asp:Content>