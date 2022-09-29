using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using DataGridViewAutoFilter;
using System.Collections;
using System.Reflection;

namespace webservice
{
    public partial class Form1 : Form
    {
        webservice web;
        Byte[] filebyte;
        private static string config_access = string.Empty;
        public string str;

        public Form1()
        {
            InitializeComponent();
            tbx_webservice.Text = Config.GetConfig("webservice");
            tbx_logid.Text = Config.GetConfig("logid");
            tbx_password.Text = Config.GetConfig("password");
            tbx_key.Text = Config.GetConfig("key");
        }

        private void Form1_Load(object sender, EventArgs e)
        {        
            webservice s = new webservice(tbx_webservice.Text);
            web = s;
            dateTimePicker1.Text = DateTime.Now.AddDays(-1).ToString();
            this.dataGridView1.AutoResizeColumns();
        }

        #region 登录Login
        //获取key
        private void btn_login_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbx_webservice.Text))
            {
                Config.SaveConfig("webservice", tbx_webservice.Text.Trim());
            }
            if (!String.IsNullOrEmpty(tbx_logid.Text))
            {
                Config.SaveConfig("logid", tbx_logid.Text.Trim());
            }
            if (!String.IsNullOrEmpty(tbx_password.Text))
            {
                Config.SaveConfig("password", tbx_password.Text.Trim());
            }
            if (!String.IsNullOrEmpty(tbx_key.Text))
            {
                Config.SaveConfig("key", tbx_key.Text.Trim());
            }
            try
            {                
                if(tbx_webservice.Text.Length == 0 || tbx_logid.Text.Length == 0 || tbx_password.Text.Length == 0)
                {
                    MessageBox.Show("请输入登录信息", "提示");
                }
                else
                {
                    object[] adas = { tbx_logid.Text.Trim(), tbx_password.Text.Trim() };
                    key.Text = (web.login(adas)).ToString();
                    tbx_key.Text = key.Text;
                }                    
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                throw;
            }

        }
        #endregion

        #region 根据医院条码返回图片报告单byte格式
        /// <summary>
        /// 根据医院条码返回图片报告单byte格式（假如有多张报告单，将合并成一张）
        /// </summary>
        private void btn_GetSearchStringSampleByCustomerCodeToByte_Click(object sender, EventArgs e)
        {
            if(key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { tbx_yytm.Text.Trim(), tbx_logid.Text.Trim(), key.Text };
                filebyte = web.GetSearchStringSampleByCustomerCodeToByte(parameter) as byte[];

                //BaseToJPG_Form BaseToJPG = new BaseToJPG_Form();
                //str = System.Text.Encoding.Default.GetString(filebyte);
                //BaseToJPG.ShowDialog();

                base64.output(filebyte, tbx_yytm.Text.Trim(), "jpg");                
                MessageBox.Show("生成" + tbx_yytm.Text.Trim() + "条码图片报告成功", "提示");                
            }
        }
        #endregion

        #region 根据医院条码返回PDF格式的报告单
        /// <summary>
        /// 根据医院条码返回PDF格式的报告单
        /// </summary>
        private void btn_GetByteReportByYYtm_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { tbx_yytm.Text.Trim(), key.Text };
                filebyte = web.GetByteReportByYYtm(parameter) as byte[];
                base64.output(filebyte, tbx_yytm.Text.Trim(), "pdf");
                MessageBox.Show("生成" + tbx_yytm.Text.Trim() + "条码PDF报告成功", "提示");
            }
        }
        #endregion

        #region 根据艾迪康条码 获取图片GetSearchStringSampleByAdiconCodeToByte
        private void btn_GetSearchStringSampleByAdiconCodeToByte_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { tbx_adktm.Text.Trim(), tbx_logid.Text.Trim(),key.Text };
                filebyte = web.GetSearchStringSampleByAdiconCodeToByte(parameter) as byte[];
                base64.output(filebyte, tbx_adktm.Text.Trim(), "jpg");
                MessageBox.Show("生成" + tbx_adktm.Text.Trim() + "条码图片报告成功", "提示");
            }
        }
        #endregion

        #region 根据艾迪康条码 获取PDF GetSearchByteSample
        private void btn_GetSearchByteSample_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { tbx_adktm.Text.Trim(), key.Text };
                filebyte = web.GetSearchByteSample(parameter) as byte[];
                base64.output(filebyte, tbx_adktm.Text.Trim(), "pdf");
                MessageBox.Show("生成" + tbx_adktm.Text.Trim() + "条码PDF报告成功", "提示");
            }
        }
        #endregion

        #region 可下载标本列表GetReportList
        private void btn_GetReportList_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { key.Text, dateTimePicker1.Value.ToString(), dateTimePicker2.Value.ToString(), "2","1" };
                string xmlstr = web.GetReportList(parameter) as string;
                                
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlstr);
                XmlElement root = null;
                root = xmlDoc.DocumentElement;
                XmlNodeList listNodes = null;
                //listNodes = root.SelectNodes("/NewDataSet/listtable/PatientName");
                listNodes = root.GetElementsByTagName("listtable");

                List<DataItem_list> data = new List<DataItem_list>();
                foreach (XmlNode node in listNodes)
                {
                    try
                    {
                        var Id = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Id").InnerText;
                        var ReportType = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ReportType").InnerText;
                        var AdiconBarcode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "AdiconBarcode").InnerText;
                        var PatientName = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "PatientName").InnerText;
                        var CustomerBarcode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "CustomerBarcode").InnerText;
                        var Repno = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Repno").InnerText;
                        var Sjrq = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Sjrq").InnerText;
                        var Sjys = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Sjys").InnerText;
                        var Brnl = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Brnl").InnerText;
                        var Brxb = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Brxb").InnerText;
                        var bbzl = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "bbzl").InnerText;
                        var Bgrq = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Bgrq").InnerText;                        
                        data.Add(new DataItem_list(Id, ReportType, AdiconBarcode, PatientName, CustomerBarcode, Repno, Sjrq, Sjys, Brnl, Brxb, bbzl, Bgrq));
                    }
                    catch { }
                }
                //dataGridView1.DataSource = data;
                DataTable table = new DataTable();
                table = ListToDt(data);
                BindingSource dataSource = new BindingSource(table, null);
                dataGridView1.DataSource = dataSource;

                GetXML(xmlDoc);
            }
        }
        /// <summary>
        /// list to datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public DataTable ListToDt<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new
            DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        public DataItem_list ToModel(DataRow row)
        {
            DataItem_list contrast = new DataItem_list();
            contrast.Id = row["Id"].ToString();
            return contrast;
        }

        #endregion

        private List<DataItem_detailed> GetReportItemList(string method, string key,string tm,string type)
        {
            object[] parameter = { key,tm, type };
            string xmlstr = null;
            if (method == "GetReportItemListByAdiconBarocde")
            {
                xmlstr = web.GetReportItemListByAdiconBarocde(parameter) as string;
            }
            else if (method == "GetReportItemListByCustomerBarocde")
            {
                xmlstr = web.GetReportItemListByCustomerBarocde(parameter) as string;
            }            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlElement root = null;
            root = xmlDoc.DocumentElement;
            XmlNodeList listNodes = null;
            listNodes = root.GetElementsByTagName("item");
            List<DataItem_detailed> data = new List<DataItem_detailed>();
            GetXML(xmlDoc);
            foreach (XmlNode node in listNodes)
            {
                try
                {
                    var ItemCode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ItemCode").InnerText;
                    var ItemName_CN = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ItemName_CN").InnerText;
                    var ItemName_EN = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ItemName_EN").InnerText;
                    var Result = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Result").InnerText;
                    var ResultHint = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ResultHint").InnerText;
                    var ResultReference = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ResultReference").InnerText;
                    var ResultUnit = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "ResultUnit").InnerText;
                    var TestMethod = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "TestMethod").InnerText;
                    var Str1 = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Str1").InnerText;
                    var Str2 = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Str2").InnerText;
                    var Str3 = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "Str3").InnerText;
                    var jyjs = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "jyjs").InnerText;
                    data.Add(new DataItem_detailed(ItemCode, ItemName_CN, ItemName_EN, Result, ResultHint, ResultReference, ResultUnit, TestMethod, Str1, Str2, Str3,jyjs));
                }
                catch { }
            }
            return data;
            
        }

        #region 根据艾迪康条码 获取结果GetReportItemListByAdiconBarocde
        private void btn_GetReportItemListByAdiconBarocde_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else if(tbx_reporttype.Text == "")
            {
                MessageBox.Show("请先获取可下载标本列表，再双击数据窗口中要查看结果的记录", "提示");
            }
            else
            {                
                dataGridView2.DataSource = GetReportItemList("GetReportItemListByAdiconBarocde", key.Text, tbx_adktm.Text.Trim(), tbx_reporttype.Text.Trim());
            }
        }
        #endregion
       
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.DataSource = null;
            tbx_reporttype.Text = dataGridView1.Rows[e.RowIndex].Cells["ReportType"].Value.ToString();
            tbx_id.Text = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            tbx_adktm.Text = dataGridView1.Rows[e.RowIndex].Cells["AdiconBarcode"].Value.ToString();
            tbx_yytm.Text = dataGridView1.Rows[e.RowIndex].Cells["CustomerBarcode"].Value.ToString();
        }

        #region 根据医院条码获取GetReportItemListByCustomerBarocde
        private void btn_GetReportItemListByCustomerBarocde_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else if (tbx_reporttype.Text == "")
            {
                MessageBox.Show("请先获取可下载标本列表，再双击数据窗口中要查看结果的记录", "提示");
            }
            else
            {
                dataGridView2.DataSource = GetReportItemList("GetReportItemListByCustomerBarocde", key.Text, tbx_yytm.Text.Trim(), tbx_reporttype.Text.Trim());
            }
        }
        #endregion

        #region 根据艾迪康条码 是否检测完成ExistsReport
        private void btn_ExistsReport_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { tbx_adktm.Text.Trim(), key.Text };
                string str = web.ExistsReport(parameter) as string;
                MessageBox.Show( tbx_adktm.Text.Trim() + "  " + str, "提示");
            }
        }
        #endregion

        #region 根据医院条码是否检测完成ExistsReportByYYtm
        private void btn_sfbgyytm_Click(object sender, EventArgs e)
        {
            if (key.Text == "")
            {
                MessageBox.Show("请点击登录按钮", "提示");
            }
            else
            {
                object[] parameter = { tbx_yytm.Text.Trim(), key.Text };
                string str = web.ExistsReportByYYtm(parameter) as string;
                MessageBox.Show(tbx_yytm.Text.Trim() + "  " + str, "提示");
            }
        }
        #endregion

        #region 筛选过滤
        // 当用户按下时显示下拉列表  ALT+向下箭头或ALT+向上箭头。
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell =
                    dataGridView1.CurrentCell.OwningColumn.HeaderCell as
                    DataGridViewAutoFilterColumnHeaderCell;
                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }
        }

        // 更新筛选器状态标签。
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell
                .GetFilterStatus(dataGridView1);
            if (String.IsNullOrEmpty(filterStatus))
            {
                showAllLabel.Visible = false;
                filterStatusLabel.Visible = false;
            }
            else
            {
                showAllLabel.Visible = true;
                filterStatusLabel.Visible = true;
                filterStatusLabel.Text = filterStatus;
            }
        }

        //当用户单击“全部显示”链接时清除筛选器
        private void showAllLabel_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }

        /// <summary>
        /// 配置自动生成的列，替换其标题。带有自动筛选标题单元格的单元格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
        {
            // Continue only if the data source has been set.
            if (dataGridView1.DataSource == null)
            {
                return;
            }

            // Add the AutoFilter header cell to each column.
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell = new
                    DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }

            // Format the OrderTotal column as currency. 
            //dataGridView1.Columns["OrderTotal"].DefaultCellStyle.Format = "c";

            // Resize the columns to fit their contents.
            dataGridView1.AutoResizeColumns();
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            BaseToJPG_Form BaseToJPG = new BaseToJPG_Form();
            BaseToJPG.ShowDialog();
        }

        private void GetXML(XmlDocument xmlDoc)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(sw))
            {
                writer.Indentation = 2;  // the Indentation
                writer.Formatting = System.Xml.Formatting.Indented;
                xmlDoc.WriteContentTo(writer);
                writer.Close();
            }
            textBox1.Text = sw.ToString();
        }
    }
}
