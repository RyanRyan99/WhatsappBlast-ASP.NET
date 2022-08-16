<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" MaintainScrollPositionOnPostback="true" CodeBehind="Dashboard.aspx.cs" Inherits="whatsappmobil.Dashboard" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <h1 class="app-page-title text-color-custom">Dashboard</h1>
            <div class="g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="row">
                        <div class="col mt-2">
                            <div class="app-card app-card-stat shadow-sm h-100 gradient-custom-card-2">
                                <div id="cardPlan">
                                    <div class="app-card-body p-3 p-lg-4">
                                        <h4 class="stats-type mb-1 text-white">Total Target</h4>
                                        <div class="stats-figure">
                                            <asp:Label runat="server" ID="lblTotalTarget" CssClass="text-white"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <a class="app-card-link-mask" href="SentPlan.aspx"></a>
                            </div>
                        </div>
                        <div class="col mt-2">
                            <div class="app-card app-card-stat shadow-sm h-100 gradient-custom-card-3">
                                <div class="app-card-body p-3 p-lg-4">
                                    <h4 class="stats-type mb-1 text-white">Total Message</h4>
                                    <div class="stats-figure">
                                        <asp:Label runat="server" ID="lblSentMessage" CssClass="text-white"></asp:Label>
                                    </div>
                                </div>
                                <a class="app-card-link-mask" href="SentWhatsapp.aspx"></a>
                            </div>
                        </div>
                        <div class="col mt-2">
                            <div class="app-card app-card-stat shadow-sm h-100 gradient-custom-card-2">
                                <div class="app-card-body p-3 p-lg-4">
                                    <h4 class="stats-type mb-1 text-white">Total Media</h4>
                                    <div class="stats-figure">
                                        <asp:Label runat="server" ID="lblSentMedia" CssClass="text-white"></asp:Label>
                                    </div>
                                </div>
                                <a class="app-card-link-mask" href="SentWhatsappMedia.aspx"></a>
                            </div>
                        </div>
                        <div class="col mt-2">
                            <div class="app-card app-card-stat shadow-sm h-100 gradient-custom-card-3">
                                <div class="app-card-body p-3 p-lg-4">
                                    <h4 class="stats-type mb-1 text-white">Total Plan</h4>
                                    <div class="stats-figure">
                                        <asp:Label runat="server" ID="lblPlan" CssClass="text-white"></asp:Label>
                                    </div>
                                </div>
                                <a class="app-card-link-mask" href="SentPlan.aspx"></a>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-4">
                        <h1 class="app-page-title text-color-custom">Chart</h1>
                        <div class="col">
                            <div class="col-md-12">
                                <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                                    <div class="card-body">
                                        <canvas id="myChart"></canvas>
                                        <asp:HiddenField runat="server" ID="lblJanuari" />
                                        <asp:HiddenField runat="server" ID="lblFebruari" />
                                        <asp:HiddenField runat="server" ID="lblMaret" />
                                        <asp:HiddenField runat="server" ID="lblApril" />
                                        <asp:HiddenField runat="server" ID="lblMei" />
                                        <asp:HiddenField runat="server" ID="lblJuni" />
                                        <asp:HiddenField runat="server" ID="lblJuli" />
                                        <asp:HiddenField runat="server" ID="lblAgustus" />
                                        <asp:HiddenField runat="server" ID="lblSeptember" />
                                        <asp:HiddenField runat="server" ID="lblOktober" />
                                        <asp:HiddenField runat="server" ID="lblNovember" />
                                        <asp:HiddenField runat="server" ID="lblDesember" />

                                        <asp:HiddenField runat="server" ID="pJanuari" />
                                        <asp:HiddenField runat="server" ID="pFebruari" />
                                        <asp:HiddenField runat="server" ID="pMaret" />
                                        <asp:HiddenField runat="server" ID="pApril" />
                                        <asp:HiddenField runat="server" ID="pMei" />
                                        <asp:HiddenField runat="server" ID="pJuni" />
                                        <asp:HiddenField runat="server" ID="pJuli" />
                                        <asp:HiddenField runat="server" ID="pAgustus" />
                                        <asp:HiddenField runat="server" ID="pSeptember" />
                                        <asp:HiddenField runat="server" ID="pOktober" />
                                        <asp:HiddenField runat="server" ID="pNovember" />
                                        <asp:HiddenField runat="server" ID="pDesember" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style>
        .gradient-custom-card-2 {
            background: #fccb90;
            background: -webkit-linear-gradient(to right, #19c19e, #4cd9a4, #67e6a5);
            background: linear-gradient(to right, #19c19e, #4cd9a4, #67e6a5);
        }
        .gradient-custom-card-3 {
            background: #fccb90;
            background: -webkit-linear-gradient(to right, #67e6a5, #4cd9a4, #19c19e);
            background: linear-gradient(to right, #67e6a5, #4cd9a4, #19c19e);
        }
        .text-color-custom{
            background-color: green;
            background-image: linear-gradient(to right, #10bd9e, #4cd9a4, #67e6a5);
            background-size: 100%;
            background-repeat: repeat;
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            -moz-background-clip: text;
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/chart.js@3.8.0/dist/chart.min.js"></script>
    <script>
        const ctx = document.getElementById('myChart').getContext('2d');
        var Jan = document.getElementById('<%= lblJanuari.ClientID %>').value;
        var Feb = document.getElementById('<%= lblFebruari.ClientID %>').value;
        var Mar = document.getElementById('<%= lblMaret.ClientID %>').value;
        var Apr = document.getElementById('<%= lblApril.ClientID %>').value;
        var Mei = document.getElementById('<%= lblMei.ClientID %>').value;
        var Jun = document.getElementById('<%= lblJuni.ClientID %>').value;
        var Jul = document.getElementById('<%= lblJuli.ClientID %>').value;
        var Agu = document.getElementById('<%= lblAgustus.ClientID %>').value;
        var Sep = document.getElementById('<%= lblSeptember.ClientID %>').value;
        var Okt = document.getElementById('<%= lblOktober.ClientID %>').value;
        var Nov = document.getElementById('<%= lblNovember.ClientID %>').value;
        var Des = document.getElementById('<%= lblDesember.ClientID %>').value;

        var pJan = document.getElementById('<%= pJanuari.ClientID %>').value;
        var pFeb = document.getElementById('<%= pFebruari.ClientID %>').value;
        var pMar = document.getElementById('<%= pMaret.ClientID %>').value;
        var pApr = document.getElementById('<%= pApril.ClientID %>').value;
        var pMei = document.getElementById('<%= pMei.ClientID %>').value;
        var pJun = document.getElementById('<%= pJuni.ClientID %>').value;
        var pJul = document.getElementById('<%= pJuli.ClientID %>').value;
        var pAgu = document.getElementById('<%= pAgustus.ClientID %>').value;
        var pSep = document.getElementById('<%= pSeptember.ClientID %>').value;
        var pOkt = document.getElementById('<%= pOktober.ClientID %>').value;
        var pNov = document.getElementById('<%= pNovember.ClientID %>').value;
        var pDes = document.getElementById('<%= pDesember.ClientID %>').value;
        const myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'],
                datasets: [{
                    label: 'Message',
                    data: [Jan, Feb, Mar, Apr, Mei, Jun, Jul, Agu, Sep, Okt, Nov, Des],
                    borderColor: 'rgb(25,193,158)',
                    borderWidth: 4,
                },
                {
                    label: 'Plan',
                    data: [pJan, pFeb, pMar, pApr, pMei, pJun, pJul, pAgu, pSep, pOkt, pNov, pDes],
                    borderColor: 'rgb(103,230,165)',
                    borderWidth: 4
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    </script>
</asp:Content>
