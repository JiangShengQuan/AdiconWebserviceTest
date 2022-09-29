using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webservice
{
    public class DataItem_list
    {
        public string Id { get; set; }
        public string ReportType { get; set; }
        public string AdiconBarcode { get; set; }
        public string PatientName { get; set; }
        public string CustomerBarcode { get; set; }
        public string Repno { get; set; }
        public string Sjrq { get; set; }
        public string Sjys { get; set; }
        public string Brnl { get; set; }
        public string Brxb { get; set; }
        public string bbzl { get; set; }
        public string Bgrq { get; set; }
        public DataItem_list(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k, string l)
        {
            Id = a;
            ReportType = b;
            AdiconBarcode = c;
            PatientName = d;
            CustomerBarcode = e;
            Repno = f;
            Sjrq = g;
            Sjys = h;
            Brnl = i;
            Brxb = j;
            bbzl = k;
            Bgrq = l;
        }

        public DataItem_list()
        {
        }
    }
}
