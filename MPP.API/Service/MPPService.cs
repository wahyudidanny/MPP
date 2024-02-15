using PdfSharpCore;
using PdfSharpCore.Pdf;
using MPP.API.Repository;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using MPP.API.Entities;

namespace MPP.API.Service
{
    public class MPPService
    {
        private readonly MPPRepository _MPPRepository;
        public MPPService(MPPRepository MPPRepository)
        {
            _MPPRepository = MPPRepository;

        }

        public async Task<bool?> generateApprovalMPP()
        {
            try
            {
                var document = new PdfDocument();

                var contentPdf = await _MPPRepository.getAllDataApprovalMPP();

                string htmlcontent = SetTableMPP(contentPdf);

                PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A5);

                using (MemoryStream ms = new MemoryStream())
                {

                    document.Save(ms);
                    var filename = "MPP_01_21.pdf";
                    byte[] response = ms.ToArray();
                    string fullPath = Path.Combine("C:\\AllProject\\Change Request\\2024\\MPP\\MPP.File", filename);
                    File.WriteAllBytes(fullPath, response);

                }

                return true;

            }
            catch
            {

                return false;

            }


        }

        public string setSpanValue(string val, string color = "black", string bold = "normal")
        {
            string results = string.Empty;
            results = "<span style='font-family:Arial, Helvetica, sans-serif;color:" + color + "; font-weight: " + bold + "; font-size: 10px;'>" + val + "</span>";
            return results;
        }


        private string SetTableMPP(IEnumerable<T_MsMPP?> contentPDF)
        {

            string htmlcontent = "<div style='margin-top:-27px; padding-top:10px;'>";
            htmlcontent += "<h4 style='text-align:center'><span style='font-family:Arial,Helvetica,sans-serif;font-size:10px'>";
            htmlcontent += "<strong>&nbsp;Mohon segera dilakukan pengurangan Tenaga Kerja atau pembaharuan persetujuan over TK pada PT berikut</strong><br>";

            htmlcontent += "</span></h4>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align: center;'>";
            htmlcontent += "<table style='border: 0.75px solid black; border-collapse: collapse; width: 90%;margin-left:25px;'>";
            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:white;' colspan=7>" + setSpanValue("MONITORING MPP REGION RIAU FR ", bold: "bold") + "</td></tr>";
            
            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' rowspan=2>"+ setSpanValue("No", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' colspan=2>"+ setSpanValue("Bisnis Unit", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' colspan=3>"+ setSpanValue("Tenaga Kerja", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' colspan=3>"+ setSpanValue("Approval COO", "white") + "</td></tr>";

            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>"+ setSpanValue("Budget TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>"+ setSpanValue("Aktual TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>"+ setSpanValue("Variance over", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>"+ setSpanValue("Masa Berlaku", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>"+ setSpanValue("Catatan", "white") + "</td></tr>";
          
            int totalAktual = 0;
            int totalBdgt = 0;
            int totalVariance = 0;

            foreach (var dataResponse in contentPDF)
            {

                htmlcontent += "<tr>";

                foreach (var property in typeof(T_MsMPP).GetProperties())
                {

                    object? value = property.GetValue(dataResponse);

                    if (property.Name.ToLower().Contains("aktual"))
                    {

                        totalAktual += totalAktual + Convert.ToInt32(value);

                    }
                    else if (property.Name.ToLower().Contains("bdgt"))
                    {

                        totalBdgt += totalBdgt + Convert.ToInt32(value);
                    }
                    else if (property.Name.Contains("variance"))
                    {

                        totalVariance += totalVariance + Convert.ToInt32(value);
                    }

                    htmlcontent += "<td style='padding: 0; margin: 0; font-size:10px; border: 1px solid black; height: 15px; border-collapse: collapse;'>" + value + "</td>";

                }

                htmlcontent += "</tr>";

            }

            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:white;' colspan=2>"+ setSpanValue("Total") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>"+ setSpanValue(totalBdgt.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>"+ setSpanValue(totalAktual.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>"+ setSpanValue(totalVariance.ToString()) + "</td></tr>";

            htmlcontent += "</table>";
            htmlcontent += "</div>";

            return htmlcontent;

        }


    }
}