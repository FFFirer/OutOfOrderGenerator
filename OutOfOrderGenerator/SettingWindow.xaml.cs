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
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OutOfOrderGenerator.Handlers;

namespace OutOfOrderGenerator
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        //视图模型
        public GongsiNameList list { get; set; }
        public DataHandler dh { get; set; }
        public string Selected { get; set; }
        public const string DefaultName = "山西杏花村汾酒厂股份有限公司";
        public SettingWindow()
        {
            InitializeComponent();
            this.Closing += SettingWindow_Closing;
            dh = new DataHandler();
            list = dh.ReadData();
            listGongsi.ItemsSource = list;
        }

        private void SettingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //列表中 数量小于1时，界面显示默认的
            if(list.Count < 1)
            {
                Selected = DefaultName;
            }
            if (dh != null)
            {
                dh.WriteData(list);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SetValueWindow setting = new SetValueWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            setting.ShowDialog();
            if(setting.gcname != "" && setting.gcname != null)
            {
                GongsiName gc = new GongsiName() { Name = setting.gcname };
                list.Add(gc);
                listGongsi.ScrollIntoView(listGongsi.Items[listGongsi.Items.Count - 1]);
            }
        }

        private void listGongsi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GongsiName gongsiName = (GongsiName)this.listGongsi.SelectedItem;
            Selected = gongsiName.Name;
            this.Close();
        }

        private void mDel_Click(object sender, RoutedEventArgs e)
        {
            GongsiName gongsi = (GongsiName)this.listGongsi.SelectedItem;
            if(Selected == gongsi.Name)
            {
                Selected = DefaultName;
            }
            list.Remove(gongsi);
        }

        private void mEdit_Click(object sender, RoutedEventArgs e)
        {
            GongsiName gongsi = (GongsiName)this.listGongsi.SelectedItem;
            SetValueWindow setting = new SetValueWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            setting.SetValue(gongsi.Name);
            setting.ShowDialog();
            if(Selected == gongsi.Name)
            {
                Selected = setting.gcname;
            }
            gongsi.Name = setting.gcname;
        }
    }
}
