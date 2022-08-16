<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="SentWhatsappGroup.aspx.cs" Inherits="whatsappmobil.SentWhatsappGroup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Blast Group</h1>
                </div>
                <div class="col">
                </div>
                <div class="col">
                    <div class="row">
                        <div class="input-group input-group-sm">
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control form-control-sm" placeholder="Search Group Name"></asp:TextBox>
                            <span class="input-group-text inputGroup-sizing-sm form-control-sm gradient-custom-button-1">
                                <asp:LinkButton runat="server" ID="btnSearch" Width="30" OnClick="btnSearch_Click"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row  g-12 mb-12">
                <div class="col-12 col-lg-12">
                    <div class="app-card app-card-stat shadow-sm h-100 border-left-decoration-custom border-buttom-decoration-custom">
                        <div class="app-card-body p-3 p-lg-4">
                            <div class="row">
                                <div class="col d-flex flex-row">
                                    <p>Target Group</p>
                                </div>
                                <div class="col d-flex flex-row-reverse">
                                    <asp:LinkButton runat="server" ID="btnAdd" OnClick="btnAdd_Click" CssClass="gradient-custom-text"><i class="fa-solid fa-square-plus fa-2xl"></i></asp:LinkButton>
                                </div>
                                <hr />
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:GridView runat="server" ID="gvBlastGroup" AutoGenerateColumns="false" AllowPaging="true" GridLines="None" Font-Size="Smaller"
                                        EmptyDataText="No Data Available" OnRowCommand="gvBlastGroup_RowCommand" OnPageIndexChanging="gvBlastGroup_PageIndexChanging"
                                        CssClass="table table-responsive w-100 d-block d-md-table" OnRowDataBound="gvBlastGroup_RowDataBound"
                                        HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White" PageSize="5">
                                        <Columns>
                                            <asp:BoundField DataField="trxid" Visible="false" />
                                            <asp:BoundField DataField="title_blast" HeaderText="Judul" />
                                            <asp:BoundField DataField="group_name" HeaderText="Nama Group" />
                                            <asp:BoundField DataField="scheduled_date" HeaderText="Scheduled" DataFormatString="{0: dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="scheduled_time" HeaderText="Waktu" />
                                            <asp:BoundField DataField="sender_id" HeaderText="Devices" />
                                            <asp:BoundField DataField="message_content" HeaderText="Pesan" Visible="false" />
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label Width="50" runat="server" ID="lblStatus" Text='<%# Bind("blast_status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="btnEdit" CommandName="EditData" CommandArgument='<%# Eval("trxid") %>' CssClass="btn btn-sm btn btn-sm gradient-custom-button-1 text-white"><i class="fas fa-pen"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="btnSent" CommandName="Sent" CommandArgument='<%# Eval("trxid") + "|" + Eval("group_name") + "|" + Eval("message_content") + "|" + Eval("sender_id") %>' CssClass="btn btn-sm" ToolTip="Blast Message" OnClientClick="if ( !confirm('Apakah ingin mengirim ? ')) return false;"><i class="fa-solid fa-paper-plane"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("trxid") %>' CssClass="btn btn-sm" ToolTip="Delete" OnClientClick="if ( !confirm('Apakah ingin Menghapus ? ')) return false;"><i class="fas fa-trash-alt"></i></asp:LinkButton>
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

    <div class="modal fade" id="ModalBlastGroupAdd" tabindex="-1" aria-labelledby="ModalBlastGroupAddLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <div class="col">
                            <h5 class="modal-title" id="ModalBlastGroupAddLabel">
                                <asp:Label runat="server" ID="lblViewPlan" Text="Blast Group"></asp:Label>
                            </h5>
                        </div>

                    </div>
                    <asp:LinkButton runat="server" ID="btnViewCloseTop" ClientIDMode="Static" CausesValidation="false" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="updModalView" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <asp:Label runat="server" ID="lblAlert" CssClass="alert alert-danger form-control-sm mb-1 w-100" Visible="false">
                                    Status Tidak OPEN !!
                                </asp:Label>
                                <div class="row">
                                    <div class="col">
                                        <div class="mb-3">
                                            <label for="txtBlastGroupHeader" class="form-label" style="font-size: smaller; font-weight: bold;">Judul Blast</label>
                                            <div class="input-group input-group-sm">
                                                <asp:HiddenField runat="server" ID="hiddenTrxId" />
                                                <asp:HiddenField runat="server" ID="hiddenBlastStatus" />
                                                <asp:TextBox runat="server" ID="txtBlastGroupHeader" CssClass="form-control form-control-sm" placeholder="Masukan Judul"></asp:TextBox>
                                                <span class="input-group-text inputGroup-sizing-sm form-control-sm">Media</span>
                                                <span class="input-group-text inputGroup-sizing-sm form-control-sm">
                                                    <asp:CheckBox runat="server" ID="ChkisMedia" AutoPostBack="true" OnCheckedChanged="ChkisMedia_CheckedChanged" />
                                                </span>
                                            </div>
                                        </div>
                                        <div class="mb-3">
                                            <cc1:AjaxFileUpload ID="ajaxfileupload" runat="server" Visible="false" OnUploadComplete="ajaxfileupload_UploadComplete" MaximumNumberOfFiles="1" AllowedFileTypes="" ThrobberID="myThrobber" />
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label" style="font-size: smaller; font-weight: bold;">Nama Group</label>
                                            <asp:TextBox runat="server" ID="txtNamaGroup" CssClass="form-control form-control-sm" placeholder="Nama Group WhatsApp"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label" style="font-size: smaller; font-weight: bold;">Session Devices</label>
                                            <asp:DropDownList runat="server" ID="ddlSessionId" CssClass="form-control form-control-sm">
                                                <asp:ListItem Value="Pilih Devices"></asp:ListItem>
                                                <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                                <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                                <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                                <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                                <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                                <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <div class="row">
                                                <div class="col">
                                                    <label for="" class="form-label" style="font-size: smaller; font-weight: bold;">Scheduled</label>
                                                    <div class="input-group date" id="datetimepicker1" data-target-input="nearest">
                                                        <asp:TextBox placeholder="Tanggal Kirim" runat="server" ID="txtBlastDate" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker1"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#datetimepicker1" data-toggle="datetimepicker">
                                                            <div class="input-group-text h-100 form-control-sm"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col">
                                                    <label for="" class="form-label" style="font-size: smaller; font-weight: bold;">Waktu Scheduled</label>
                                                    <div class="input-group date" id="datetimepicker2" data-target-input="nearest">
                                                        <asp:TextBox placeholder="Waktu Kirim" runat="server" ID="txtScheduledtime" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker2"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#datetimepicker2" data-toggle="datetimepicker">
                                                            <div class="input-group-text h-100 form-control-sm"><i class="fas fa-clock"></i></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="mb-1">
                                            <label class="form-label" style="font-size: smaller; font-weight: bold;">Pesan Group</label>
                                            <asp:TextBox runat="server" ID="txtPesanGroup" CssClass="form-control" TextMode="MultiLine" Font-Size="Smaller" Height="105" placeholder="Masukan Isi Pesan"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-2 text-white" ClientIDMode="Static" ID="btnViewClose">Close</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAddBlastGroup" ClientIDMode="Static" CssClass="btn btn-sm gradient-custom-button-1 text-white" OnClick="btnAddBlastGroup_Click">Save</asp:LinkButton>
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
        });
        $(document).ready(function () {
            $('#<%= btnViewClose.ClientID %>').click(function (e) {
                $('#ModalPlanView').modal('hide');
            });
            $('#<%= btnAddBlastGroup.ClientID %>').click(function (e) {
                $('#ModalPlanView').modal('hide');
            });
            $('#<%= btnViewCloseTop.ClientID %>').click(function (e) {
                $('#ModalPlanView').modal('hide');
            });
        });

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

        #ContentPlaceHolder1_ajaxfileupload_SelectFileButton {
            background-color: #3cd4a4;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }

        #ContentPlaceHolder1_ajaxfileupload_Html5DropZone {
            color: #3cd4a4;
            font-weight: bold;
            font-size: smaller;
        }

        #ContentPlaceHolder1_ajaxfileupload_FileStatusContainer {
            color: #3cd4a4;
            font-weight: bold;
            font-size: smaller;
        }

        #ContentPlaceHolder1_ajaxfileupload_ctl00 {
            border-color: #3cd4a4;
            border-width: 2px;
        }

        #ContentPlaceHolder1_ajaxfileupload_Html5DropZone {
            border-color: #3cd4a4;
        }

        #ContentPlaceHolder1_ajaxfileupload_QueueContainer {
            color: #3cd4a4 !important;
        }

        #ContentPlaceHolder1_ajaxfileupload_UploadOrCancelButton {
            background-color: #3cd4a4;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }
    </style>
</asp:Content>
