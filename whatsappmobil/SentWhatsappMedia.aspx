<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="SentWhatsappMedia.aspx.cs" Inherits="whatsappmobil.SentWhatsappMedia" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Whatsapp Media</h1>
                </div>
                <div class="col"></div>
                <div class="col">
                    <div class="row">
                        <div class="input-group input-group-sm">
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control form-control-sm" placeholder="Search"></asp:TextBox>
                            <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-button-1">
                                <asp:LinkButton runat="server" ID="btnSearch" Width="30" OnClick="btnSearch_Click"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
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
                                <div>
                                    <asp:GridView runat="server" ID="gvSentWhatsappMedia" AutoGenerateColumns="false" CssClass="table table-responsive w-100 d-block d-md-table"
                                        EmptyDataText="No Data Available" GridLines="None" OnRowCommand="gvSentWhatsappMedia_RowCommand" OnPageIndexChanging="gvSentWhatsappMedia_PageIndexChanging" OnRowDataBound="gvSentWhatsappMedia_RowDataBound" PageSize="5" Font-Size="Small" HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White">
                                        <Columns>
                                            <asp:BoundField DataField="trxid" HeaderText="" Visible="false" />
                                            <asp:BoundField DataField="wa_header" HeaderText="Judul" />
                                            <asp:BoundField DataField="scheduled_date" HeaderText="Scheduled" DataFormatString="{0: dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="scheduled_time" HeaderText="Waktu" />
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblstatus" Text='<%# Bind("status_session") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-sm gradient-custom-button-1 text-white" CommandName="View" CommandArgument='<%# Eval("trxid") + "|" + Eval("wa_header") + "|" + Eval("wa_media") %>'><i class="fa-solid fa-eye"></i></asp:LinkButton>
                                                            </td>
                                                            <td>&nbsp</td>
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="btnSentAll" CssClass="btn btn-sm" CommandName="SentAll" CommandArgument='<%# Eval("trxid") %>' OnClientClick="if ( !confirm('Apakah ingin mengirim semua ? ')) return false;"><i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                                            </td>
                                                            <td>&nbsp</td>
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="btnDelete" CssClass="btn btn-sm" CommandName="Delete" CommandArgument='<%# Eval("trxid") %>' OnClientClick="if ( !confirm('Apakah ingin menghapus ? ')) return false;"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="wa_media" Visible="false" />
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
    </div>

    <div class="modal fade" id="ModalView" tabindex="-1" aria-labelledby="ModalViewLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalViewLabel">View Target - 
                        <asp:Label runat="server" ID="lblHeaderName" CssClass="badge gradient-custom-button-1"></asp:Label></h5>
                    <asp:LinkButton runat="server" ID="btnViewCloseTop" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="updPnlModalView">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col">
                                        <div class="mb-3">
                                            <div class="row">
                                                <label for="lblWaMediaView" class="form-label" style="font-size: smaller; font-weight: bold">
                                                    Show File Media : &nbsp
                                                    <asp:CheckBox runat="server" ID="ChkShowImages" AutoPostBack="true" OnCheckedChanged="ChkShowImages_CheckedChanged"/></label>
                                            </div>
                                            <div class="mb-3">
                                                <div class="row">
                                                    <asp:HiddenField runat="server" ID="hiddenPlanId" />
                                                    <asp:HiddenField runat="server" ID="hiddenMessageTemplate" />
                                                    <asp:Image runat="server" ID="imgWhatsappMediaView" Visible="false" Width="80%" Height="80%" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col">
                                            <asp:GridView runat="server" ID="gvView" AutoGenerateColumns="false" GridLines="None" CssClass="table table-responsive w-100 d-block d-md-table"
                                                EmptyDataText="No Data Available" OnRowDataBound="gvView_RowDataBound" OnRowCommand="gvView_RowCommand" Font-Size="Smaller" HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White" AllowPaging="true" OnPageIndexChanging="gvView_PageIndexChanging" PageSize="10">
                                                <Columns>
                                                    <asp:BoundField DataField="sender_id" HeaderText="Session" />
                                                    <asp:BoundField DataField="sent_date" HeaderText="Tanggal" DataFormatString="{0: dd-MMM-yyyy}" />
                                                    <asp:BoundField DataField="type_message" HeaderText="Tipe" />
                                                    <asp:BoundField DataField="message_title" HeaderText="Judul" Visible="false" />
                                                    <asp:BoundField DataField="wa_number" HeaderText="Whatsapp" />
                                                    <asp:BoundField DataField="message_content" HeaderText="Pesan" />
                                                    <asp:BoundField DataField="status_session" HeaderText="Status" />
                                                    <asp:BoundField DataField="session_notic" HeaderText="Note" />
                                                    <asp:BoundField DataField="trxid" Visible="false" />
                                                    <asp:BoundField DataField="wa_media" Visible="false" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="btnSentRow" CommandName="SentRow" CommandArgument='<%# Eval("wa_number") + "|" + Eval("message_content") + "|" + Eval("sender_id") + "|" + Eval("trxid") + "|" + Eval("wa_media") %>' CssClass="btn btn-sm"><i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
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
            $('#<%= btnViewCloseTop.ClientID %>').click(function (e) {
                $('#ModalView').modal('hide');
            });
        });

        function showpreview(input) {

            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#imgpreview').css('visibility', 'visible');
                    $('#imgpreview').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }

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
