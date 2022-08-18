<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WhatsappMobil.Master" CodeBehind="SentPlan.aspx.cs" Inherits="whatsappmobil.SentPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl">
            <div class="row">
                <div class="col">
                    <h1 class="app-page-title gradient-custom-text">Whatsapp Plan</h1>
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
                                <div>
                                    <asp:HiddenField runat="server" ID="hiddenOperator" />
                                    <asp:HiddenField runat="server" ID="HiddenValuesServiceTerakhir" />
                                    <asp:HiddenField runat="server" ID="HiddenValues2" />
                                    <asp:HiddenField runat="server" ID="HiddenSumberData" />
                                    <asp:HiddenField runat="server" ID="HiddenBranchSplit" />
                                    <asp:HiddenField runat="server" ID="HiddenPlanStatus" />
                                    <asp:HiddenField runat="server" ID="HiddenPlanMedia" />
                                    <asp:HiddenField runat="server" ID="HiddenPlanIsMedia" />
                                    <asp:HiddenField runat="server" ID="HiddenScheduledMessage" />
                                    <asp:HiddenField runat="server" ID="HiddenScheduledMessageTime" />
                                    <asp:HiddenField runat="server" ID="HiddenIsMedia"/>
                                    <asp:HiddenField runat="server" ID="HiddenFileNameBeforUpdate"/>
                                </div>
                                <ul class="nav nav-tabs">
                                    <li class="nav-item">
                                        <a href="#planreport" class="nav-link active text-success" data-bs-toggle="tab">Report Plan</a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="#planinput" class="nav-link text-success" data-bs-toggle="tab">Input Plan</a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane fade" id="planinput">
                                        <asp:UpdatePanel runat="server" ID="updPnl1">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col">
                                                        <div class="row">
                                                            <div class="col">
                                                                <div class="mb-3 mt-4">
                                                                    <asp:Label runat="server" ID="lblNamaPlan" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Nama Plan</asp:Label>
                                                                    <div class="input-group input-group-sm">
                                                                        <asp:TextBox runat="server" ID="txtPlanTitle" CssClass="form-control form-control-sm" placeholder="Masukan Nama Plan"></asp:TextBox>
                                                                        <%--<span class="input-group-text inputGroup-sizing-sm form-control-sm">Survey ?</span>
                                                                        <span class="input-group-text inputGroup-sizing-sm form-control-sm">
                                                                            <asp:CheckBox runat="server" ID="checkboxSurvey" />
                                                                        </span>--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="mb-1">
                                                            <asp:Label runat="server" ID="lblsessionid" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Devices</asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlSessionId" CssClass="form-control form-control-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSessionId_SelectedIndexChanged">
                                                                <asp:ListItem Value="">Pilih Devices</asp:ListItem>
                                                                <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                                                <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                                                <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                                                <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                                                <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                                                <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>

                                                        <div class="mb-3" style="display: none;">
                                                            <div class="row">
                                                                <div class="col">
                                                                    <asp:Label runat="server" ID="lbltanggalScheduled" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Tanggal</asp:Label>
                                                                    <div class="input-group date" id="datetimepicker1" data-target-input="nearest">
                                                                        <asp:TextBox runat="server" ID="txtSentDate" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker1" placeholder="Tanggal Scheduled Plan"></asp:TextBox>
                                                                        <div class="input-group-append" data-target="#datetimepicker1" data-toggle="datetimepicker">
                                                                            <div class="input-group-text h-100 form-control-sm"><i class="fa fa-calendar"></i></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col">
                                                                    <asp:Label runat="server" ID="lblWaktuScheduled" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Waktu Scheduled Plan<asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="Waktu untuk generate plan">(*)</asp:Label> </asp:Label>
                                                                    <div class="input-group date" id="datetimepicker2" data-target-input="nearest">
                                                                        <asp:TextBox runat="server" ID="txtScheduledTime" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker2" placeholder="Waktu Scheduled Plan"></asp:TextBox>
                                                                        <div class="input-group-append" data-target="#datetimepicker2" data-toggle="datetimepicker">
                                                                            <div class="input-group-text h-100 form-control-sm"><i class="fas fa-clock"></i></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="mb-3" style="display: none;">
                                                            <div class="row">
                                                                <div class="col">
                                                                    <asp:Label runat="server" ID="lblScheduledPesan" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Tanggal Scheduled Pesan<asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="Schedule untuk Kirim Pesan">(*)</asp:Label> </asp:Label>
                                                                    <div class="input-group date" id="datetimepicker3" data-target-input="nearest">
                                                                        <asp:TextBox runat="server" ID="txtScheduledMessage" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker3" placeholder="Tanggal Scheduled Pesan"></asp:TextBox>
                                                                        <div class="input-group-append" data-target="#datetimepicker3" data-toggle="datetimepicker">
                                                                            <div class="input-group-text h-100 form-control-sm"><i class="fa fa-calendar"></i></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col">
                                                                    <asp:Label runat="server" ID="lblScheduledTime" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Waktu Scheduled Pesan<asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="Waktu untuk kirim pesan">(*)</asp:Label> </asp:Label>
                                                                    <div class="input-group date" id="datetimepicker4" data-target-input="nearest">
                                                                        <asp:TextBox runat="server" ID="txtScheduledTimePesan" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker4" placeholder="Waktu Scheduled Pesan"></asp:TextBox>
                                                                        <div class="input-group-append" data-target="#datetimepicker4" data-toggle="datetimepicker">
                                                                            <div class="input-group-text h-100 form-control-sm"><i class="fas fa-clock"></i></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <hr class="text-success" />

                                                        <div class="mb-1">
                                                            <asp:Label runat="server" ID="lblPesan" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Pesan</asp:Label>
                                                            <asp:TextBox runat="server" ID="txtMessageContent" Font-Size="Smaller" CssClass="form-control" TextMode="MultiLine" Height="100" placeholder="Masukan Pesan &#10;note : Pilih tombol dibawah untuk memilih template pesan &#10;Exp : Hallo Bpk/Ibu {name}"></asp:TextBox>
                                                        </div>

                                                        <div class="mb-3" style="overflow: auto; white-space: nowrap;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateName" Text="Name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateName_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateFullName" Text="Full_Name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateFullName_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateTglBeli" Text="Tgl_Beli" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateTglBeli_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateTypeKendaraan" Text="Type_Kendaraan" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateTypeKendaraan_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateTglSTNK" Text="Tgl_STNK" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateTglSTNK_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplatePlatNo" Text="Plat_No" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplatePlatNo_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateLastService" Text="Last_Service" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateLastService_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateBirthDate" Text="Birth_Date" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBirthDate_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateBranchName" Text="Branch_Name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBranchName_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateBranchAddress" Text="Branch_Address" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBranchAddress_Click"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="btnTemplateBranchPhone" Text="Branch_Phone" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnTemplateBranchPhone_Click"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>

                                                        <div class="mb-1">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblfile" Font-Size="Smaller" Font-Bold="true">Unggah File</asp:Label>
                                                                    </td>
                                                                    <td>&nbsp</td>
                                                                    <td>
                                                                        <asp:CheckBox CssClass="d-flex RadioButtonInterval" runat="server" ID="chkIsMedia" AutoPostBack="true" OnCheckedChanged="chkIsMedia_CheckedChanged" Font-Size="Smaller" Font-Bold="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblfilealert" ForeColor="Red" Text="(*)" ToolTip="Harap klik upload file terlebih dahulu"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <cc1:AjaxFileUpload ID="ajaxfileupload" runat="server" Visible="false" OnUploadComplete="ajaxfileupload_UploadComplete" MaximumNumberOfFiles="1" AllowedFileTypes="" ThrobberID="myThrobber" />
                                                            <asp:Label CssClass="d-flex flex-row-reverse" runat="server" Visible="false" ID="lblalertnote" Text="*Note : Harap klik upload untuk menambahkan file" Font-Size="X-Small" ForeColor="#ff0000" Font-Bold="true"></asp:Label>
                                                        </div>

                                                        <hr class="text-success" />
                                                        <div class="mb-3">
                                                            <asp:Label runat="server" ID="lblInterval" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Interval</asp:Label>
                                                            <asp:CheckBoxList runat="server" ID="checkBoxInterval" CellSpacing="2" Font-Size="Smaller" RepeatDirection="Horizontal" RepeatColumns="4" CssClass="RadioButtonInterval">
                                                                <asp:ListItem Value="1">Daily</asp:ListItem>
                                                                <%--<asp:ListItem Value="2">Weekly</asp:ListItem>
                                                                <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                                <asp:ListItem Value="4">Custom</asp:ListItem>--%>
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <hr class="text-success" />
                                                        <div class="mb-3" style="text-align: start">
                                                            <asp:Label runat="server" ID="lblbranch" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Branch</asp:Label>
                                                            <asp:CheckBoxList runat="server" Font-Size="Smaller" ID="checkboxBranch" RepeatDirection="Horizontal" RepeatColumns="6" CssClass="RadioButtonInterval">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <hr class="text-success" />
                                                        
                                                        <div class="mb-3">
                                                            <asp:Label runat="server" ID="lblCriteriaCust" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Kriteria Customer</asp:Label>
                                                        </div>
                                                        <div class="mb-0">
                                                            <table class="table table-striped">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="font-size: smaller">Pilih</th>
                                                                        <th style="font-size: smaller">Kriteria</th>
                                                                        <th style="font-size: smaller">Kondisi</th>
                                                                        <th style="font-size: smaller">Isi</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody style="font-size: smaller; text-align: center">
                                                                    <tr>
                                                                        <th>
                                                                            <asp:CheckBox runat="server" ID="checkboxSumberData" Checked="true" />
                                                                        </th>
                                                                        <td>Sumber Data</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlSumberDataOperator" CssClass="form-control form-control-sm">
                                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList OnSelectedIndexChanged="ddlSumberData_SelectedIndexChanged" AutoPostBack="true" runat="server" ID="ddlSumberData" CssClass="form-control form-control-sm">
                                                                                <asp:ListItem Value="">Choose</asp:ListItem>
                                                                                <asp:ListItem Value="H1">H1</asp:ListItem>
                                                                                <asp:ListItem Value="H2">H2</asp:ListItem>
                                                                                <asp:ListItem Value="All">All</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="checkboxTanggalSTNK" />
                                                                        </td>
                                                                        <td>Tanggal STNK</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlTanggalSTNK">
                                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtTanggalSTNK" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </td>
                                                                        <td>Hari</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="checkboxTanggalPembelian" />
                                                                        </td>
                                                                        <td>Tanggal Pembelian</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlTanggalPembelian">
                                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtTanggalPembelian" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </td>
                                                                        <td>Hari</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>
                                                                            <asp:CheckBox runat="server" ID="checkboxServiceTerakhir" />
                                                                        </th>
                                                                        <td>Service Terakhir</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlServiceTerakhir" CssClass="form-control form-control-sm">
                                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtServieTerakhir" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </td>
                                                                        <td>Hari</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="checkboxJumlahService" />
                                                                        </td>
                                                                        <td>Jumlah Service</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlJumlahService">
                                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtJumlahService" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </td>
                                                                        <td>Kali</td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="d-flex flex-row-reverse">
                                                    <asp:LinkButton runat="server" ID="btnSavePlan" OnClick="btnSavePlan_Click" CssClass="btn btn-sm gradient-custom-card-1 text-white">Save &nbsp; <i class="fa-solid fa-floppy-disk"></i></asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="tab-pane fade show active" id="planreport">
                                        <asp:UpdatePanel runat="server" ID="updPnl">
                                            <ContentTemplate>
                                                <div class="row mt-1">
                                                    <div class="col">
                                                        <div class="d-flex mt-1 mb-2">
                                                            <asp:LinkButton ToolTip="Refresh" runat="server" ID="btnRefresh" OnClick="btnRefresh_Click" CssClass="btn btn-sm gradient-custom-card-1 text-white rounded-circle"><i class="fas fa-redo"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="col">
                                                        <div class="d-flex flex-row-reverse">
                                                            <asp:DropDownList runat="server" ID="ddlViewAllBranch" Width="37" CssClass="form-control form-control-sm gradient-custom-card-1" ForeColor="White" BackColor="Black" AutoPostBack="true" OnSelectedIndexChanged="ddlViewAllBranch_SelectedIndexChanged">
                                                                <asp:ListItem Value="false">false</asp:ListItem>
                                                                <asp:ListItem Value="true">true</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label runat="server" ID="lblViewAllBranch" CssClass="mt-1" Font-Size="Smaller">View All Branch : &nbsp;</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:GridView runat="server" ID="gvPlan" AutoGenerateColumns="false" EmptyDataText="No Plan Available" GridLines="None"
                                                    CssClass="table table-responsive w-100 d-block d-md-table" OnRowCommand="gvPlan_RowCommand"
                                                    OnRowDataBound="gvPlan_RowDataBound" Font-Size="Small" AllowPaging="true"
                                                    OnPageIndexChanging="gvPlan_PageIndexChanging" PageSize="5" HeaderStyle-BackColor="#3cd4a4" HeaderStyle-ForeColor="White">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID_PLAN" Visible="false" />
                                                        <asp:BoundField DataField="PLAN_TITLE" HeaderText="Plan" />
                                                        <asp:BoundField DataField="INTERVAL" HeaderText="Interval" />
                                                        <asp:BoundField DataField="BRANCH_NAME" HeaderText="Cabang" />
                                                        <asp:BoundField DataField="BRANCH_ID" Visible="false" />
                                                        <%--<asp:BoundField DataField="SCHEDULE_DATE" HeaderText="Schedule" DataFormatString="{0: dd-MMM-yyyy}" />
                                                        <asp:BoundField DataField="SCHEDULE_TIME" HeaderText="Waktu" />--%>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblstatus" Text='<%# Bind("PLAN_STATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="IS_MEDIA" Visible="false" />
                                                        <asp:BoundField DataField="MEDIA_NAME" Visible="false" />
                                                        <asp:BoundField DataField="SCHEDULED_MESSAGE" Visible="false" />
                                                        <asp:BoundField DataField="SCHEDULED_MESSAGE_TIME" Visible="false" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton runat="server" ID="btnEdit" CommandName="EditPlan" CommandArgument='<%# Eval("ID_PLAN") %>' CssClass="btn btn-sm gradient-custom-card-1 text-white"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton runat="server" ID="btnViewPlan" OnClick="btnViewPlan_Click" CommandName="ViewPlan" CommandArgument='<%# Eval("ID_PLAN") + "|" + Eval("BRANCH_ID") + "|" + Eval("IS_MEDIA") + "|" + Eval("MEDIA_NAME") + "|" + Eval("SCHEDULED_MESSAGE") + "|" + Eval("SCHEDULED_MESSAGE_TIME") %>' CssClass="btn btn-sm gradient-custom-card-1 text-white"><i class="fa-solid fa-eye"></i></asp:LinkButton>
                                                                        </td>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:LinkButton runat="server" ID="btnDelete" CssClass="btn btn-sm gradient-custom-button-2 text-white" CommandName="Delete" CommandArgument='<%# Eval("ID_PLAN") %>' OnClientClick="if ( !confirm('Apakah ingin menghapus ? ')) return false;"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="gvPlan" />
                                            </Triggers>
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

    <div class="modal fade" id="ModalPlanEdit" tabindex="-1" aria-labelledby="ModalPlanViewLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalPlanEditLabel">Edit - 
                        <asp:Label runat="server" ID="lblEditPlan" CssClass="badge gradient-custom-button-1"></asp:Label>
                    </h5>
                    <asp:LinkButton runat="server" ID="btnEditCloseTop" ClientIDMode="Static" CausesValidation="false" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:HiddenField runat="server" ID="hiddenplanedit" />
                    <asp:UpdatePanel runat="server" ID="updEdit" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col">
                                        <div class="row">
                                            <div class="col">
                                                <div class="mb-3 mt-4">
                                                    <asp:Label runat="server" ID="Label1" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Nama Plan</asp:Label>
                                                    <div class="input-group input-group-sm">
                                                        <asp:TextBox runat="server" ID="txtEditPlan" CssClass="form-control form-control-sm" placeholder="Masukan Nama Plan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mb-3" style="display: none;">
                                            <div class="row">
                                                <div class="col">
                                                    <asp:Label runat="server" ID="Label4" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Tanggal Scheduled Pesan<asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="Schedule untuk Kirim Pesan">(*)</asp:Label> </asp:Label>
                                                    <div class="input-group date" id="datetimepicker7" data-target-input="nearest">
                                                        <asp:TextBox runat="server" ID="txtEditShceduledPesan" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker7" placeholder="Tanggal Scheduled Pesan"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#datetimepicker7" data-toggle="datetimepicker">
                                                            <div class="input-group-text h-100 form-control-sm"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col">
                                                    <asp:Label runat="server" ID="Label5" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Waktu Scheduled Pesan<asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="Waktu untuk kirim pesan">(*)</asp:Label> </asp:Label>
                                                    <div class="input-group date" id="datetimepicker8" data-target-input="nearest">
                                                        <asp:TextBox runat="server" ID="txtEditSecheduledWaktuPesan" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepicker8" placeholder="Waktu Scheduled Pesan"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#datetimepicker8" data-toggle="datetimepicker">
                                                            <div class="input-group-text h-100 form-control-sm"><i class="fas fa-clock"></i></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <hr class="text-success" />

                                        <div class="mb-1">
                                            <asp:Label runat="server" ID="Label6" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Pesan</asp:Label>
                                            <asp:TextBox runat="server" ID="txtEditPesan" Font-Size="Smaller" CssClass="form-control" TextMode="MultiLine" Height="100" placeholder="Masukan Pesan &#10;note : Pilih tombol dibawah untuk memilih template pesan &#10;Exp : Hallo Bpk/Ibu {name}"></asp:TextBox>
                                        </div>

                                        <div class="mb-3" style="overflow: auto; white-space: nowrap;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateName" Text="Name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateFullName" Text="Full_Name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateFullName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateTglBeli" Text="Tgl_Beli" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateTglBeli_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateTypeKendaraan" Text="Type_Kendaraan" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateTypeKendaraan_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateTglStnk" Text="Tgl_STNK" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateTglStnk_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplatePlatNo" Text="Plat_No" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplatePlatNo_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateLastService" Text="Last_Service" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateLastService_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateBirthDate" Text="Birth_Date" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateBirthDate_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateBranchName" Text="Branch_Name" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateBranchName_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateBranchAddress" Text="Branch_Address" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateBranchAddress_Click"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="btnEditTemplateBranchPhone" Text="Branch_Phone" CssClass="badge gradient-custom-card-1 text-white" OnClick="btnEditTemplateBranchPhone_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="mb-1">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label7" Font-Size="Smaller" Font-Bold="true">Unggah File</asp:Label>
                                                    </td>
                                                    <td>&nbsp</td>
                                                    <td>
                                                        <asp:CheckBox CssClass="d-flex RadioButtonInterval" runat="server" ID="chkEditIsMedia" AutoPostBack="true" OnCheckedChanged="chkEditIsMedia_CheckedChanged" Font-Size="Smaller" Font-Bold="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label8" ForeColor="Red" Text="(*)" ToolTip="Harap klik upload file terlebih dahulu"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <cc1:AjaxFileUpload ID="ajaxfileupload1" runat="server" Visible="false" OnUploadComplete="ajaxfileupload1_UploadComplete" MaximumNumberOfFiles="1" AllowedFileTypes="" ThrobberID="myThrobber" />
                                            <asp:Label CssClass="d-flex flex-row-reverse" runat="server" Visible="false" ID="Label9" Text="*Note : Harap klik upload untuk menambahkan file" Font-Size="X-Small" ForeColor="#ff0000" Font-Bold="true"></asp:Label>
                                        </div>

                                        <hr class="text-success" />
                                        <div class="mb-3">
                                            <asp:Label runat="server" ID="Label10" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Interval</asp:Label>
                                            <asp:CheckBoxList runat="server" ID="chkEditInterval" CellSpacing="2" Font-Size="Smaller" RepeatDirection="Horizontal" RepeatColumns="4" CssClass="RadioButtonInterval">
                                                <asp:ListItem Value="1">Daily</asp:ListItem>
                                                <%--<asp:ListItem Value="2">Weekly</asp:ListItem>
                                                                <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                <asp:ListItem Value="4">Custom</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </div>
                                        <hr class="text-success" />
                                        <div class="mb-3" style="text-align: start">
                                            <asp:Label runat="server" ID="Label11" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Branch</asp:Label>
                                            <asp:CheckBoxList runat="server" Font-Size="Smaller" ID="chkEditBranch" RepeatDirection="Horizontal" RepeatColumns="6" CssClass="RadioButtonInterval">
                                            </asp:CheckBoxList>
                                        </div>
                                        <hr class="text-success" />
                                        <div class="mb-3">
                                            <asp:Label runat="server" ID="Label12" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Devices</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlEditDevices" CssClass="form-control form-control-sm">
                                                <asp:ListItem Value="">Pilih Devices</asp:ListItem>
                                                <asp:ListItem Value="CIMONE">CIMONE</asp:ListItem>
                                                <asp:ListItem Value="CIMONE1">CIMONE 1</asp:ListItem>
                                                <asp:ListItem Value="CIMONE2">CIMONE 2</asp:ListItem>
                                                <asp:ListItem Value="CIMONE3">CIMONE 3</asp:ListItem>
                                                <asp:ListItem Value="CIMONE4">CIMONE 4</asp:ListItem>
                                                <asp:ListItem Value="CIMONE5">CIMONE 5</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <asp:Label runat="server" ID="Label13" CssClass="form-label d-flex" Font-Size="Smaller" Font-Bold="true">Kriteria Customer</asp:Label>
                                        </div>
                                        <div class="mb-0">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th style="font-size: smaller">Choose</th>
                                                        <th style="font-size: smaller">Fields</th>
                                                        <th style="font-size: smaller">Kondisi</th>
                                                        <th style="font-size: smaller">Value</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody style="font-size: smaller; text-align: start">
                                                    <tr>
                                                        <th>
                                                            <asp:CheckBox runat="server" ID="chkEditSumberData" />
                                                        </th>
                                                        <td>Sumber Data</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlEditSumberDataOperator" CssClass="form-control form-control-sm">
                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlSumberData_SelectedIndexChanged" AutoPostBack="true" runat="server" ID="ddlEditSumberData" CssClass="form-control form-control-sm">
                                                                <asp:ListItem Value="">Choose</asp:ListItem>
                                                                <asp:ListItem Value="H1">H1</asp:ListItem>
                                                                <asp:ListItem Value="H2">H2</asp:ListItem>
                                                                <asp:ListItem Value="All">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkEditTanggalStnk" />
                                                        </td>
                                                        <td>Tanggal STNK</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEditTanggalStnk">
                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEditTanggalStnk" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </td>
                                                        <td>Hari</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkEditTanggalBeli" />
                                                        </td>
                                                        <td>Tanggal Pembelian</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEditTanggalPembelian">
                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEditTanggalPembelian" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </td>
                                                        <td>Hari</td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <asp:CheckBox runat="server" ID="chkEditServiceTerakhir" />
                                                        </th>
                                                        <td>Service Terakhir</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlEditServiceTerakhir" CssClass="form-control form-control-sm">
                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEditServiceTerakhir" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </td>
                                                        <td>Hari</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkEditJumlahService" />
                                                        </td>
                                                        <td>Jumlah Service</td>
                                                        <td>
                                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEditJumlahService">
                                                                <asp:ListItem Value="=">=</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEditJumlahService" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </td>
                                                        <td>Kali</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-2 text-white" ClientIDMode="Static" ID="btnEditClose">Close</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnEditPlan" ClientIDMode="Static" OnClick="btnEditPlan_Click" CssClass="btn btn-sm gradient-custom-button-1 text-white">Update Plan</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalPlanView" tabindex="-1" aria-labelledby="ModalPlanViewLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalPlanViewLabel">
                        <asp:Label runat="server" ID="lblViewPlan"></asp:Label>
                        -
                                <asp:Label runat="server" ID="lblViewPlanBadge" CssClass="badge gradient-custom-button-1"></asp:Label>
                    </h5>
                    <asp:LinkButton runat="server" ID="btnViewCloseTop" ClientIDMode="Static" CausesValidation="false" CssClass="btn-close"></asp:LinkButton>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="updModalView" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col">
                                        <div class="mb-3">
                                            <asp:Label runat="server" ID="lblPlantitleView" CssClass="form-label" Font-Size="Smaller">Judul</asp:Label>
                                            <asp:TextBox runat="server" ID="txtPlanTitleView" CssClass="form-control form-control-sm" Font-Size="Smaller"></asp:TextBox>
                                        </div>
                                        <div class="mb-3" style="display: none;">
                                            <asp:Label runat="server" ID="lblScheduledView" CssClass="form-label" Font-Size="Smaller">Tanggal Scheduled Plan</asp:Label>
                                            <asp:TextBox runat="server" ID="lblSentDate" CssClass="form-control form-control-sm" Font-Size="Smaller"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="mb-3">
                                            <asp:Label runat="server" ID="lblDevicesView" CssClass="form-label" Font-Size="Smaller">Devices</asp:Label>
                                            <asp:TextBox runat="server" ID="txtDevicesView" CssClass="form-control form-control-sm" Font-Size="Smaller"></asp:TextBox>
                                        </div>
                                        <div class="mb-3" style="display: none;">
                                            <asp:Label runat="server" ID="lblScheduledTimeView" CssClass="form-label" Font-Size="Smaller">Waktu Scheduled Plan</asp:Label>
                                            <asp:TextBox runat="server" ID="txtScheduledTimeView" CssClass="form-control form-control-sm" Font-Size="Smaller"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col mb-3">
                                        <asp:Label runat="server" ID="lblPesanView" CssClass="form-label" Font-Size="Smaller">Pesan</asp:Label>
                                        <asp:TextBox runat="server" ID="lblMessageContentView" TextMode="MultiLine" CssClass="form-control" Font-Size="Smaller" Height="100"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4 mb-2">
                                        <div class="card">
                                            <div class="card-body">
                                                <asp:GridView runat="server" ID="gvPlanOperator" AutoGenerateColumns="false" CssClass="table" GridLines="None" Font-Size="X-Small">
                                                    <Columns>
                                                        <asp:BoundField DataField="kunci" HeaderText="Key" />
                                                        <asp:BoundField DataField="operator" HeaderText="Operator" HtmlEncode="false" />
                                                        <asp:BoundField DataField="isi" HeaderText="Value" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="card">
                                            <div class="card-body">
                                                <asp:GridView runat="server" ID="gvDataCats" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No Data Available" CssClass="table" Font-Size="X-Small" AllowPaging="true" OnPageIndexChanging="gvDataCats_PageIndexChanging" PageSize="5">
                                                    <Columns>
                                                        <asp:BoundField DataField="" HeaderText="Cust Name" />
                                                        <asp:BoundField DataField="" HeaderText="Whatsaapp No" />
                                                    </Columns>
                                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                </asp:GridView>
                                                <div class="mb-2 text-center">
                                                    <asp:Label runat="server" ID="lblCountRow" CssClass="ml-2 badge gradient-custom-button-1"></asp:Label>
                                                </div>
                                            </div>
                                            <asp:HiddenField runat="server" ID="hiddenPlanId" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" CssClass="btn btn-sm gradient-custom-button-2 text-white" ClientIDMode="Static" ID="btnViewClose">Close</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnViewGeneratePlan" ClientIDMode="Static" OnClick="btnViewGeneratePlan_Click" CssClass="btn btn-sm gradient-custom-button-1 text-white">Generate Plan</asp:LinkButton>
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
            $('#datetimepicker5').datetimepicker({
                format: 'DD/MM/YYYY',
            });
            $('#datetimepicker6').datetimepicker({
                format: 'HH:mm',
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });
            $('#datetimepicker7').datetimepicker({
                format: 'DD/MM/YYYY',
            });
            $('#datetimepicker8').datetimepicker({
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
            $('#<%= btnViewGeneratePlan.ClientID %>').click(function (e) {
                $('#ModalPlanView').modal('hide');
            });
            $('#<%= btnViewCloseTop.ClientID %>').click(function (e) {
                $('#ModalPlanView').modal('hide');
            });
            $('#<%= btnEditCloseTop.ClientID %>').click(function (e) {
                $('#ModalPlanView').modal('hide');
            });
            $('#<%= btnEditClose.ClientID %>').click(function (e) {
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
        function alertsession(params, icon, paramsdesc) {
            Swal.fire({
                icon: icon,
                title: params,
                text: paramsdesc,
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

