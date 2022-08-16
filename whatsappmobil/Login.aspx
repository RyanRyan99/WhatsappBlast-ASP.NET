<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="whatsappmobil.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Whatsapp H2</title>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link type="image/png" sizes="16x16" rel="icon" href="assets/images/whatsapp.png" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="" />
    <link href="https://fonts.googleapis.com/css2?family=Montserrat&display=swap" rel="stylesheet" />
    <link
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css"
        rel="stylesheet" />
    <!-- Google Fonts -->
    <link
        href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
        rel="stylesheet" />
    <!-- MDB -->
    <link
        href="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/3.10.2/mdb.min.css"
        rel="stylesheet" />
    <!-- MDB -->
    <script
        type="text/javascript"
        src="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/3.10.2/mdb.min.js"></script>
    <style>
        .gradient-custom-2 {
            /* fallback for old browsers */
            background: #fccb90;
            /* Chrome 10-25, Safari 5.1-6 */
            background: -webkit-linear-gradient(to right, #10bd9e, #4cd9a4, #67e6a5);
            /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
            background: linear-gradient(to right, #10bd9e, #4cd9a4, #67e6a5);
        }

        @media (min-width: 768px) {
            .gradient-form {
                height: 100vh !important;
            }
        }

        @media (min-width: 769px) {
            .gradient-custom-2 {
                border-top-right-radius: .3rem;
                border-bottom-right-radius: .3rem;
            }
        }
        .gradient-custom-text {
            background-color: green;
            background-image: linear-gradient(to right, #10bd9e, #4cd9a4, #67e6a5);
            background-size: 100%;
            background-repeat: repeat;
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            -moz-background-clip: text;
            -moz-text-fill-color: transparent;
        }

        body {
            font-family: 'Montserrat', sans-serif;
        }
    </style>
    <script>
        function alert(errormessage, icon) {
            Swal.fire({
                icon: icon,
                text: errormessage,
                type: 'warning',
                confirmButtonColor: '#3cd4a4',
            })
        }
    </script>
</head>
<body>
    <section class="h-100 gradient-form" style="background-color: #eee;">
        <div class="container py-5 h-100">
            <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col-xl-10">
                    <div class="card rounded-3 text-black">
                        <div class="row g-0">
                            <div class="col-lg-6">
                                <div class="card-body p-md-5 mx-md-4">
                                    <div class="text-center">
                                        <img src="assets/images/whatsapp.png" style="width: 100px;" />
                                        <h4 class="mb-4 mt-4 gradient-custom-text" style="font-weight: bold">Whatsapp Blast</h4>
                                    </div>

                                    <form runat="server">
                                        <div class="form-outline mb-4">
                                            <label class="gradient-custom-text">Username</label>
                                            <asp:TextBox runat="server" ID="txtUserID" class="form-control" placeholder="Username" BackColor="#3cd4a4" ForeColor="White"></asp:TextBox>
                                        </div>

                                        <div class="form-outline mb-4">
                                            <label class="gradient-custom-text">Password</label>
                                            <asp:TextBox runat="server" ID="txtPassword" class="form-control" placeholder="Password" TextMode="Password" BackColor="#3cd4a4" ForeColor="White"></asp:TextBox>
                                        </div>

                                        <asp:Panel runat="server" DefaultButton="btnLogin">
                                            <div class="text-center pt-1 mb-1 pb-1">
                                                <asp:LinkButton runat="server" ID="btnLogin" OnClick="btnLogin_Click" CssClass="btn btn-primary btn-block fa-lg gradient-custom-2 mb-3">Sign In</asp:LinkButton>
                                            </div>
                                        </asp:Panel>
                                    </form>

                                </div>
                            </div>
                            <div class="col-lg-6 d-flex align-items-center gradient-custom-2">
                                <div class="text-white px-3 py-4 p-md-5 mx-md-4">
                                    <h5 class="mb-4 font-weight-bold">HONDA BINTANG CIMONE</h5>
                                    <hr class="my-3"/>
                                    <p class="small mb-0">Honda Bintang Cimone merupakan perusahaan yang bergerak di bidang penjualan dan jasa.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>
</html>

