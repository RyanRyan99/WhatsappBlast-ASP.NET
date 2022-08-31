<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" MaintainScrollPositionOnPostback="true" CodeBehind="CreateSession.aspx.cs" Inherits="whatsappmobil.CreateSession" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Create Session Devices</h1>
                </div>
                <div class="col"></div>
                <div class="col">
                    <div class="row">
                        <div class="input-group input-group-sm">
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control form-control-sm" placeholder="Search"></asp:TextBox>
                            <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-card-1">
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
                                        <asp:Label runat="server" ID="lblAdd" ForeColor="GrayText">Add Session</asp:Label>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="d-flex flex-row-reverse">
                                        <asp:LinkButton runat="server" ID="btnAdd" CssClass="btn btn-sm gradient-custom-button-1 text-white" OnClick="btnAdd_Click">Add &nbsp; <i class="fa-solid fa-plus"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:GridView runat="server" ID="gvListDevices" AutoGenerateColumns="false" GridLines="None" CssClass="table table-responsive w-100 d-block d-md-table"
                                        OnRowCommand="gvListDevices_RowCommand" OnRowDataBound="gvListDevices_RowDataBound" PageSize="10" EmptyDataText="No Devices Available" Font-Size="Smaller">
                                        <Columns>
                                            <asp:BoundField DataField="session_id" HeaderText="Perangkat" />
                                            <asp:TemplateField HeaderText="Tipe Klien">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblIsLegecy" Text='<%# Bind("is_legecy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSessionStatus" Text='<%# Bind("session_status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="col d-flex flex-row-reverse">
                                                            <asp:LinkButton runat="server" ID="btnBarcode" CommandName="barcode" CommandArgument='<%# Eval("session_id") + "|" + Eval("is_legecy") %>' CssClass="btn btn-sm gradient-custom-button-1 text-white">Scan &nbsp; <i class="fa-solid fa-qrcode"></i></asp:LinkButton>
                                                        </div>
                                                        <div class="col">
                                                            <asp:LinkButton runat="server" ID="btnConnected" CommandName="connected" CommandArgument='<%# Eval("session_id") + "|" + Eval("is_legecy") %>' CssClass="btn btn-sm gradient-custom-button-1 text-white">Check &nbsp; <i class="fa-solid fa-link"></i></asp:LinkButton>
                                                        </div>
                                                        <div class="col d-flex justify-content-start">
                                                            <asp:LinkButton runat="server" ID="btnDelete" CommandName="deleted" CommandArgument='<%# Eval("session_id") + "|" + Eval("is_legecy") %>' CssClass="btn btn-sm gradient-custom-button-2 text-white" OnClientClick="if ( !confirm('Apakah ingin mengirim data pesan ini? ')) return false;">Delete &nbsp; <i class="fa-solid fa-trash-can"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalBarcode" tabindex="-1" aria-labelledby="ModalBarcodeLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalBarcodeLabel">Scan QrCode</h5>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="pnlBarcode">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <img runat="server" id="imgBarcode" src="" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-2 text-white" ID="btnClose" OnClick="btnClose_Click">Close</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalAdd" tabindex="-1" aria-labelledby="ModalAddLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalAddLabel">Add Session Devices</h5>
                    <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="mb-2">
                                    <asp:HiddenField runat="server" ID="hiddenSessionId" />
                                    <asp:Label runat="server" ID="lblDevices" CssClass="form-label d-flex mb-1" Font-Bold="true" Font-Size="Smaller">Nama Perangkat</asp:Label>
                                    <%--<asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSessionId"></asp:TextBox>--%>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSessionId">
                                        <asp:ListItem Value="">Pilih</asp:ListItem>
                                        <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                        <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                        <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                        <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                        <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                        <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                        <asp:ListItem Value="CIMONE6">CIMONE 6</asp:ListItem>
                                        <asp:ListItem Value="CIMONE7">CIMONE 7</asp:ListItem>
                                        <asp:ListItem Value="CIMONE8">CIMONE 8</asp:ListItem>
                                        <asp:ListItem Value="TESTING">TESTING DEV (X)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="mb-1">
                                    <asp:Label runat="server" ID="lblIsLegecy" CssClass="form-label d-flex mb-1" Font-Bold="true" Font-Size="Smaller">Tipe Klien</asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlIsLegecy" CssClass="form-control form-control-sm">
                                        <asp:ListItem Value="">Pilih</asp:ListItem>
                                        <asp:ListItem Value="false">Beta Multi-Device (RECOMENDED)</asp:ListItem>
                                        <asp:ListItem Value="true">Normal WhatsApp Web / Legacy</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-2 text-white" ID="btnViewClose">Close</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm gradient-custom-button-1 text-white" OnClick="btnSave_Click">Save &nbsp; <i class="fa-regular fa-floppy-disk"></i></asp:LinkButton>
                </div>
            </div>

        </div>
    </div>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function alerterror(params, icon) {
            Swal.fire({
                icon: icon,
                text: params,
                type: 'warning',
                confirmButtonColor: '#3cd4a4',
            })
        }
    </script>
    <style>
        .RadioButtonInterval label {
            margin-right: 15px !important;
            margin-left: 5px !important;
        }

        .form-control-sm {
            height: calc(1em + .375rem + 2px) !important;
            padding: .125rem .25rem !important;
            font-size: .75rem !important;
            line-height: 1.5;
            border-radius: .2rem;
        }

        .pagination-ys {
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

        .badge-sm {
            min-width: 1.8em;
            padding: .25em !important;
            margin-left: .1em;
            margin-right: .1em;
            color: white !important;
            cursor: pointer;
        }

        #ContentPlaceHolder1_ajaxfileupload_SelectFileButton {
            background-color: #4cd9a4;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }

        #ContentPlaceHolder1_ajaxfileupload_Html5DropZone {
            color: #4cd9a4;
            font-weight: bold;
            font-size: smaller;
        }

        #ContentPlaceHolder1_ajaxfileupload_FileStatusContainer {
            color: #4cd9a4;
            font-weight: bold;
            font-size: smaller;
        }

        #ContentPlaceHolder1_ajaxfileupload_ctl00 {
            border-color: #4cd9a4;
            border-width: 2px;
        }

        #ContentPlaceHolder1_ajaxfileupload_Html5DropZone {
            border-color: #4cd9a4;
        }

        #ContentPlaceHolder1_ajaxfileupload_QueueContainer {
            color: #4cd9a4 !important;
        }

        #ContentPlaceHolder1_ajaxfileupload_UploadOrCancelButton {
            background-color: #4cd9a4;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }

        #ContentPlaceHolder1_ajaxfileupload1_SelectFileButton {
            background-color: #4cd9a4;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }

        #ContentPlaceHolder1_ajaxfileupload1_Html5DropZone {
            color: #4cd9a4;
            font-weight: bold;
            font-size: smaller;
        }

        #ContentPlaceHolder1_ajaxfileupload1_FileStatusContainer {
            color: #4cd9a4;
            font-weight: bold;
            font-size: smaller;
        }

        #ContentPlaceHolder1_ajaxfileupload1_ctl00 {
            border-color: #4cd9a4;
            border-width: 2px;
        }

        #ContentPlaceHolder1_ajaxfileupload1_Html5DropZone {
            border-color: #4cd9a4;
        }

        #ContentPlaceHolder1_ajaxfileupload1_QueueContainer {
            color: #4cd9a4 !important;
        }

        #ContentPlaceHolder1_ajaxfileupload1_UploadOrCancelButton {
            background-color: #4cd9a4;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }

        .table-striped > tbody > tr:nth-child(2n+1) > td, .table-striped > tbody > tr:nth-child(2n+1) > th {
            background-color: #eafaf5;
        }
    </style>
</asp:Content>
