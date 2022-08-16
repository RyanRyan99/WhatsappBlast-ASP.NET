<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="Singlechat.aspx.cs" Inherits="whatsappmobil.Singlechat" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col-md-4">
                    <h1 class="app-page-title gradient-custom-text">Single Chat</h1>
                </div>
            </div>
            <div class="row g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">
                            <ul class="nav nav-tabs">
                                <li class="nav-item">
                                    <a href="#messagetext" class="nav-link active text-success" data-bs-toggle="tab">Message Text</a>
                                </li>
                                <li class="nav-item">
                                    <a href="#messagemedia" class="nav-link  text-success" data-bs-toggle="tab">Message Media</a>
                                </li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade show active" id="messagetext">
                                    <asp:UpdatePanel ID="updpnl" runat="server">
                                        <ContentTemplate>
                                            <div class="row mt-4">
                                                <div class="col">
                                                    <div class="mb-3">
                                                        <label class="form-label d-flex">Device</label>
                                                        <asp:DropDownList runat="server" ID="ddlSessionId" CssClass="form-control form-control-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSessionId_SelectedIndexChanged">
                                                            <asp:ListItem Value="">Pilih</asp:ListItem>
                                                            <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                                            <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                                            <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                                            <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                                            <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                                            <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="mb-3">
                                                        <label class="form-label d-flex">Ke</label>
                                                        <asp:TextBox runat="server" ID="txtTO" CssClass="form-control form-control-sm" placeholder="Nomor Tujuan"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col">
                                                    <div class="mb-3">
                                                        <label class="form-label d-flex">Pesan</label>
                                                        <asp:TextBox runat="server" ID="txtMESSAGE" CssClass="form-control" Font-Size="Smaller" TextMode="MultiLine" Height="100" placeholder="Isi Pesan"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 d-flex flex-row-reverse">
                                                        
                                                    </div>
                                                </div>
                                                <asp:LinkButton runat="server" ID="btnSendText" OnClick="btnSendText_Click" CssClass="btn btn-sm gradient-custom-button-1 text-white flex-row-reverse">Mengirim &nbsp; <i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="tab-pane fade" id="messagemedia">
                                    <asp:UpdatePanel runat="server" ID="updpnl2">
                                        <ContentTemplate>
                                            <div class="row mt-4">
                                                <div class="mb-3">
                                                    <label class="form-label d-flex">Device</label>
                                                    <asp:DropDownList runat="server" ID="ddlSessionIdMedia" CssClass="form-control form-control-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSessionIdMedia_SelectedIndexChanged">
                                                        <asp:ListItem Value="">Pilih</asp:ListItem>
                                                        <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                                        <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                                        <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                                        <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                                        <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                                        <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col">
                                                    <div class="mb-3">
                                                        <label class="form-label d-flex">Ke</label>
                                                        <asp:TextBox runat="server" ID="txtTOMedia" CssClass="form-control form-control-sm" placeholder="Nomor Tujuan"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3">
                                                        <label class="form-label d-flex">Media</label>
                                                        <asp:FileUpload runat="server" ID="fileUplad" CssClass="form-control form-control-sm" />
                                                    </div>
                                                </div>
                                                <div class="col">
                                                    <div class="mb-3">
                                                        <label class="form-label d-flex">Pesan</label>
                                                        <asp:TextBox runat="server" ID="txtMESSAGEMEDIA" CssClass="form-control" Font-Size="Smaller" TextMode="MultiLine" Height="100" placeholder="Isi Pesan"></asp:TextBox>
                                                    </div>
                                                    <div class="mb-3 d-flex flex-row-reverse">
                                                        
                                                    </div>
                                                </div>
                                                <asp:LinkButton runat="server" ID="btnSendMedia" OnClick="btnSendMedia_Click" CssClass="btn btn-sm gradient-custom-button-1 text-white flex-row-reverse">Mengirim &nbsp; <i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
        function alert(params, icon) {
            Swal.fire({
                icon: icon,
                text: params,
                type: 'warning',
                confirmButtonColor: '#3cd4a4',
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
