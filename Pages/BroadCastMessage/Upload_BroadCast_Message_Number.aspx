<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="Upload_BroadCast_Message_Number.aspx.cs" Inherits="Pages_BroadCastMessage_Upload_BroadCast_Message_Number" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar" id="ul" runat="server">
        <li>
            <!-- Button to trigger modal new business form -->
            <input type="button" id="btnClear" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
    </ul>
    <style>
        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 250%;
            background-color: #ababab;
            z-index: 9999999;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=90);
        }

        #modal {
            display: none;
            position: absolute;
            top: 40%;
            left: 45%;
            width: 64px;
            height: 64px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 9999999;
            text-align: center;
            overflow: auto;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('#<%=txtScript.ClientID%>').keyup(function () {
                var limited_char = 160;
                var text_lengh = 0;
                var remian_char = 0;
                text_lengh = $(this).val().length;
                remian_char = limited_char - text_lengh;

                if (text_lengh >= limited_char) {
                    $(this).val($(this).val().substring(0, limited_char));
                    //alert('you have reached the limit');
                    $('#char_count').text('[ ' + ($(this).val().length - limited_char) + ' characters left ]');
                }
                else {

                    // $('#char_count').text(remian_char + ' characters left');
                    $('#char_count').text('[ ' + remian_char + ' characters left ]');
                }
            });

            $('#btnClear').click(function () {
                window.location.replace('Upload_BroadCast_Message_Number.aspx');
            });

            //Span hide event
            $('#<%=div_preview_records.ClientID%>').hide();
            $('#spanShow').click(function () {
                if ($(this).text() == 'Hide') {
                    $('#<%=div_preview_records.ClientID%>').hide();
                    $(this).text('Show');
                }
                else {
                    $('#<%=div_preview_records.ClientID%>').show();
                    $(this).text('Hide');
                   
                }

            });

            $('#spanHide').click(function () {
                $('#<%=div_preview_records.ClientID%>').hide();
                $('#spanShow').text('Show');
             });

            $('#spanHide').mouseover(function () {
                $(this).css('cursor', 'pointer');
                $(this).css('font-style', 'italic');
            });
            $('#spanHide').mouseleave(function () {
                $(this).css('font-style', 'normal');
            });
            $('#spanShow').mouseover(function () {
                $(this).css('cursor', 'pointer');
                $(this).css('font-style', 'italic');
            });
            $('#spanShow').mouseleave(function () {
                $(this).css('font-style', 'normal');
            });
        });

        function loading() {
            $("#modal").show();
            $("#fade").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <br />
    <br />
    <br />

    <div id="fade"></div>
    <div id="modal">
        <img id="loader" src="../../App_Themes/functions/loading.gif" alt="" />
        <span>Sending...</span>
    </div>

    <div id="div_main" runat="server" class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Upload Number</h3>
        </div>
        <div class="panel-body">

            <%-- Upload Wing Account Design Section--%>
            <table style="width: 70%;">

                <tr>
                    <td style="text-align: left;">
                        <asp:Label runat="server" ID="lblUpload" Text="Upload"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="fupload" runat="server" Width="40%" CssClass="btn-small" />

                    </td>
                    <td>
                        <asp:Button ID="btnPreview" runat="server" Width="100px" Text="Preview" TabIndex="6" Height="30px" CssClass="btn-small" OnClick="btnPreview_Click" Style="border-radius: 10px;" />
                    </td>
                    <td style="padding-left: 20px;">
                        <a href="upload_broadcast_message_number_format.xlsx"><span>Download Template</span></a>
                    </td>
                </tr>
            </table>

            <div id="div_preview" runat="server">
                <div id="div_preview_count_records" runat="server"></div>
                <div id="div_preview_records" runat="server"></div>
            </div>
            <div style="margin-top: 5px;">
                <table style="width: 100%; ">
                    <tr>
                        <td style="text-align: left; vertical-align: top; width: 100px;">
                            <asp:Label ID="lblMessageCate" runat="server" Text="Message Cate."></asp:Label>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlMessageCate" runat="server" AppendDataBoundItems="true" DataSourceID="ds_message_cate" DataTextField="cate" DataValueField="id">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; vertical-align: top; width: 20px;">
                            <asp:Label runat="server" ID="lblScript" Text="Script"></asp:Label><span style="color: red; padding-left: 5px;">*</span>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtScript" runat="server" Style="width: 400px; height: 100px;" TextMode="MultiLine" MaxLength="160"></asp:TextBox>
                            <span style="padding: 5px; color: red; vertical-align: top;">* Max charactors 160</span>
                            <span id="char_count" style="padding: 5px; color: green; vertical-align: top;"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="float: left; padding: 5px; padding-left: 400px;">
                                <asp:Button runat="server" ID="btnSend" Text="Send" CssClass="btn-small btn-primary" Enabled="false" OnClick="btnSend_Click" Style="border-radius: 10px;" OnClientClick="loading();" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="div_upload" runat="server">
                <div id="div_upload_records" runat="server">
                </div>
                <div id="div_fail" runat="server"></div>
            </div>
        </div>

        <div class="panel-body">
        </div>
    </div>

    <div id="div_err" runat="server" style="text-align: center; vertical-align: middle; margin-top: 30px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;">
    </div>
    <asp:SqlDataSource ID="ds_message_cate" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select id, cate from messagecate order by cate;" runat="server"></asp:SqlDataSource>
</asp:Content>

