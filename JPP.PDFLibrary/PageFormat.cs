using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JPP.PDFLibrary
{
    public class PageFormat
    {
        //SIZE IN POINTS
        private static Dictionary<string, PageFormat> formats = new Dictionary<string, PageFormat>
        {
            { "A4",new PageFormat(595,842) },
            {"LETTER",new PageFormat(612,792) }
        };
        double width;
        double height;

        public static PageFormat getKnownFormat(string name)
        {
            return formats[name.ToUpper()];
        }

        public PageFormat(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        public PageFormat(string name)
        {
            PageFormat p = getKnownFormat(name);
            this.width = p.Width;
            this.height = p.Height;
        }

        public double Width { get => this.width; set => this.width = value; }
        public double Height { get => this.height; set => this.height = value; }

    }
}
