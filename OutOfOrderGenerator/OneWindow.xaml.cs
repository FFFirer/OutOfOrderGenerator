using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OutOfOrderGenerator.ViewModels;
using System.Reflection;
using System.Configuration;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using ICSharpCode.SharpZipLib;
using NPOI.HSSF.Util;

namespace OutOfOrderGenerator
{
    /// <summary>
    /// OneWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OneWindow : Window
    {
        //ViewModel
        public OneViewModel model { get; set; }
        //保存路径
        public string SavePath { get; set; }
        public OneWindow()
        {
            InitializeComponent();
            this.Closing += OneWindow_Closing;
            model = new OneViewModel();
            txtBuckleWeight.SetBinding(TextBox.TextProperty, new Binding("BuckleWeight") { Source = model });
            txtCarNum.SetBinding(TextBox.TextProperty, new Binding("CarNum") { Source = model });
            txtCertificateNum.SetBinding(TextBox.TextProperty, new Binding("CertificateNum") { Source = model });
            txtCollectName.SetBinding(TextBox.TextProperty, new Binding("CollectName") { Source = model });
            txtGrainWeight.SetBinding(TextBox.TextProperty, new Binding("GrainWeight") { Source = model });
            txtGrossWeight.SetBinding(TextBox.TextProperty, new Binding("GrossWeight") { Source = model });
            txtLineItemsNum.SetBinding(TextBox.TextProperty, new Binding("LineItemsNum") { Source = model });
            txtMaterialCode.SetBinding(TextBox.TextProperty, new Binding("MaterialCode") { Source = model });
            txtMaterialName.SetBinding(TextBox.TextProperty, new Binding("MaterialName") { Source = model });
            txtNetWeight.SetBinding(TextBox.TextProperty, new Binding("NetWeight") { Source = model });
            txtNum.SetBinding(TextBox.TextProperty, new Binding("Num") { Source = model });
            txtProviderName.SetBinding(TextBox.TextProperty, new Binding("ProviderName") { Source = model });
            txtTare.SetBinding(TextBox.TextProperty, new Binding("Tare") { Source = model });
            SavePath = ConfigurationManager.AppSettings["SavePath"];
            if (SavePath == "")
            {
                SavePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            }
            this.Title = string.Format("Excel生成器————保存在 {0}", SavePath);
        }

        //窗体关闭事件
        private void OneWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        //测试
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string msg = string.Empty;
            msg += string.Format("{0} {1}\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()); ;
            
            foreach(PropertyInfo p in model.GetType().GetProperties())
            {
                msg += string.Format("{0}:{1}\n", p.Name, p.GetValue(model, null));
            }

            msg += string.Format("{0}\n", GetNowTime());
            MessageBox.Show(msg);
        }
        
        //但会当前日期时间的字符串
        public string GetNowTime()
        {
            string time = string.Empty;
            time += DateTime.Now.Year;
            time += "年";
            time += DateTime.Now.Month <= 10 ? "0" : "";
            time += DateTime.Now.Month;
            time += "月";
            time += DateTime.Now.Day <= 10 ? "0" : "";
            time += DateTime.Now.Day;
            time += "日";
            time += DateTime.Now.Hour <= 10 ? "0" : "";
            time += DateTime.Now.Hour;
            time += "时";
            time += DateTime.Now.Minute <= 10 ? "0" : "";
            time += DateTime.Now.Minute;
            time += "分";
            time += DateTime.Now.Second <= 10 ? "0" : "";
            time += DateTime.Now.Second;
            time += "秒";
            return time;
        }
        
        //选择保存路径
        private void btnSelectSavePath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog f_dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = f_dialog.ShowDialog();

            if(result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            SavePath = f_dialog.SelectedPath.Trim();
            this.Title = string.Format("Excel生成器————保存在 {0}", SavePath);
            //lblFolderPath.Content = SavePath;
            SavePath2Config();
        }
        
        //保存进配置
        private void SavePath2Config()
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfg.AppSettings.Settings["SavePath"].Value = SavePath;
            cfg.Save();
        }

