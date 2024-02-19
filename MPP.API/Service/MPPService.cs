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

        public async Task<bool?> generateApprovalMPP(string company, string location, string tahun, string bulan)
        {
            try
            {



                var document = new PdfDocument();

                var contentPdf = await _MPPRepository.getAllDataApprovalMPP(company, location, tahun, bulan);

                if (contentPdf != null)
                {

                    string htmlcontent = SetTableMPP(contentPdf);

                    PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A5);

                    using (MemoryStream ms = new MemoryStream())
                    {

                        document.Save(ms);
                        var filename = "MPP_ " + company + "_" + location + ".pdf";
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
              htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:white;' colspan=6>" + setSpanValue("MONITORING MPP REGION RIAU FR ", bold: "bold") + "</td></tr>";

            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' rowspan=2>" + setSpanValue("No", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' colspan=2>" + setSpanValue("Bisnis Unit", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;' colspan=2>" + setSpanValue("Tenaga Kerja", "white") + "</td></tr>";

            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Budget TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Aktual TK", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Variance over", "white") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:#2E6443;'>" + setSpanValue("Masa Berlaku", "white") + "</td></tr>";

            int totalAktual = 0;
            int totalBdgt = 0;
            int totalVariance = 0;
            bool createColumn = true ; 


             foreach (var dataResponse in contentPDF)
             {

              htmlcontent += "<tr>";

               foreach (var property in typeof(T_MsMPP).GetProperties())
              {

                  object? value = property.GetValue(dataResponse);

                    if (property.Name.ToLower().Contains("Aktual"))
                    {

                        totalAktual += totalAktual + Convert.ToInt32(value);

                    }
                    else if (property.Name.ToLower().Contains("totalBdgt"))
                    {

                        totalBdgt += totalBdgt + Convert.ToInt32(value);
                    }
                    else if (property.Name.Contains("varianceOver"))
                    {

                        totalVariance += totalVariance + Convert.ToInt32(value);
                    }
                     else if (property.Name.Contains("EmailLevel") || property.Name.Contains("MasaBerlaku"))
                    {

                        createColumn = false ;
                    }

                    if  (createColumn != false) {

                        htmlcontent += "<td style='padding: 0; margin: 0; font-size:10px; border: 1px solid black; height: 15px; border-collapse: collapse;'>" + value + "</td>";

                    }else{

                            createColumn = true ;

                    }
                    
                 
             }

              htmlcontent += "</tr>";

            }

            htmlcontent += "<tr><td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;background-color:white;' colspan=2>" + setSpanValue("Total") + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalBdgt.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalAktual.ToString()) + "</td>";
            htmlcontent += "<td style='padding: 0; margin: 0;border: 1px solid black;height: 12px; border-collapse: collapse;width:10px;background-color:white;'>" + setSpanValue(totalVariance.ToString()) + "</td></tr>";

            htmlcontent += "</table>";
            htmlcontent += "</div>";

            return htmlcontent;

        }


    }
}