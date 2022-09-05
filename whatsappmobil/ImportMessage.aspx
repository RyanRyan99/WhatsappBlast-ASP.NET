<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="ImportMessage.aspx.cs" Inherits="whatsappmobil.ImportMessage" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Import Message</h1>
                </div>
            </div>
            <div class="row g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">

                            <div class="row">
                                <div class="col">
                                    <div class="mt-2">
                                        <asp:Label runat="server" ID="Label4" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Session Devices</asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlSessionId" CssClass="form-control form-control-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSessionId_SelectedIndexChanged">
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
                                    <div class="mt-2">
                                        <asp:Label runat="server" ID="lbltitle" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Judul</asp:Label>
                                        <asp:TextBox runat="server" ID="txtTitle" CssClass="form-control form-control-sm" placeholder="Judul"></asp:TextBox>
                                    </div>
                                    <div class="mt-2">
                                        <asp:Label runat="server" ID="lbltanggalScheduled" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Tanggal</asp:Label>
                                        <div class="input-group date" id="datetimepicker1" data-target-input="nearest">
                                            <asp:TextBox runat="server" ID="txtScheduled" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker1" placeholder="Tanggal Scheduled"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datetimepicker1" data-toggle="datetimepicker">
                                                <div class="input-group-text h-100 form-control-sm"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="mt-2">
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label runat="server" ID="Label2" CssClass="form-label d-flex text-success" Font-Size="Smaller" Font-Bold="true">Import EXCEL .xls</asp:Label>
                                            </div>
                                            <div class="col">
                                                <div class="d-flex flex-row-reverse">
                                                    <asp:LinkButton runat="server" ID="btnDownload" OnClick="btnDownload_Click" OnClientClick="if(!comf('Yakin', 'Download Template', 'question', 'Download')) return false" CssClass="badge gradient-custom-button-1">Download Template</asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="btnDownloadSec" OnClick="btnDownloadSec_Click"></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:FileUpload runat="server" ID="uploadfilemessage" CssClass="form-control form-control-sm" />
                                    </div>
                                    <div class="mt-2">
                                        <asp:Label runat="server" ID="Label5" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Upload File / Media &nbsp; <asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="Masukan File/Media jika anda ingin mengirim file">(*)</asp:Label></asp:Label>
                                        <asp:FileUpload runat="server" ID="uploadMedia" CssClass="form-control form-control-sm" />
                                    </div>

                                    <div class="mt-2" style="display: none;">
                                        <asp:Label runat="server" ID="lblWaktuScheduled" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Waktu Scheduled</asp:Label>
                                        <div class="input-group date" id="datetimepicker2" data-target-input="nearest">
                                            <asp:TextBox runat="server" ID="txtScheduledTime" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker2" placeholder="Waktu Scheduled"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datetimepicker2" data-toggle="datetimepicker">
                                                <div class="input-group-text h-100 form-control-sm"><i class="fas fa-clock"></i></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <div class="mt-2">
                                        <asp:Label runat="server" ID="Label1" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Pesan</asp:Label>
                                        <asp:TextBox runat="server" ID="txtMessage" CssClass="form-control" TextMode="MultiLine" Font-Size="Smaller" Height="150" placeholder="Masukan Pesan &#10;note :Upload File jika anda ingin mengirim file &#10;note : params huruf kecil semua exp : {full_name}, {tgl_stnk} dll"></asp:TextBox>
                                    </div>
                                    <div class="mb-2" style="overflow: auto; white-space: nowrap;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateName" Text="name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateName_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateFullName" Text="full_name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateFullName_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateTglBeli" Text="tgl_beli" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateTglBeli_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateTypeKendaraan" Text="type_kendaraan" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateTypeKendaraan_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateTglSTNK" Text="tgl_stnk" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateTglSTNK_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplatePlatNo" Text="plat_no" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplatePlatNo_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateLastService" Text="last_service" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateLastService_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateBirthDate" Text="birth_date" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBirthDate_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateBranchName" Text="branch_name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBranchName_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateBranchAddress" Text="branch_address" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBranchAddress_Click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="btnTemplateBranchPhone" Text="branch_phone" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBranchPhone_Click"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="mt-2">
                                <div class="d-grid">
                                    <asp:LinkButton runat="server" ID="btnUpload" CssClass="btn btn-sm gradient-custom-card-1 text-white" OnClick="btnUpload_Click">Start Upload &nbsp; <i class="fa-solid fa-cloud-arrow-up"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        $(function () {
            $('#datetimepicker1').datetimepicker({
                format: 'DD/MM/YYYY',
            });
            $('#datetimepicker2').datetimepicker({
                format: 'HH:mm',
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });
            $('#datetimepicker3').datetimepicker({
                format: 'DD/MM/YYYY',
            });
            $('#datetimepicker4').datetimepicker({
                format: 'HH:mm',
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });
        });

        function alert(errormessage, icon, errordesc) {
            Swal.fire({
                icon: icon,
                title: errormessage,
                text: errordesc,
                type: 'warning',
                confirmButtonColor: '#3cd4a4',
            })
        }

        function comf(header, errormessage, icon, confirmbutton) {
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
                    var btn = document.getElementById('<%=btnDownloadSec.ClientID%>');
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
    </style>
</asp:Content>