        //写入Excel文件
        public void Write2Excel(string filepath)
        {
            //新建xls工作簿
            IWorkbook wb;
            string extension = System.IO.Path.GetExtension(filepath);
            if (extension.Equals(".xls"))
            {
                wb = new HSSFWorkbook();    // 新建xls工作簿
            }
            else
            {
                wb = new XSSFWorkbook();    // 新建xlsx工作簿
            }
            //设置样式
            //1、宋体，22，加粗，水平居中，垂直居中
            //2、宋体，11，水平居中，垂直居中
            //3、宋体，8，水平居中，垂直居中
            //行高：13.5*3，27*1，13.5*14
            //列宽：9.88*1，8.38*10
            IFont font1 = wb.CreateFont();
            font1.FontName = "宋体";
            font1.Boldweight = (short)FontBoldWeight.Bold;
            font1.FontHeightInPoints = 22;

            IFont font2 = wb.CreateFont();
            font2.FontName = "宋体";
            font2.Boldweight = (short)FontBoldWeight.Normal;
            font2.FontHeightInPoints = 11;

            IFont font3 = wb.CreateFont();
            font3.FontName = "宋体";
            font3.Boldweight = (short)FontBoldWeight.Normal;
            font3.FontHeightInPoints = 8;

            ICellStyle style1 = wb.CreateCellStyle();
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//文字水平对齐方式
            style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
            //设置边框
            style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.WrapText = true;//自动换行
            style1.SetFont(font1);

            ICellStyle style2 = wb.CreateCellStyle();
            style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//文字水平对齐方式
            style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
            //设置边框
            style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.WrapText = true;//自动换行
            style2.SetFont(font2);

            ICellStyle style3 = wb.CreateCellStyle();
            style3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//文字水平对齐方式
            style3.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
            //设置边框
            style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.WrapText = true;//自动换行
            style3.SetFont(font3);

            //创建一个表单
            ISheet sheet = wb.CreateSheet("sheet1");
            //设置列宽
            int[] columnWidth = { 11, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            for(int i = 0; i<columnWidth.Length; i++)
            {
                //设置列宽度，256*字符数，因为单位是1/256个字符， 行高的单位是1/20个点
                sheet.SetColumnWidth(i, 256 * columnWidth[i]);
            }

            //准备数据
            int rowCount = 18, colCount = 11;
            string[,] data =
            {
                {"山西杏花村汾酒厂股份有限公司","","","","","","","","","","" },
                {"","","","","","","","","","","" },
                {"","","","","","","","","","","" },
                {"","","","计量单","","","","",GetNowTime(),"","" },
                {"计量单号",model.Num,"","","车号",model.CarNum,"","","","","" },
                {"","","","","","","","","","","" },
                {"凭证号",model.CertificateNum,"","","行项目号",model.LineItemsNum,"","","","","" },
                {"","","","","","","","","","","" },
                {"物料编码",model.MaterialCode,"","","物料名称",model.MaterialName,"","","","","" },
                {"","","","","","","","","","","" },
                {"供方",model.ProviderName,"","","收方",model.CollectName,"","","","","" },
                {"","","","","","","","","","","" },
                {"毛重(kg)",model.GrossWeight,"","","皮重(kg)",model.Tare,"","","","","" },
                {"","","","","","","","","","","" },
                {"粮重(kg)",model.GrainWeight,"","","扣重(%)",model.BuckleWeight,"","","","","" },
                {"","","","","","","","","","","" },
                {"净重(kg)",model.NetWeight,"","","","","","","","","" },
                {"","","","","","","","","","","" }
            };

            //填充数据
            IRow row;
            ICell cell;
            for(int i = 0;i < rowCount; i++)
            {
                row = sheet.CreateRow(i);
                for(int j = 0; j < colCount; j++)
                {
                    cell = row.CreateCell(j);   //创建第j列
                    if(i < 3)
                    {
                        cell.CellStyle = style1;
                    }
                    else if (i == 3 && j < 8)
                    {
                        cell.CellStyle = style1;
                    }
                    else if(i == 3 && j >= 8)
                    {
                        cell.CellStyle = style3;
                    }
                    else
                    {
                        cell.CellStyle = style2;
                    }
                    cell.SetCellValue(data[i, j]);
                }
            }
            
            //合并单元格
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, 10));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 3, 5));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 8, 10));
            for(int i=2; i <= 7; i++)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2 * i, 2 * i + 1, 0, 0));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2 * i, 2 * i + 1, 1, 3));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2 * i, 2 * i + 1, 4, 4));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2 * i, 2 * i + 1, 5, 10));
            }
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(16, 17, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(16, 17, 1, 10));

            try
            {
                FileStream fs = File.OpenWrite(filepath);
                wb.Write(fs);   //向这个路径写入数据
                fs.Flush();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Format("{0}\\{1}.xls", SavePath, GetNowTime());
            MessageBoxResult mbr = MessageBox.Show(string.Format("文件将要保存到{0}", filePath), "是否保存？", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                lblInfo.Content = "正在保存中...";
                Write2Excel(filePath);
                lblInfo.Content = "保存完成！";
            }
        }
    }
}
