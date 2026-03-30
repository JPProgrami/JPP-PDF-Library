using PDFiumSharp;
using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JPP.PDFLibrary
{
    public class PDFEditor
    {
        private FPDF_DOCUMENT ndoc;
        private int curr_index;
        private int count;
        private PageFormat format;
        private List<PageInfo> page_info;

        public PageFormat Format { get => this.format; set => this.format = value; }

        public int CurrentPage { get => this.curr_index; set => curr_index = value; }

        public PDFEditor(PdfDocument doc)
        {
            this.ndoc = doc.PDFiumDocument;
            curr_index = doc.Pages.Count>=0 ? 0 : -1;
            count = curr_index;
            this.format = new PageFormat("LETTER");
            page_info = new List<PageInfo>();
        }

        public PDFEditor()
        {

            this.ndoc = PDFium.FPDF_CreateNewDocument();
            curr_index = -1;
            this.format = new PageFormat("LETTER");
            page_info = new List<PageInfo>();
        }

        public void addPage()
        {
            curr_index++;
            count++;
            PDFium.FPDFPage_New(ndoc, curr_index, Format.Width, Format.Height);
            page_info.Add(new PageInfo());
        }

        public void addString(string text)
        {
            FPDF_PAGE page = PDFium.FPDF_LoadPage(ndoc, curr_index);
            FPDF_PAGEOBJECT po = PDFium.FPDFPageObj_NewTextObj(ndoc, "Courier", 12);
            PDFium.FPDFText_SetText(po, text);
            PDFium.FPDFPageObj_Transform(po, 1, 0, 0, 1, 10, this.format.Height-12-10);
            PDFium.FPDFPage_InsertObject(page, ref po);
            PDFium.FPDFPage_GenerateContent(page);
            FPDF_PAGEOBJECT obj = PDFium.FPDFPage_GetObject(page, PDFium.FPDFPage_CountObjects(page) - 1);
            page_info[curr_index].add(PDFObjectType.String);
        }
        bool ImageFormatHasAlpha(PixelFormat fmt)
        {
            return (fmt & PixelFormat.Alpha) == PixelFormat.Alpha;
        }
        public void addPicture(string src)
        {
            //Currently tested for .jpg and .png .
            FPDF_PAGE page = PDFium.FPDF_LoadPage(ndoc, curr_index);
            Bitmap bmp = new Bitmap(src);
            FPDF_BITMAP pdfb = PDFium.FPDFBitmap_Create(bmp.Width, bmp.Height, ImageFormatHasAlpha(bmp.PixelFormat));
            PixelConverter.ToBGRA(bmp, ref pdfb);
            FPDF_PAGEOBJECT po = PDFium.FPDFPageObj_NewImageObj(ndoc);
            PDFium.FPDFImageObj_SetBitmap(ref page, 0, po, pdfb);
            PDFium.FPDFPageObj_Transform(po, bmp.Width, 0, 0, bmp.Height, 100, 100);
            PDFium.FPDFPage_InsertObject(page, ref po);
            PDFium.FPDFPage_GenerateContent(page);
            page_info[curr_index].add(PDFObjectType.Image);

        }

        public bool removeObject(string key)
        {
            if (page_info[curr_index].hasObj(key))
            {
                FPDF_PAGE page = PDFium.FPDF_LoadPage(ndoc, curr_index);
                FPDF_PAGEOBJECT po = PDFium.FPDFPage_GetObject(page,page_info[curr_index][key]);
                bool ret = PDFium.FPDFPage_RemoveObject(page, ref po);
                page_info[curr_index].remove(key);
                PDFium.FPDFPage_GenerateContent(page);
                return ret;
            }
            return false;
        }

        public bool Save(string path)
        {
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                return PDFium.FPDF_SaveAsCopy(ndoc, stream, SaveFlags.None);
            }
        }
    }
}
