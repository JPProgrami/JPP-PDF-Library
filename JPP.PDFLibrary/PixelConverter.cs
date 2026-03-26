using PDFiumSharp;
using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JPP.PDFLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RGBA
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BGRA
    {
        public byte b;
        public byte g;
        public byte r;
        public byte a;
    }

    public unsafe class PixelConverter
    {
        public static void ToBGRA(Bitmap bmp,ref FPDF_BITMAP pdfb)
        {
            int width = bmp.Width;
            int height = bmp.Height;


            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr srcptr = data.Scan0;
            IntPtr dstptr = PDFium.FPDFBitmap_GetBuffer(pdfb);

            int src_stride = data.Stride;
            int dst_stride = PDFium.FPDFBitmap_GetStride(pdfb);
            unsafe
            {
                byte* isrc = (byte*)srcptr;
                byte* idst = (byte*)dstptr;

                for (int y = 0; y < height; y++)
                {

                    byte* srcRow = isrc + (y * src_stride);
                    byte* dstRow = idst + (y * dst_stride);

                    for (int x = 0; x < width; x++)
                    {
                        byte b = srcRow[x * 4 + 0];
                        byte g = srcRow[x * 4 + 1];
                        byte r = srcRow[x * 4 + 2];
                        byte a = srcRow[x * 4 + 3];

                        dstRow[x * 4 + 0] = b;
                        dstRow[x * 4 + 1] = g;
                        dstRow[x * 4 + 2] = r;
                        dstRow[x * 4 + 3] = a;
                    }
                }

            }
            bmp.UnlockBits(data);
        }

    }
}
