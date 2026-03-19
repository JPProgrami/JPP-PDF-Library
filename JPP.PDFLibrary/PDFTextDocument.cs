using PDFiumSharp;
using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPP.PDFLibrary
{
    public class PDFTextDocument
    {
        public List<PDFTextPage> Pages { get; }

        public string Text { get
            {
                StringBuilder sb = new StringBuilder();
                int pgcount = 0;
                foreach(PDFTextPage p in this.Pages)
                {
                    try
                    {
                        sb.Append(p.Text);
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message + "\n PANIC!!! AT PDF PAGE " + pgcount);
                    }
                    sb.Append(Environment.NewLine+Environment.NewLine+Environment.NewLine + "-- PAGE " +(p.Index+1)+" / "+this.Pages.Count+" --"+Environment.NewLine+Environment.NewLine+Environment.NewLine);
                    pgcount++;
                }
                return sb.ToString();
            } }

        public PDFTextDocument(PdfDocument doc)
        {
            List<PDFTextPage> pag = new List<PDFTextPage>();
            foreach (PdfPage p in doc.Pages)
            {
                FPDF_PAGE temp_page = PDFium.FPDF_LoadPage(doc.PDFiumDocument, p.Index);
                FPDF_TEXTPAGE text_page = PDFium.FPDFText_LoadPage(temp_page);
                pag.Add(new PDFTextPage(text_page, p.Width, p.Height, p.Index));
            }
            this.Pages = pag;
        }

        public PDFTextDocument(string path, string password = null) : this(new PdfDocument(path, password))
        {

        }

    }
}
