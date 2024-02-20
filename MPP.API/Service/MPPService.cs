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

                if (contentPdf != null)
                {

                    string htmlcontent = SetTableMPP(contentPdf, kodeRegion);

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
                results = "<td style='padding: 0; margin: 0;border: 1px solid black;height: " + height.ToString() + "px;width=" + width.ToString() + "px; border-collapse: collapse;background-color:" + backgroundColor + ";' colspan=" + colspan.ToString() + ">" + valSpan+ "</td>";
            else if (rowspan > 0)
                results = "<td style='padding: 0; margin: 0;border: 1px solid black;height: " + height.ToString() + "px;width=" + width.ToString() + "px; border-collapse: collapse;background-color:" + backgroundColor + ";' rowspan=" + rowspan.ToString() + ">" + valSpan + "</td>";
         
            return results;

        }


        private string SetTableMPP(IEnumerable<T_MsMPP?> contentPDF, string kodeRegion)
        {

            string htmlcontent = "<div style='margin-top:-20px; padding-top:50px;'>";
            htmlcontent += setSpanValue("Yth") + "<br>";
            htmlcontent += setSpanValue("Bapak  Pimpinan Kebun", bold: "bold") + "<br>";
            htmlcontent += setSpanValue("Mohon segera dilakukan pengurangan tenaga kerja atau pembaharuan persetujuan over TK pada PT berikut:") + "<br>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align: center;'>";
            htmlcontent += "<table style='border: 0.75px solid black; border-collapse: collapse; width: 90%;margin-left:25px;'>";

            htmlcontent += "<tr>";
            htmlcontent += setColumTable(setSpanValue("Monitoring MPP Region " + kodeRegion, bold: "bold"), height: 20, colspan: 8);
            htmlcontent += "</tr>";

            htmlcontent += "<tr>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:5px;background-color:#2E6443;' rowspan=2>" + setSpanValue("No", "white") + "</td>";
            //htmlcontent += setColumTable(setSpanValue("Business Unit", "white"), rowspan: 2, backgroundColor: "#2E6443");
            htmlcontent += setColumTable(setSpanValue("Business Unit", "white"), rowspan: 2, backgroundColor: "#2E6443", width: 25);
            htmlcontent += setColumTable(setSpanValue("STD COO", "white"), rowspan: 2, backgroundColor: "#2E6443");
            htmlcontent += setColumTable(setSpanValue("Tenaga Kerja", "white"), colspan: 2, backgroundColor: "#2E6443");
            htmlcontent += setColumTable(setSpanValue("Variance", "white"), colspan: 2, backgroundColor: "#2E6443");
            htmlcontent += "</tr>";

            htmlcontent += "<tr>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Budget TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Aktual TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Budget vs COO", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Aktual vs COO", "white") + "</td>";
            htmlcontent += "</tr>";



            // htmlcontent += "<tr>";
            // htmlcontent += setColumTable(setSpanValue("Budget TK", "white"), backgroundColor: "#2E6443");
            // htmlcontent += setColumTable(setSpanValue("Aktual TK", "white"), backgroundColor: "#2E6443");
            // htmlcontent += setColumTable(setSpanValue("Variance over", "white"), backgroundColor: "#2E6443");
            // htmlcontent += "</tr>";



            int totalAktual = 0;
            int totalBdgt = 0;
            int totalCOO = 0;
            int totalVarianceBudgetCOO = 0;
            int totalVarianceAktualCOO = 0;
            bool createColumn = true;


            foreach (var dataResponse in contentPDF)
            {

                htmlcontent += "<tr>";

                foreach (var property in typeof(T_MsMPP).GetProperties())
                {

                    object? value = property.GetValue(dataResponse);

                    if (property.Name.ToLower().Contains("aktual"))
                        totalAktual += totalAktual + Convert.ToInt32(value);
                    else if (property.Name.ToLower().Contains("totalCOO"))
                        totalCOO += totalCOO + Convert.ToInt32(value);
                    else if (property.Name.ToLower().Contains("bdgt"))
                        totalBdgt += totalBdgt + Convert.ToInt32(value);
                    else if (property.Name.Contains("varianceBudget"))
                        totalVarianceBudgetCOO += totalVarianceBudgetCOO + Convert.ToInt32(value);
                    else if (property.Name.Contains("varianceAktual"))
                        totalVarianceAktualCOO += totalVarianceAktualCOO + Convert.ToInt32(value);
                    else if (property.Name.Contains("EmailLevel") || property.Name.Contains("MasaBerlaku"))
                        createColumn = false;

                    if (createColumn != false)
                        htmlcontent += "<td style='padding: 0; margin: 0; font-size:10px; border: 1px solid black; height: 15px; border-collapse: collapse;'>" + value + "</td>";
                    else
                        createColumn = true;

                }

                htmlcontent += "</tr>";

            }

            htmlcontent += "<tr>";
            htmlcontent += setColumTable(setSpanValue("Total"), colspan: 2);
           //  htmlcontent += setColumTable(setSpanValue(totalCOO.ToString()));
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalCOO.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalBdgt.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalAktual.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalVarianceBudgetCOO.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalVarianceAktualCOO.ToString()) + "</td>";
            htmlcontent += "</tr>";
            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "<br>";
            htmlcontent += setSpanValue("Harap segera melakukan approve email man power planning", bold: "bold") + "<br>";

            return htmlcontent;

        }


    }
}