using PdfSharpCore;
using PdfSharpCore.Pdf;
using MPP.API.Repository;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using MPP.API.Entities;
using Microsoft.Extensions.Options;

namespace MPP.API.Service
{
    public class MPPService
    {

        private static AppSettings? _appSettings;
        private readonly MPPRepository _MPPRepository;
        public MPPService(MPPRepository MPPRepository, IOptions<AppSettings> appSettings)
        {
            _MPPRepository = MPPRepository;
            _appSettings = appSettings.Value;

        }

        public async Task<bool?> generateApprovalMPP(string company, string location, string kodeRegion, string tahun, string bulan)
        {
            try
            {

                var document = new PdfDocument();

                var contentPdf = await _MPPRepository.getAllDataApprovalMPP(company, location, tahun, bulan);
                var contentPdfDetail = await _MPPRepository.getAllDataDetailApprovalMPP(company, location, tahun, bulan);

                if (contentPdf.Count() > 0 || contentPdfDetail.Count() > 0)
                {
                    string htmlcontent = SetTableMPP(contentPdf, contentPdfDetail, kodeRegion);


                    PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);

                    using (MemoryStream ms = new MemoryStream())
                    {

                        document.Save(ms);
                        var filename = "MPP_" + company + "_" + location + ".pdf";
                        byte[] response = ms.ToArray();
                        string fullPath = Path.Combine("C:\\AllProject\\Change Request\\2024\\MPP\\MPP.File\\PDF\\Riau", filename);
                        File.WriteAllBytes(fullPath, response);

                    }

                    return true;

                }
                else
                {

                    return false;

                }

            }
            catch
            {

                return false;

            }


        }

        public string setSpanValue(string val, string color = "black", string bold = "normal")
        {
            string results = "<span style='font-family:Arial, Helvetica, sans-serif;color:" + color + "; font-weight: " + bold + "; font-size: 10px;'>" + val + "</span>";
            return results;
        }

        public string setColumTable(string valSpan, int height = 12, int colspan = 0, int rowspan = 0, string backgroundColor = "white", int width = 12)
        {

            string results = "";

            if (colspan > 0)
                results = "<td style='padding: 0; margin: 0;border: 1px solid black;height: " + height.ToString() + "px;width=" + width.ToString() + "px; border-collapse: collapse;background-color:" + backgroundColor + ";' colspan=" + colspan.ToString() + ">" + valSpan + "</td>";
            else if (rowspan > 0)
                results = "<td style='padding: 0; margin: 0;border: 1px solid black;height: " + height.ToString() + "px;width=" + width.ToString() + "px; border-collapse: collapse;background-color:" + backgroundColor + ";' rowspan=" + rowspan.ToString() + ">" + valSpan + "</td>";

            return results;

        }



        public string setNormalColumTable(string valSpan, int height = 12, int colspan = 0, int rowspan = 0, string backgroundColor = "white", int width = 12)
        {

            string results = "";

            results = "<td style='padding: 0; margin: 0;border: 1px solid black;height: " + height.ToString() + "px;width=" + width.ToString() + "px; border-collapse: collapse;background-color:" + backgroundColor + ";'>" + valSpan + "</td>";

            return results;

        }


        private string SetTableMPP(IEnumerable<T_MsMPP?> contentPDF, IEnumerable<T_MsMPPDetail?> contentPDFDetail, string kodeRegion)
        {

            string catatan = contentPDF.FirstOrDefault().catatan;

            string htmlcontent = "<div style='margin-top:-20px; padding-top:50px;'>";
            htmlcontent += setSpanValue("Yth") + "<br>";
            htmlcontent += setSpanValue("Bapak  Pimpinan Kebun " + contentPDF.FirstOrDefault().state.ToString(), bold: "bold") + "<br>";
            htmlcontent += setSpanValue("Mohon persetujuan penambahan kelebihan tenaga kerja dengan rincian sebagai berikut:") + "<br>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align: center;'><br>";
            htmlcontent += "<table style='border: 0.75px solid black; border-collapse: collapse; width: 90%;margin-left:25px;'>";

            htmlcontent += "<tr>";
            htmlcontent += setColumTable(setSpanValue("Monitoring MPP Region " + kodeRegion, bold: "bold"), height: 20, colspan: 8);
            htmlcontent += "</tr>";

            htmlcontent += "<tr>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;' rowspan=2>" + setSpanValue("No Pengajuan", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;' rowspan=2>" + setSpanValue("Business Unit Name", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;' rowspan=2>" + setSpanValue("STD COO", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;' colspan=2>" + setSpanValue("Tenaga Kerja", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;' colspan=2>" + setSpanValue("Variance", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;' rowspan=2>" + setSpanValue("Bulan Berlaku", "white") + "</td>";
            htmlcontent += "</tr>";

            htmlcontent += "<tr>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;'>" + setSpanValue("Budget TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;'>" + setSpanValue("Aktual TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;'>" + setSpanValue("Aktual TK VS Budget", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:#2E6443;'>" + setSpanValue("Aktual VS COO", "white") + "</td>";
            htmlcontent += "</tr>";

            bool createColumn = true;


            foreach (var dataResponseDetail in contentPDFDetail)
            {

                string monthNameDetail = dataResponseDetail.BulanPriodeName;

                foreach (var dataResponse in contentPDF)
                {

                    htmlcontent += "<tr>";

                    foreach (var property in typeof(T_MsMPP).GetProperties())
                    {

                        object? value = property.GetValue(dataResponse);

                        if (property.Name.ToLower() == "emaillevel" || property.Name.ToLower() == "state" || property.Name.ToLower() == "finalstate" || property.Name.ToLower() == "catatan" || property.Name.ToLower() == "masaberlakuint")
                        {
                            createColumn = false;
                        }

                        if (createColumn == true)
                        {
                            
                             if (property.Name.ToLower() == "masaberlakuto"){

                                  htmlcontent += "<td style='padding: 0; margin: 0; font-size:10px; border: 1px solid black; height: 15px;text-align:center; border-collapse: collapse;'>" + monthNameDetail.ToString() + "</td>";

                             }else{

                                  htmlcontent += "<td style='padding: 0; margin: 0; font-size:10px; border: 1px solid black; height: 15px;text-align:center; border-collapse: collapse;'>" + value.ToString() + "</td>";
                       
                             }

                          
                        }
                        createColumn = true;

                    }
                    htmlcontent += "</tr>";

                }
            }

            htmlcontent += "<tr>";
            htmlcontent += setColumTable(setSpanValue("Catatan"), colspan: 2);
            htmlcontent += setColumTable(setSpanValue(catatan.ToString()), colspan: 6);
            htmlcontent += "</tr>";

            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "<br>";
            htmlcontent += setSpanValue("Harap segera melakukan approve email man power planning", bold: "bold") + "<br>";

            return htmlcontent;

        }


    }
}