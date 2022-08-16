<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="SentWhatsapp.aspx.cs" Inherits="whatsappmobil.SentWhatsapp" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Whatsapp Message</h1>
                </div>
                <div class="col">
                </div>
                <div class="col">
                    <div class="row">
                        <div class="input-group input-group-sm">
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control form-control-sm" placeholder="Search"></asp:TextBox>
                            <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-button-1 text-white">
                                <asp:LinkButton runat="server" ID="btnSearch" Width="30" OnClick="btnSearch_Click"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
                                <asp:HiddenField runat="server" ID="hiddenPlanId" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">
                            <div class="row">
                                <div class="col">
                                    <div class="d-flex">
                                        <asp:Label runat="server" ID="lblViewAllBranch" CssClass="mt-1" Font-Size="Smaller">View All Branch : &nbsp;</asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlViewAllBranch" Width="37" CssClass="form-control form-control-sm gradient-custom-card-1" ForeColor="White" BackColor="Black" Font-Size="Smaller" AutoPostBack="true" OnSelectedIndexChanged="ddlViewAllBranch_SelectedIndexChanged">
                                            <asp:ListItem Value="false">false</asp:ListItem>
                                            <asp:ListItem Value="true">true</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="d-flex mb-2 flex-row-reverse">
                                        <asp:LinkButton ToolTip="Kirim semua daftar pesan OPEN tanpa terkecuali" runat="server" ID="btnSentAllScheduled" OnClick="btnSentAllScheduled_Click" OnClientClick="if(!alert('Perhatian', 'Apakah ingin mengirim semua data dengan status OPEN ?', 'question', 'Ya Kirim Semua')) return false" CssClass="btn btn-sm gradient-custom-button-1 text-white">Send All &nbsp; <i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnSentAllSecound" OnClick="btnSentAllSecound_Click"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <asp:GridView runat="server" ID="gvSentWhatsapp" AutoGenerateColumns="false" GridLines="None" CssClass="table table-responsive w-100 d-block d-md-table" EmptyDataText="No Data Available"
                                    OnRowCommand="gvSentWhatsapp_RowCommand" OnRowDataBound="gvSentWhatsapp_RowDataBound" AllowPaging="true" OnPageIndexChanging="gvSentWhatsapp_PageIndexChanging" Font-Size="Smaller" PageSize="5" HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tipe">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTypeMessage" Text='<%# Eval("type_message") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="trxid" HeaderText="Id" Visible="false" />
                                        <asp:BoundField DataField="wa_header" HeaderText="Judul" />
                                        <asp:BoundField DataField="wa_date" HeaderText="Tanggal" DataFormatString="{0: dd-MMM-yyyy}" />
                                        <%--<asp:BoundField DataField="scheduled_time" HeaderText="Waktu" />--%>
                                        <asp:BoundField DataField="wa_media" Visible="false" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblStatus" Text='<%# Bind("status_session") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton runat="server" ToolTip="View Message" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-sm gradient-custom-button-1 text-white" CommandName="View" CommandArgument='<%# Eval("trxid") + "|" + Eval("wa_header") + "|" + Eval("is_media") + "|" + Eval("wa_media") %>'><i class="fa-solid fa-eye"></i></asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" ToolTip="Sent All Message" ID="btnSentAll" CssClass="btn btn-sm" CommandName="SentAll" CommandArgument='<%# Eval("trxid") + "|" + Eval("wa_media") %>' OnClientClick="if ( !confirm('Apakah ingin mengirim data pesan ini? ')) return false;"><i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" ToolTip="Delete All Message" ID="btnDelete" CssClass="btn btn-sm" CommandName="Delete" CommandArgument='<%# Eval("trxid") %>' OnClientClick="if ( !confirm('Apakah ingin menghapus ? ')) return false;"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalView" tabindex="-1" aria-labelledby="ModalViewLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalViewLabel">View Target - 
                        <asp:Label runat="server" ID="lblTitleContentView" CssClass="badge gradient-custom-button-1"></asp:Label></h5>
                    <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="col" runat="server" id="colsrc">
                                    <div class="mb-3">
                                        <div class="row">
                                            <label for="lblWaMediaView" class="form-label" style="font-size: smaller; font-weight: bold">
                                                Show File Media : &nbsp
                                                    <asp:CheckBox runat="server" ID="ChkShowImages" AutoPostBack="true" OnCheckedChanged="ChkShowImages_CheckedChanged" /></label>
                                        </div>
                                        <div class="mb-3">
                                            <div class="row">
                                                <asp:Image runat="server" ID="imgWhatsappMediaView" Visible="false" Width="30%" Height="30%" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <asp:HiddenField runat="server" ID="HiddenMediaName"/>
                                    <div class="col">
                                        <asp:GridView runat="server" ID="gvView" AutoGenerateColumns="false" GridLines="None" CssClass="table table-responsive w-100 d-block d-md-table"
                                            EmptyDataText="No Data Available" OnRowDataBound="gvView_RowDataBound" OnRowCommand="gvView_RowCommand" Font-Size="Small" HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White" AllowPaging="true" OnPageIndexChanging="gvView_PageIndexChanging" PageSize="10">
                                            <Columns>
                                                <asp:BoundField DataField="sender_id" HeaderText="Sessions" />
                                                <asp:BoundField DataField="sent_date" HeaderText="Tanggal" DataFormatString="{0: dd-MMM-yyyy}" HeaderStyle-Width="100" />
                                                <asp:BoundField DataField="type_message" HeaderText="Tipe" />
                                                <asp:BoundField DataField="message_title" HeaderText="Judul" Visible="false" />
                                                <asp:BoundField DataField="wa_number" HeaderText="Whatsapp" />
                                                <asp:BoundField DataField="message_content" HeaderText="Pesan" />
                                                <asp:BoundField DataField="status_session" HeaderText="Status" />
                                                <asp:BoundField DataField="session_notic" HeaderText="Note" />
                                                <asp:BoundField DataField="trxid" Visible="false" />
                                                <asp:BoundField DataField="push_name" Visible="false" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ClientIDMode="Static" ID="btnSentRow" CommandName="SentRow" CommandArgument='<%# Eval("wa_number") + "|" + Eval("message_content") + "|" + Eval("sender_id") + "|" + Eval("trxid") + "|" + Eval("push_name") %>' CssClass="btn btn-sm"><i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-2 text-white" ID="btnViewClose">Close</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnViewCloseFooter" CssClass="btn btn-sm gradient-custom-button-1 text-white" Visible="false">Save</asp:LinkButton>
                </div>
            </div>

        </div>
    </div>

    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        $(function () {
            $('#datetimepicker1').datetimepicker({
                format: 'MM/DD/YYYY',
            });
            $('#datetimepicker2').datetimepicker({
                format: 'MM/DD/YYYY',
            });
        });

        $(document).ready(function () {
            $('#<%= btnViewClose.ClientID %>').click(function (e) {
                $('#ModalView').modal('hide');
            });
            $('#<%= btnViewCloseFooter.ClientID %>').click(function (e) {
                $('#ModalView').modal('hide');
            });
        });

        function alerterror(params, icon) {
            Swal.fire({
                icon: icon,
                text: params,
                type: 'warning',
                confirmButtonColor: '#3cd4a4',
            })
        }

        function alert(header, errormessage, icon, confirmbutton) {
            Swal.fire({
                title: header,
                text: errormessage,
                icon: icon,
                showCancelButton: true,
                confirmButtonColor: '#3cd4a4',
                cancelButtonColor: '#FF5D5D',
                confirmButtonText: confirmbutton
            }).then((result) => {
                if (result.isConfirmed) {
                    var btn = document.getElementById('<%=btnSentAllSecound.ClientID%>');
                    btn.click();
                }
            })
        }
    </script>
    <style>
        .form-control-sm {
            height: calc(1em + .375rem + 2px) !important;
            padding: .125rem .25rem !important;
            font-size: .75rem !important;
            line-height: 1.5;
            border-radius: .2rem;
        }

        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #19c19e;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #d7fbee;
                    background-color: #19c19e;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }
    </style>
</asp:Content>
