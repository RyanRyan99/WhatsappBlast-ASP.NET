<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" MaintainScrollPositionOnPostback="true" CodeBehind="Listclient.aspx.cs" Inherits="whatsappmobil.Listclient" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">List Client</h1>
                </div>
            </div>
            <div class="row  g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">
                            <div class="row">
                                <div class="col d-flex flex-row">
                                    <p>Buat Client</p>
                                </div>
                                <div class="col"></div>
                                <div class="col"></div>
                                <div class="col"></div>
                                <hr />
                            </div>
                            <div class="row">
                                <div class="col">
                                   <iframe runat="server" src="http://192.168.100.1:9001/" width="900" height="1000"></iframe>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
