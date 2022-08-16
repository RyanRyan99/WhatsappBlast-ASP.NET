<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="Chat.aspx.cs" Inherits="whatsappmobil.Chat" EnableEventValidation="false" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="https://fonts.googleapis.com/css2?family=Montserrat&display=swap" rel="stylesheet"/>
    <style>
        body{
            font-family: 'Montserrat', sans-serif;
        }
    </style>
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">
                            <div class="row">
                                <div class="mb-2">
                                    <label class="form-label d-flex">Device</label>
                                    <asp:DropDownList runat="server" ID="ddlDevices" CssClass="form-control form-control-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlDevices_SelectedIndexChanged">
                                        <asp:ListItem Value="Pilih Devices"></asp:ListItem>
                                        <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                        <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                        <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                        <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                        <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                        <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hiddenNumberContact" />
                                </div>
                                <div class="col-3">
                                    <div class="row">
                                        <div class="col">
                                            <div class="input-group input-group-sm mt-2">
                                                <asp:TextBox runat="server" ID="txtSearchContact" CssClass="form-control form-control-sm" placeholder="Cari Nomor"></asp:TextBox>
                                                <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-button-1">
                                                    <asp:LinkButton runat="server" ID="btnSearchContact" OnClick="btnSearchContact_Click">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-search" viewBox="0 0 16 16">
                                                          <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
                                                        </svg>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                            <asp:Literal runat="server" ID="ltlContact" Visible="false"></asp:Literal>
                                            <div class="gvWidthHight">
                                                <asp:GridView runat="server" ID="gvContact" AutoGenerateColumns="false" EmptyDataText="Contact Not Available"
                                                    CssClass="table table-responsive w-100 d-block d-md-table" GridLines="None" ShowHeader="false" Font-Size="X-Small"
                                                    OnRowDataBound="gvContact_RowDataBound" DataKeyNames="whatsapp_number" OnSelectedIndexChanged="gvContact_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:BoundField DataField="whatsapp_number" />
                                                        <asp:BoundField DataField="whatsapp_user" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="btnShowChat" CssClass="btn btn-sm">
                                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-chat-dots" viewBox="0 0 16 16">
                                                                  <path d="M5 8a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm4 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm3 1a1 1 0 1 0 0-2 1 1 0 0 0 0 2z"/>
                                                                  <path d="m2.165 15.803.02-.004c1.83-.363 2.948-.842 3.468-1.105A9.06 9.06 0 0 0 8 15c4.418 0 8-3.134 8-7s-3.582-7-8-7-8 3.134-8 7c0 1.76.743 3.37 1.97 4.6a10.437 10.437 0 0 1-.524 2.318l-.003.011a10.722 10.722 0 0 1-.244.637c-.079.186.074.394.273.362a21.673 21.673 0 0 0 .693-.125zm.8-3.108a1 1 0 0 0-.287-.801C1.618 10.83 1 9.468 1 8c0-3.192 3.004-6 7-6s7 2.808 7 6c0 3.193-3.004 6-7 6a8.06 8.06 0 0 1-2.088-.272 1 1 0 0 0-.711.074c-.387.196-1.24.57-2.634.893a10.97 10.97 0 0 0 .398-2z"/>
                                                                </svg>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="scrolling" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="card" style="height: 350px;">
                                        <div class="card-header">
                                            <div class="row">
                                                <asp:UpdatePanel runat="server" ID="updpnlHeader">
                                                    <ContentTemplate>
                                                        <div class="col">
                                                            <div class="d-flex">
                                                                <asp:Label runat="server" ID="lblHeaderChat" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="card-body" style="overflow-y: auto; overflow-x: auto">
                                            <asp:UpdatePanel runat="server" ID="updpnlchat" UpdateMode="Always" ClientIDMode="Static">
                                                <ContentTemplate>
                                                    <div class="imessage">
                                                        <asp:Literal runat="server" ID="ltlCc"></asp:Literal>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-group input-group-sm mt-2">
                                            <asp:DropDownList runat="server" ID="ddlTemplateMessage" CssClass="form-control form-control-sm" OnSelectedIndexChanged="ddlTemplateMessage_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <span class="input-group-text inputGroup-sizing-sm form-control-sm">
                                                <asp:FileUpload runat="server" ID="uploadmedia" />
                                            </span>
                                        </div>
                                        <div class="input-group input-group-sm mt-2">
                                            <asp:TextBox runat="server" ID="txtSent" CssClass="form-control form-control-sm" placeholder="Masukan Pesan" TextMode="MultiLine"></asp:TextBox>
                                            <span class="input-group-text inputGroup-sizing-sm form-control-sm">
                                                <asp:LinkButton runat="server" ID="btnSent" CssClass="btn btn-sm" OnClick="btnSent_Click">
                                                   <i class="fas fa-paper-plane"></i>
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div>
                                <asp:Literal runat="server" ID="privateContact" ClientIDMode="Static"></asp:Literal>
                            </div>
                            <!--/.Card-->
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModaladdTemplate" tabindex="-1" aria-labelledby="ModaladdTemplateLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModaladdTemplateLabel">Template
                        <asp:Label runat="server" ID="lblHeaderName" CssClass="badge bg-secondary"></asp:Label></h5>
                    <asp:LinkButton runat="server" ID="btnViewCloseTop" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="updPnlModalView">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col">
                                    <label class="form-label">Template</label>
                                    <asp:TextBox runat="server" ID="txtTemplate" CssClass="form-control form-control-sm" TextMode="MultiLine" Height="100"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm btn-success" ID="btnViewClose">Close</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnaddtemplate" CssClass="btn btn-sm btn-success" OnClick="btnaddtemplate_Click">Save</asp:LinkButton>
                </div>
            </div>

        </div>
    </div>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat&display=swap" rel="stylesheet">
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript" language="javascript">
        function warningsalert() {
            Swal.fire({
                icon: 'error',
                text: "Session Devices Tidak Ditemukan",
                type: 'warning',
                confirmButtonColor: '#3cd4a4',
            })
        }

        function showBrowseDialog() {
            document.getElementById('<%=uploadmedia.ClientID%>').click();
        }
        $(document).ready(function () {
            $('#<%=gvContact.ClientID %>').Scrollable();
        })
    </script>
    <style>
        body {
            font-family: 'Montserrat', sans-serif;
        }

        .UploadFile {
            display: none;
        }

        .form-control-sm {
            height: calc(1em + .375rem + 2px) !important;
            padding: .125rem .25rem !important;
            font-size: .75rem !important;
            line-height: 1.5;
            border-radius: .2rem;
        }

        .scrolling {
            position: absolute;
        }

        .gvWidthHight {
            overflow: scroll;
            height: 385px;
            width: 230px;
        }

        .card-horizontal {
            display: flex;
            flex: 1 1 auto;
        }

        .scroll-cards-height {
            position: relative;
            overflow-y: scroll;
            height: 400px;
        }

        /* .card-img-top {
    height: 20vw;
    width: 55vw;
} */

        .card-footer {
            /* position:absolute; */
            bottom: 0;
            border: none;
            background-color: #FFFFFF;
        }
    </style>
    <style>
        .imessage {
            display: flex;
            flex-direction: column;
            font-family: 'Montserrat', sans-serif;
            font-size: smaller;
        }

            .imessage p {
                border-radius: 1.15rem;
                line-height: 1.25;
                max-width: 75%;
                padding: 0.5rem .875rem;
                position: relative;
                word-wrap: break-word;
            }

                .imessage p::before,
                .imessage p::after {
                    bottom: -0.1rem;
                    content: "";
                    height: 1rem;
                    position: absolute;
                }

        p.from-me {
            align-self: flex-end;
            background-color: #3cd4a4;
            color: #fff;
        }

            p.from-me::before {
                border-bottom-left-radius: 0.8rem 0.7rem;
                border-right: 1rem solid #3cd4a4;
                right: -0.35rem;
                transform: translate(0, -0.1rem);
            }

            p.from-me::after {
                background-color: #fff;
                border-bottom-left-radius: 0.5rem;
                right: -40px;
                transform: translate(-30px, -2px);
                width: 10px;
            }

        p[class^="from-"] {
            margin: 0.5rem 0;
            width: fit-content;
        }

        p.from-me ~ p.from-me {
            margin: 0.25rem 0 0;
        }

            p.from-me ~ p.from-me:not(:last-child) {
                margin: 0.25rem 0 0;
            }

            p.from-me ~ p.from-me:last-child {
                margin-bottom: 0.5rem;
            }

        p.from-them {
            align-items: flex-start;
            background-color: #e5e5ea;
            color: #000;
        }

            p.from-them:before {
                border-bottom-right-radius: 0.8rem 0.7rem;
                border-left: 1rem solid #e5e5ea;
                left: -0.35rem;
                transform: translate(0, -0.1rem);
            }

            p.from-them::after {
                background-color: #fff;
                border-bottom-right-radius: 0.5rem;
                left: 20px;
                transform: translate(-30px, -2px);
                width: 10px;
            }

        p[class^="from-"].emoji {
            background: none;
            font-size: 2.5rem;
        }

            p[class^="from-"].emoji::before {
                content: none;
            }

        .no-tail::before {
            display: none;
        }

        .margin-b_none {
            margin-bottom: 0 !important;
        }

        .margin-b_one {
            margin-bottom: 1rem !important;
        }

        .margin-t_one {
            margin-top: 1rem !important;
        }
        /* general styling */
        @font-face {
            font-family: "SanFrancisco";
            src: url("https://cdn.rawgit.com/AllThingsSmitty/fonts/25983b71/SanFrancisco/sanfranciscodisplay-regular-webfont.woff2") format("woff2"), url("https://cdn.rawgit.com/AllThingsSmitty/fonts/25983b71/SanFrancisco/sanfranciscodisplay-regular-webfont.woff") format("woff");
        }

        body {
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen-Sans, Ubuntu, Cantarell, "Helvetica Neue", sans-serif;
            font-weight: normal;
            margin: 0;
        }

        .container {
            margin: 0 auto;
            max-width: 600px;
            padding: 1rem;
        }

        h1 {
            font-weight: normal;
            margin-bottom: 0.5rem;
        }

        h2 {
            border-bottom: 1px solid #e5e5ea;
            color: #666;
            font-weight: normal;
            margin-top: 0;
            padding-bottom: 1.5rem;
        }

        .comment {
            color: #222;
            font-size: 1.25rem;
            line-height: 1.5;
            margin-bottom: 1.25rem;
            max-width: 100%;
            padding: 0;
        }

        @media screen and (max-width: 800px) {
            body {
                margin: 0 0.5rem;
            }

            .container {
                padding: 0.5rem;
            }

            .imessage {
                font-size: 1.05rem;
                margin: 0 auto 1rem;
                max-width: 600px;
                padding: 0.25rem 0.875rem;
            }

                .imessage p {
                    margin: 0.5rem 0;
                }
        }
    </style>
    <style>
        .imessage .fromreply {
            border-radius: 1.15rem;
            line-height: 1.25;
            max-width: 75%;
            padding: 0.5rem .875rem;
            position: relative;
            word-wrap: break-word;
        }

            .imessage .fromreply::before,
            .imessage .fromreply::after {
                bottom: -0.1rem;
                content: "";
                height: 1rem;
                position: absolute;
            }

        .imessage .fromreplythem {
            border-radius: 1.15rem;
            line-height: 1.25;
            max-width: 75%;
            padding: 0.5rem .875rem;
            position: relative;
            word-wrap: break-word;
        }

            .imessage .fromreplythem::before,
            .imessage .fromreplythem::after {
                bottom: -0.1rem;
                content: "";
                height: 1rem;
                position: absolute;
            }

        .fromreply {
            align-self: flex-end;
            background-color: #3cd4a4;
            color: #fff;
        }

            .fromreply::before {
                border-bottom-left-radius: 0.8rem 0.7rem;
                border-right: 1rem solid #3cd4a4;
                right: -0.35rem;
                transform: translate(0, -0.1rem);
            }

            .fromreply::after {
                background-color: #fff;
                border-bottom-left-radius: 0.5rem;
                right: -40px;
                transform: translate(-30px, -2px);
                width: 10px;
            }

            .fromreply[class^="from"] {
                margin: 0.5rem 0;
                width: fit-content;
            }

            .fromreply ~ .fromreply {
                margin: 0.25rem 0 0;
            }

                .fromreply ~ .fromreply:not(:last-child) {
                    margin: 0.25rem 0 0;
                }

                .fromreply ~ .fromreply:last-child {
                    margin-bottom: 0.5rem;
                }


        .fromreplythem {
            align-items: flex-start;
            background-color: #e5e5ea;
            color: #000;
        }

            .fromreplythem::before {
                border-bottom-right-radius: 0.8rem 0.7rem;
                border-left: 1rem solid #e5e5ea;
                left: -0.35rem;
                transform: translate(0, -0.1rem);
            }

            .fromreplythem::after {
                background-color: #fff;
                border-bottom-right-radius: 0.5rem;
                left: 20px;
                transform: translate(-30px, -2px);
                width: 10px;
            }

            .fromreplythem[class^="from"] {
                margin: 0.5rem 0;
                width: fit-content;
            }

            .fromreplythem ~ .fromreplythem {
                margin: 0.25rem 0 0;
            }

                .fromreplythem ~ .fromreplythem:not(:last-child) {
                    margin: 0.25rem 0 0;
                }

                .fromreplythem ~ .fromreplythem:last-child {
                    margin-bottom: 0.5rem;
                }
    </style>
</asp:Content>
