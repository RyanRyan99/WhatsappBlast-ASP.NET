<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="ReportPlan.aspx.cs" Inherits="whatsappmobil.ReportPlan" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col-md-4">
                    <h1 class="app-page-title gradient-custom-text">Message Report</h1>
                </div>
                <div class="col"></div>
                <div class="col">
                    <div class="row">
                        <div class="input-group input-group-sm" id="datetimepicker1" data-target-input="nearest">
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control form-control-sm" placeholder="Search" data-target="#datetimepicker1"></asp:TextBox>
                            <div style="width: 30px;" class="input-group-append input-group-text form-control-sm gradient-custom-button-1 justify-content-center" data-target="#datetimepicker1" data-toggle="datetimepicker">
                                <i class="fa fa-calendar text-white"></i>
                            </div>
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
                                <div class="col">
                                    <div class="d-flex">
                                        <div class="d-flex flex-lg-row-start mb-1">
                                            <asp:LinkButton ToolTip="Export Excel" runat="server" ID="btnExport" CssClass="btn btn-sm gradient-custom-button-1 text-white" OnClick="btnExport_Click">Export &nbsp; <i class="fa-solid fa-file-excel"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="d-flex flex-row-reverse">
                                        <asp:DropDownList runat="server" ID="ddlViewAllBranch" Width="37" CssClass="form-control form-control-sm gradient-custom-button-1" ForeColor="White" BackColor="Black" AutoPostBack="true" OnSelectedIndexChanged="ddlViewAllBranch_SelectedIndexChanged">
                                            <asp:ListItem Value="false">false</asp:ListItem>
                                            <asp:ListItem Value="true">true</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="lblViewAllBranch" CssClass="mt-1" Font-Size="Smaller">View All Branch : &nbsp;</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div>
                                    <asp:GridView runat="server" ID="gvReportPlan" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No Report Available" OnRowCommand="gvReportPlan_RowCommand"
                                        AllowPaging="true" OnPageIndexChanging="gvReportPlan_PageIndexChanging" CssClass="table table-responsive w-100 d-block d-md-table"
                                        OnRowDataBound="gvReportPlan_RowDataBound" Font-Size="Small" PageSize="10" HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White">
                                        <Columns>
                                            <asp:BoundField DataField="sent_date" HeaderText="Tanggal" DataFormatString="{0: dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="tidak_terkirim" HeaderText="Belum Terkirim" />
                                            <asp:BoundField DataField="terkirim" HeaderText="Terkirim" />
                                            <asp:BoundField DataField="gagal" HeaderText="Gagal" />
                                            <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ToolTip="Lihat Data" runat="server" ID="btnView" CssClass="btn btn-sm gradient-custom-button-1 text-white" CommandName="View" CommandArgument='<%# Eval("sent_date") %>'><i class="fa-solid fa-eye"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ToolTip="Kirim ulang yang belum terkirim" runat="server" ID="btnResent" CssClass="btn btn-sm" CommandName="Resent" CommandArgument='<%# Eval("sent_date") %>' OnClientClick="if ( !confirm('Kirim ulang yang belum terkirim ? ')) return false;"><i class="fa-solid fa-share"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ToolTip="Export" runat="server" ID="btnExport" CssClass="btn btn-sm gradient-custom-button-1 text-white" CommandName="Export" CommandArgument='<%# Eval("sent_date") %>'><i class="fa-solid fa-file-excel"></i></asp:LinkButton>
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
    </div>

    <div class="modal fade" id="ModalViewReport" tabindex="-1" aria-labelledby="ModalViewReportLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalViewReportLabel">Message Report -
                        <asp:Label runat="server" ID="lblViewHeader" CssClass="badge bg-success"></asp:Label></h5>
                    <asp:LinkButton runat="server" ID="btnCloseTopHeader" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body" style="font-size: small">
                    <asp:UpdatePanel runat="server" ID="updPnl1">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col">
                                </div>
                                <div class="col">
                                    <asp:HiddenField ID="hiddenTrxId" runat="server" />
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ToolTip="Search Berdasarkan Nomor Whatsapp" runat="server" ID="txtSearchView" CssClass="form-control form-control-sm" placeholder="Masukan Whatsapp"></asp:TextBox>
                                        <asp:TextBox ToolTip="Search Berdasarkan Nama" runat="server" ID="txtSearchViewName" CssClass="form-control form-control-sm" placeholder="Masukan Nama"></asp:TextBox>
                                        <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-button-1">
                                            <asp:LinkButton ToolTip="Search" runat="server" ID="btnSearchView" Width="30" OnClick="btnSearchView_Click"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:GridView runat="server" ID="gvViewReport" AutoGenerateColumns="false" AllowPaging="true" Font-Size="Smaller"
                                        CssClass="table table-responsive w-100 d-block d-md-table" GridLines="None" OnRowDataBound="gvViewReport_RowDataBound" OnPageIndexChanging="gvViewReport_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="sender_id" HeaderText="Session" />
                                            <asp:BoundField DataField="sent_date" HeaderText="Tanggal Kirim" DataFormatString="{0: dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="type_message" HeaderText="Tipe" />
                                            <asp:BoundField DataField="message_title" HeaderText="Judul" />
                                            <asp:BoundField DataField="wa_number" HeaderText="Whatsapp" />
                                            <asp:BoundField DataField="message_content" HeaderText="Pesan" />
                                            <asp:BoundField DataField="status_session" HeaderText="Status" />
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-1 text-white" ID="btnCloseHeader">Close</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(function () {
            $('#datetimepicker1').datetimepicker({
                format: 'DD/MM/YYYY',
            });
            $('#datetimepicker2').datetimepicker({
                format: 'MM/DD/YYYY',
            });
        });
        $(document).ready(function () {
            $('#<%= btnCloseHeader.ClientID %>').click(function (e) {
                $('#ModalViewReport').modal('hide');
            });
            $('#<%= btnCloseTopHeader.ClientID %>').click(function (e) {
                $('#ModalViewReport').modal('hide');
            });
        });
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
                    color: #019267;
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
                    color: #aea79f;
                    background-color: #f5f5f5;
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
