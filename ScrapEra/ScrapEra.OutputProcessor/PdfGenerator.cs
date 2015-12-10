using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ScrapEra.ScrapLogger;

namespace ScrapEra.OutputProcessor
{
    public class PdfGenerator
    {
        public static void GeneratePdfSingleDataType(string filePath, string title, List<string> content)
        {
            try
            {
                Logger.LogI("GeneratePDF -> " + filePath);
                var doc = new Document(PageSize.A3, 36, 72, 72, 144);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    doc.AddTitle(title);
                    doc.AddAuthor(Environment.MachineName);
                    doc.Add(
                        new Paragraph("Title : " + title + Environment.NewLine + "ServerName : " +
                                      Environment.MachineName +
                                      Environment.NewLine + "Author : " + Environment.UserName));
                    foreach (var para in content)
                    {
                        doc.NewPage();
                        doc.Add(new Paragraph(para));
                    }
                    doc.Close();
                }
                ApplyWaterMark(filePath);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex.Source + " -> " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void GeneratePdfSingleDataType(string filePath, string title, string content)
        {
            try
            {
                Logger.LogI("GeneratePDF -> " + filePath);
                var doc = new Document(PageSize.A3, 36, 72, 72, 144);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    doc.AddTitle(title);
                    doc.AddAuthor(Environment.MachineName);
                    doc.Add(
                        new Paragraph("Title : " + title + Environment.NewLine + "ServerName : " +
                                      Environment.MachineName +
                                      Environment.NewLine + "Author : " + Environment.UserName));
                    doc.NewPage();
                    doc.Add(new Paragraph(content));
                    doc.Close();
                }
                ApplyWaterMark(filePath);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex.Source + " -> " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static void ApplyWaterMark(string filePath)
        {
            Logger.LogI("ApplyWatermark -> " + filePath);
            var watermarkedFile = Path.GetFileNameWithoutExtension(filePath) + "-w.pdf";
            using (var reader1 = new PdfReader(filePath))
            {
                using (var fs = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var stamper = new PdfStamper(reader1, fs))
                {
                    var pageCount = reader1.NumberOfPages;
                    var layer = new PdfLayer("WatermarkLayer", stamper.Writer);
                    for (var i = 1; i <= pageCount; i++)
                    {
                        var rect = reader1.GetPageSize(i);
                        var cb = stamper.GetUnderContent(i);
                        cb.BeginLayer(layer);
                        cb.SetFontAndSize(BaseFont.CreateFont(
                            BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 50);
                        var gState = new PdfGState {FillOpacity = 0.25f};
                        cb.SetGState(gState);
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.BeginText();
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                            "(c)2015 ScrapEra", rect.Width/2, rect.Height/2, 45f);
                        cb.EndText();
                        cb.EndLayer();
                    }
                }
            }
            File.Delete(filePath);
        }
    }
}