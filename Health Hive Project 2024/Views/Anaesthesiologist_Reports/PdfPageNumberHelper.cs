using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Health_Hive_Project_2024.Views.Anaesthesiologist_Reports
{
    public class PdfPageNumberHelper : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Page " + writer.PageNumber, 520, 30, 0);
            cb.EndText();
        }
    }

}
