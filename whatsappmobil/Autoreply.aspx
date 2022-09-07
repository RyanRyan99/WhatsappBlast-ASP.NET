<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="Autoreply.aspx.cs" Inherits="whatsappmobil.Autoreply" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Auto Reply</h1>
                </div>
                <div class="col">
                    <div class="d-flex flex-row-reverse">
                        <div class="row">
                            <div class="input-group input-group-sm">
                                <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control form-control-sm" placeholder="Search Reply"></asp:TextBox>
                                <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-button-1">
                                    <asp:LinkButton runat="server" ID="btnSearch" Width="30" OnClick="btnSearch_Click"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">
                            <div class="row">
                                <ul class="nav nav-tabs">
                                    <li class="nav-item">
                                        <a href="#replyreport" class="nav-link active text-success" data-bs-toggle="tab">Reply</a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="#replyinput" class="nav-link text-success" data-bs-toggle="tab">Input</a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane fade show active" id="replyreport">
                                        <asp:UpdatePanel runat="server" ID="updpnl">
                                            <ContentTemplate>
                                                <div class="mb-2 mt-2">
                                                    <asp:GridView runat="server" ID="gvReply" AutoGenerateColumns="false"
                                                        CssClass="table table-responsive w-100 d-block d-md-table" GridLines="None"
                                                        HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White" Font-Size="Smaller" OnRowCommand="gvReply_RowCommand" OnPageIndexChanging="gvReply_PageIndexChanging" PageSize="10">
                                                        <Columns>
                                                            <asp:BoundField DataField="sender_id" HeaderText="Session" />
                                                            <asp:BoundField DataField="message_reply" HeaderText="Reply" />
                                                            <asp:BoundField DataField="template_reply" HeaderText="Template Reply" />
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnDelete" CommandName="btndelete" CommandArgument='<%# Eval("sender_id") + "|" + Eval("message_reply") + "|" + Eval("template_reply") %>' CssClass="btn btn-sm gradient-custom-button-2 text-white"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="tab-pane fade" id="replyinput">
                                        <asp:UpdatePanel runat="server" ID="updpnl2">
                                            <ContentTemplate>
                                                <div class="mt-2 mb-2">
                                                    <asp:Label runat="server" ID="lblSessionId" CssClass="form-label d-flex" Font-Bold="true" Font-Size="Smaller">Devices</asp:Label>
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
                                                <div class="row">
                                                    <div class="col">
                                                        <div class="mb-2 mt-3">
                                                            <asp:Label runat="server" ID="lblReplyContent" Font-Bold="true" CssClass="form-label d-flex" Font-Size="Smaller">Pesan Reply</asp:Label>
                                                            <asp:TextBox runat="server" ID="txtReplyContent" CssClass="form-control" Font-Size="Smaller" TextMode="MultiLine" Height="150" placeholder="Reply  &#10;Exp: Selamat Pagi"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <div class="mb-2 mt-3">
                                                            <asp:Label runat="server" ID="lblReplyDesc" Font-Bold="true" CssClass="form-label d-flex" Font-Size="Smaller">Pesan</asp:Label>
                                                            <asp:TextBox runat="server" ID="txtReplyDesc" CssClass="form-control" Font-Size="Smaller" TextMode="MultiLine" Height="150" placeholder="Pesan yang dibalaskan &#10;Exp: Selamat Pagi Juga"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="mb-2 mt-2">
                                                        <div class="d-flex flex-row-reverse">
                                                            <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm gradient-custom-button-1 text-white" OnClick="btnSave_Click">Save &nbsp; <i class="fa-solid fa-floppy-disk"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
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
    </div>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
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
    </style>
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
</asp:Content>
