using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JPP.PDFLibrary
{
    public class PageInfo
    {
        List<string> pdfobjects;
        private long managed_textobj_count = 0;
        private long managed_imageobj_count = 0;
        private long managed_generalobj_count = 0;
        public int ObjectCount { get { return this.pdfobjects.Count; } }

        public long ManagedTextObjCount { get => this.managed_textobj_count; set => this.managed_textobj_count = value; }
        public long ManagedImageObjCount { get => this.managed_imageobj_count; set => this.managed_imageobj_count = value; }
        public long ManagedGeneralObjCount { get => this.managed_generalobj_count; set => this.managed_generalobj_count = value; }

        public PageInfo()
        {
            this.pdfobjects = new List<string>();
        }

        public int this[string name]
        {
            get
            {
                return pdfobjects.FindIndex(x => x == name); ;
            }
        }

        public bool add(PDFObjectType type, string key="")
        {
            if(type==PDFObjectType.Image)
            {
                managed_imageobj_count++;
                if (string.IsNullOrEmpty(key))
                {
                    key = "img"+managed_imageobj_count;
                }
            }
            else if(type==PDFObjectType.String)
            {
                managed_textobj_count++;
                if (string.IsNullOrEmpty(key))
                {
                    key = "str"+managed_textobj_count;
                }
            }
            else if(type==PDFObjectType.General)
            {
                managed_generalobj_count++;
                if (string.IsNullOrEmpty(key))
                {
                    key = "obj"+managed_generalobj_count;
                }
            }
                if (!pdfobjects.Exists(x => x == key))
                {
                    pdfobjects.Add(key);
                    return true;
                }
            return false;
        }
        public void remove()
        {
            pdfobjects.RemoveAt(ObjectCount-1);
        }
        public bool remove(string key)
        {
            if (!pdfobjects.Exists(x => x == key))
            {
                pdfobjects.RemoveAt(pdfobjects.FindIndex(x=>x==key));
                return true;
            }
            return false;
        }

        public void removeAt(int i)
        {
            pdfobjects.RemoveAt(i);
        }

        public bool hasObj(string key)
        {
            return pdfobjects.Exists(x => x == key);
        }




    }
}
