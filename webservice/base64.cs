using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;

namespace webservice
{
    class base64
    {
        public static void output(Byte[] str, string filename, string filetype)
        {
            //生成路径
            string filePath = Path.Combine(System.Windows.Forms.Application.StartupPath, filetype);
            //如果不存在就创建 SignImgs 文件夹  
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            //转成 Base64 形式的 System.String  
            string imagebyte64 = Convert.ToBase64String(str);
            //Base64格式去除开头多余字符data:image/png;base64,
            imagebyte64 = imagebyte64.Substring(imagebyte64.IndexOf(',') + 1);
            File.WriteAllBytes(filePath + "\\" + filename + "." + filetype, Convert.FromBase64String(imagebyte64));//将base64转成文件
            //string v_OpenFolderPath = @"目录路径";
            //System.Diagnostics.Process.Start("explorer.exe", filePath);
            System.Diagnostics.Process.Start(filePath + "\\" + filename + "." + filetype);
        }

        public void Base64StringToFile(string strbase64, string strurl)
        {
            try
            {
                strbase64 = strbase64.Replace(' ', '+');
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(strbase64));
                FileStream fs = new FileStream(strurl, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] b = stream.ToArray();
                //byte[] b = stream.GetBuffer();
                fs.Write(b, 0, b.Length);
                fs.Close();

            }
            catch (Exception e)
            {

            }
        }

    }
}
