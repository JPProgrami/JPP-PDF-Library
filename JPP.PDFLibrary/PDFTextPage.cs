using PDFiumSharp;
using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPP.PDFLibrary
{
    public class PDFTextPage
    {
        public string Text { get { return PDFium.FPDFText_GetBoundedText(this.fPDF_TEXTPAGE, 0, 0, this.width, this.height); } }

        public double Width { get => this.width; set => this.width = value; }
        public double Height { get => this.height; set => this.height = value; }

        public int Index => this.index;

        private FPDF_TEXTPAGE fPDF_TEXTPAGE;
        private double width;
        private double height;
        private readonly int index;

        public PDFTextPage(FPDF_TEXTPAGE fPDF_TEXTPAGE,double width,double height,int index)
        {
            this.fPDF_TEXTPAGE = fPDF_TEXTPAGE;
            this.width = width;
            this.height = height;
            this.index = index;
        }

    }
}
