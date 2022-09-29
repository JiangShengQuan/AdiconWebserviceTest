using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webservice
{
    class DataItem_detailed
    {
        public string ItemCode { get; set; }
        public string ItemName_CN { get; set; }
        public string ItemName_EN { get; set; }
        public string Result { get; set; }
        public string ResultHint { get; set; }
        public string ResultReference { get; set; }
        public string ResultUnit { get; set; }
        public string TestMethod { get; set; }
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }
        public string jyjs { get; set; }
        public DataItem_detailed(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k,string l)
        {
            ItemCode = a;
            ItemName_CN = b;
            ItemName_EN = c;
            Result = d;
            ResultHint = e;
            ResultReference = f;
            ResultUnit = g;
            TestMethod = h;
            Str1 = i;
            Str2 = j;
            Str3 = k;
            jyjs = l;
        }
    }
}
