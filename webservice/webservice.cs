using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;

namespace webservice
{
    public class webservice
    {
        Type t = null;
        object obj = null;
        public  webservice(string url)
        {
            //服务地址，该地址可以放到程序的配置文件中，这样即使服务地址改变了，也无须重新编译程序。
            //string url = "http://110.90.112.61:8082/FZWSForCustomer/ADReportWebService.asmx";

            //客户端代理服务命名空间，可以设置成需要的值。
            //string ns = string.Format("ProxyServiceReference");


            //获取WSDL
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url + "?WSDL");
            ServiceDescription sd = ServiceDescription.Read(stream);//服务的描述信息都可以通过ServiceDescription获取
            string classname = sd.Services[0].Name;

            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace("");

            //生成客户端代理类代码
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();

            //设定编译参数
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");

            //编译代理类
            CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
            if (cr.Errors.HasErrors == true)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }

            //生成代理实例，并调用方法
            Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType("" + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);

            this.obj = obj;
            this.t = t;
        }

        /// <summary>
        /// 用户验证获得授权码 Login
        /// </summary>
        /// <param name="parameter">用户名，密码</param>
        /// <returns></returns>
        public object login( Object[] parameter)
        {
            try
            {
                MethodInfo key = t.GetMethod("Login");
                object keyReturn = key.Invoke(obj, parameter);
                return keyReturn;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                throw;
            }            
        }

        /// <summary>
        /// 根据医院条码返回图片报告单byte格式（假如有多张报告单，将合并成一张）GetSearchStringSampleByCustomerCodeToByte
        /// </summary>
        /// <param name="parameter">医院条码，客户代号，key</param>
        /// <returns></returns>
        public object GetSearchStringSampleByCustomerCodeToByte(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetSearchStringSampleByCustomerCodeToByte");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }

        /// <summary>
        /// 根据医院条码返回PDF格式的报告单GetByteReportByYYtm
        /// </summary>
        /// <param name="parameter">医院条码，key</param>
        /// <returns></returns>
        public object GetByteReportByYYtm(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetByteReportByYYtm");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }

        public object GetSearchStringSampleByAdiconCodeToByte(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetSearchStringSampleByAdiconCodeToByte");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }
        /// <summary>
        /// 根据艾迪康条码返回PDF格式的报告单GetSearchByteSample
        /// </summary>
        /// <param name="parameter">艾迪康条码，key</param>
        /// <returns></returns>
        public object GetSearchByteSample(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetSearchByteSample");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }

        /// <summary>
        /// 获取可下载标本列表 GetReportList
        /// Key＝有效授权码
        /// BeginDateTime＝起始时间
        /// EndDateTime＝结束时间
        /// TypeDateTime＝1=按采集时间统计，2＝按报告时间统计；AgainFlag:1=重新下载已下载过的标本，0＝只下载未下载的标本
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object GetReportList(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetReportList");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }
        /// <summary>
        /// 根据艾迪康条码获取标本检测结果或诊断结果 GetReportItemListByAdiconBarocde
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object GetReportItemListByAdiconBarocde(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetReportItemListByAdiconBarocde");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }
        /// <summary>
        /// 根据客户条码（医院条码）获取标本检测结果或诊断结果。Key＝有效授权码，CustomerBarcode＝客户条码（医院条码）。
        /// GetReportItemListByCustomerBarocde
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object GetReportItemListByCustomerBarocde(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("GetReportItemListByCustomerBarocde");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }
        /// <summary>
        /// 根据艾迪康条码判断报告单是否检测完成（只判断常规的报告单）
        /// ExistsReport
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object ExistsReport(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("ExistsReport");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }
        /// <summary>
        /// 根据医院条码判断报告单是否检测完成（只判断常规的报告单）
        /// ExistsReportByYYtm
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object ExistsReportByYYtm(Object[] parameter)
        {
            MethodInfo key = t.GetMethod("ExistsReportByYYtm");
            object keyReturn = key.Invoke(obj, parameter);
            return keyReturn;
        }
    }
}
